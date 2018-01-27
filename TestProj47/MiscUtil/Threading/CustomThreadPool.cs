using System;
using System.Collections.Generic;
using System.Threading;
using HSNXT.MiscUtil.Collections;

namespace HSNXT.MiscUtil.Threading
{
	/// <summary>
	/// A thread pool implementation which allows policy decisions
	/// for the number of threads to run, etc, to be programatically set.
	/// </summary>
	/// <remarks>
	/// Each instance runs with entirely separate threads, so one thread pool
	/// cannot "starve" another one of threads, although having lots of threads
	/// running may mean that some threads are starved of processor time.
	/// If the values for properties such as MinThreads, IdlePeriod etc are changed
	/// after threads have been started, it may take some time before their effects
	/// are noticed. For instance, reducing the idle time from 5 minutes to 1 minute
	/// will not prevent a thread which had only just started to idle from waiting
	/// for 5 minutes before dying. Any exceptions thrown in the work item itself
	/// are handled by the WorkerException event, but all other exceptions are
	/// propagated to the AppDomain's UnhandledException event. This includes
	/// exceptions thrown by the BeforeWorkItem and AfterWorkItem events.
	/// This class is thread-safe - any thread may call any method on any instance of it.
	/// </remarks>
	public class CustomThreadPool
	{
		#region Static state
		/// <summary>
		/// Lock around all static members.
		/// </summary>
		private static object staticLock = new object();

		/// <summary>
		/// Total number of instances created
		/// </summary>
		private static int instanceCount = 0;
		#endregion

		#region Constants, defaults etc
		/// <summary>
		/// Default idle period for new pools
		/// </summary>
		private const int DefaultIdlePeriod = 60000;
		/// <summary>
		/// Default min threads for new pools
		/// </summary>
		private const int DefaultMinThreads = 5;
		/// <summary>
		/// Default max threads for new pools
		/// </summary>
		private const int DefaultMaxThreads = 10;

		/// <summary>
		/// If the idle period is short but we have less than the
		/// minimum number of threads, idle for this long instead,
		/// so as to avoid tight-looping. We don't enforce this
		/// minimum if the thread will die if it idles for its IdlePeriod.
		/// </summary>
		private const int MinWaitPeriod = 60000;

		/// <summary>
		/// If the idle period is long (or infinite) then we shouldn't
		/// actually wait that long, just in case the timeout etc changes.
		/// </summary>
		private const int MaxWaitPeriod = 300000;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new thread pool with an autogenerated name.
		/// </summary>
		public CustomThreadPool()
		{
			lock (staticLock)
			{
				instanceCount++;
				lock (stateLock)
				{
					name = "CustomThreadPool-"+instanceCount;
				}
			}
		}

		/// <summary>
		/// Creates a new thread pool with the specified name
		/// </summary>
		/// <param name="name">The name of the new thread pool</param>
		public CustomThreadPool(string name) 
		{
			lock (staticLock)
			{
				instanceCount++;
			}
			lock (stateLock)
			{
				this.name = name;
			}
		}
		#endregion

		#region Instance state
		/// <summary>
		/// Lock around access to all state other than events and the queue.
		/// The queue lock may be acquired within this lock.
		/// </summary>
		private object stateLock = new object();

		/// <summary>
		/// Lock for the queue itself. The state lock must not be acquired within
		/// this lock unless it is already held by the thread.
		/// </summary>
		private object queueLock = new object();

		/// <summary>
		/// The queue itself.
		/// </summary>
		private RandomAccessQueue<ThreadPoolWorkItem> queue = new RandomAccessQueue<ThreadPoolWorkItem>();

		/// <summary>
		/// The number of threads started (in total) by this threadpool.
		/// Used for naming threads.
		/// </summary>
		private int threadCounter = 0;

		private int idlePeriod = DefaultIdlePeriod;
		/// <summary>
		/// How long a thread may be remain idle for before dying, in ms.
		/// Note that a thread will not die if doing so would
		/// reduce the number of threads below MinThreads. A value of 0 here
		/// indicates that threads should not idle at all (except if the number
		/// of threads would otherwise fall below MinThreads). A value of
		/// Timeout.Infinite indicates that a thread will idle until a new work item
		/// is added, however long that is.
		/// </summary>
		public int IdlePeriod
		{
			get 
			{ 
				lock (stateLock)
				{
					return idlePeriod;
				}
			}
			set 
			{ 
				if (value < 0 && value != Timeout.Infinite)
				{
					throw new ArgumentException("IdlePeriod must be non-negative.", "IdlePeriod");
				}
				lock (stateLock)
				{
					idlePeriod = value; 
				}
			}
		}

		private string name;
		/// <summary>
		/// Gets the name of the thread pool.
		/// This is used to set the name of any new threads created by the pool.
		/// </summary>
		public string Name
		{
			get 
			{
				lock (stateLock)
				{
					return name;
				}
			}
		}

		private int minThreads=DefaultMinThreads;
		/// <summary>
		/// The minimum number of threads to leave in the pool. Note that
		/// the pool may contain fewer threads than this until work items
		/// have been placed on the queue. A call to StartMinThreads
		/// will make sure that at least MinThreads threads have been started.
		/// This value must be non-negative. Note that a MinThreads value of 0
		/// introduces a possible (although very unlikely) race condition where
		/// a work item may be added to the queue just as the last thread decides
		/// to exit. In this case, the work item would not be executed until the
		/// next work item was added.
		/// TODO: Try to remove this race condition
		/// </summary>
		public int MinThreads
		{
			get 
			{ 
				lock (stateLock)
				{
					return minThreads;
				}
			}
			set 
			{ 
				if (value < 0)
				{
					throw new ArgumentException ("MinThreads must be non-negative", "MinThreads");
				}
				lock (stateLock)
				{
					if (value > maxThreads)
					{
						throw new ArgumentOutOfRangeException
							("MinThreads must be less than or equal to MaxThreads");
					}
					minThreads = value; 
				}
			}
		}

		private int maxThreads=DefaultMaxThreads;
		/// <summary>
		/// The maximum number of threads to allow to be in the pool at any
		/// one time. This value must be greater than or equal to 1.
		/// </summary>
		public int MaxThreads
		{
			get 
			{ 
				lock (stateLock)
				{
					return maxThreads; 
				}
			}
			set 
			{
				if (value < 1)
				{
					throw new ArgumentException ("MaxThreads must be at least 1", "MaxThreads");
				}
				lock (stateLock)
				{
					if (value < minThreads)
					{
						throw new ArgumentOutOfRangeException
							("MaxThreads must be greater than or equal to MinThreads");
					}
					maxThreads = value; 
				}
			}
		}

		/// <summary>
		/// Sets both the minimum and maximum number of threads, atomically. This prevents
		/// exceptions which might occur when setting properties individually (e.g. going
		/// from (min=5, max=10) to (min=15, max=20), if the minimum were changed first,
		/// an exception would occur.
		/// </summary>
		public void SetMinMaxThreads(int min, int max)
		{
			lock (stateLock)
			{
				MinThreads=0;
				MaxThreads=max;
				MinThreads=min;
			}
		}

		/// <summary>
		/// The number of work items currently awaiting execution.
		/// This does not include work items currently being executed.
		/// </summary>
		public int QueueLength
		{
			get 
			{
				lock (queueLock)
				{
					return queue.Count;
				}
			}
		}

		private int workingThreads;
		/// <summary>
		/// The number of threads currently executing work items
		/// or BeforeWorkItem/AfterWorkItem/WorkerException events.
		/// </summary>
		public int WorkingThreads
		{
			get 
			{ 
				lock (stateLock)
				{
					return workingThreads;
				}
			}
		}

		private int totalThreads;
		/// <summary>
		/// The total number of threads in the pool at the present time.
		/// </summary>
		public int TotalThreads => totalThreads;

		private ThreadPriority workerThreadPriority = ThreadPriority.Normal;
		/// <summary>
		/// The priority of worker threads. Each thread's priority is set
		/// before the BeforeWorkItem event and after the AfterWorkItem event.
		/// The priority of an individual thread may be changed in 
		/// the BeforeWorkItem event, and that changed priority
		/// will remain active for the duration of the work item itself.
		/// The default is ThreadPriority.Normal.
		/// </summary>
		public ThreadPriority WorkerThreadPriority
		{
			get
			{
				lock (stateLock)
				{
					return workerThreadPriority;
				}
			}
			set
			{
				lock (stateLock)
				{
					workerThreadPriority = value;
				}
			}
		}

		private bool workerThreadsAreBackground = true;
		/// <summary>
		/// Whether or not worker threads should be created and
		/// set as background threads. This is set for the thread before
		/// the BeforeWorkItem event and after the AfterWorkItem event.
		/// The background status of a thread may be changed in the BeforeWorkItem
		/// event, and that changed status will remain active for the duration
		/// of the work item itself. Default is true.
		/// </summary>
		public bool WorkerThreadsAreBackground
		{
			get
			{
				lock (stateLock)
				{
					return workerThreadsAreBackground;
				}
			}
			set
			{
				lock (stateLock)
				{
					workerThreadsAreBackground = value;
				}
			}
		}
#endregion

		#region Events
		/// <summary>
		/// Lock around all access to events.
		/// </summary>
		private object eventLock = new object();

		private ThreadPoolExceptionHandler exceptionHandler;
		/// <summary>
		/// Event which is fired if a worker thread throws an exception in
		/// its work item.
		/// </summary>
		public event ThreadPoolExceptionHandler WorkerException
		{
			add
			{
				lock (eventLock)
				{
					exceptionHandler += value;
				}
			}
			remove
			{
				lock (eventLock)
				{
					exceptionHandler -= value;
				}
			}
		}

		/// <summary>
		/// Raises the WorkerException event.
		/// TODO: Write to the event log if no exception handlers are attached?
		/// </summary>
		private void OnException(ThreadPoolWorkItem workItem, Exception e)
		{
			ThreadPoolExceptionHandler eh;
			lock (eventLock)
			{
				eh = exceptionHandler;
			}
			if (eh != null)
			{
				Delegate[] delegates = eh.GetInvocationList();
				bool handled = false;
				foreach (ThreadPoolExceptionHandler d in delegates)
				{
					d(this, workItem, e, ref handled);
					if (handled)
					{
						return;
					}
				}
			}
		}

		/// <summary>
		/// Delegate for the BeforeWorkItem event.
		/// </summary>
		private BeforeWorkItemHandler beforeWorkItem;
		/// <summary>
		/// Event fired before a worker thread starts a work item.
		/// </summary>
		public event BeforeWorkItemHandler BeforeWorkItem
		{
			add
			{
				lock (eventLock)
				{
					beforeWorkItem += value;
				}
			}
			remove
			{
				lock (eventLock)
				{
					beforeWorkItem -= value;
				}
			}
		}

		/// <summary>
		/// Raises the BeforeWorkItem event
		/// </summary>
		/// <param name="workItem">The work item which is about to execute</param>
		/// <param name="cancel">Whether or not the work item was cancelled by an event handler</param>
		private void OnBeforeWorkItem(ThreadPoolWorkItem workItem, out bool cancel)
		{
			cancel = false;
			BeforeWorkItemHandler delegateToFire;
			lock (eventLock)
			{
				delegateToFire = beforeWorkItem;
			}
			if (delegateToFire != null)
			{
				Delegate[] delegates = delegateToFire.GetInvocationList();
				foreach (BeforeWorkItemHandler d in delegates)
				{
					d(this, workItem, ref cancel);
					if (cancel)
					{
						return;
					}
				}
			}
		}

		/// <summary>
		/// Delegate for the AfterWorkItem event.
		/// </summary>
		private AfterWorkItemHandler afterWorkItem;
		/// <summary>
		/// Event fired after a worker thread successfully finishes a work item.
		/// This event is not fired if the work item throws an exception.
		/// </summary>
		public event AfterWorkItemHandler AfterWorkItem
		{
			add
			{
				lock (eventLock)
				{
					afterWorkItem += value;
				}
			}
			remove
			{
				lock (eventLock)
				{
					afterWorkItem -= value;
				}
			}
		}

		/// <summary>
		/// Raises the AfterWorkItem event
		/// </summary>
		private void OnAfterWorkItem(ThreadPoolWorkItem workItem)
		{
			AfterWorkItemHandler delegateToFire;
			lock (eventLock)
			{
				delegateToFire = afterWorkItem;
			}
			if (delegateToFire != null)
			{
				delegateToFire(this, workItem);
			}
		}

		/// <summary>
		/// Delegate for the ThreadExit event.
		/// </summary>
		private ThreadProgress workerThreadExit;
		/// <summary>
		/// Event called just before a thread dies.
		/// </summary>
		public event ThreadProgress WorkerThreadExit
		{
			add
			{
				lock (eventLock)
				{
					workerThreadExit += value;
				}
			}
			remove
			{
				lock (eventLock)
				{
					workerThreadExit -= value;
				}
			}
		}

		/// <summary>
		/// Raises the WorkerThreadExit event and decrements the number of total worker threads
		/// </summary>
		private void OnWorkerThreadExit()
		{
			try
			{
				ThreadProgress delegateToFire;
				lock (eventLock)
				{
					delegateToFire = workerThreadExit;
				}
				if (delegateToFire != null)
				{
					delegateToFire(this);
				}
			}
			catch
			{
				// Don't do anything if this event throws an exception
			}
			lock (stateLock)
			{
				totalThreads--;
			}
		}
		#endregion

		#region Thread pool methods
		/// <summary>
		/// Ensures that the pool has at least the minimum number of threads.
		/// </summary>
		public void StartMinThreads()
		{
			lock (stateLock)
			{
				while (TotalThreads < MinThreads)
				{
					StartWorkerThread();
				}
			}
		}

		/// <summary>
		/// Adds a work item to the queue, starting a new thread if appropriate.
		/// </summary>
		/// <param name="workItemDelegate">The delegate representing the work item</param>
		/// <param name="parameters">The parameters to pass to the delegate</param>
		public void AddWorkItem(Delegate workItemDelegate, params object[] parameters)
		{
			if (workItemDelegate==null)
			{
				throw new ArgumentNullException("workItemDelegate");
			}
			AddWorkItem (new ThreadPoolWorkItem(workItemDelegate, parameters));
		}

		/// <summary>
		/// Adds a work item to the queue, starting a new thread if appropriate.
		/// </summary>
		/// <param name="workItemDelegate">The delegate representing the work item.</param>
		public void AddWorkItem(Delegate workItemDelegate)
		{
			if (workItemDelegate==null)
			{
				throw new ArgumentNullException("workItemDelegate");
			}
			AddWorkItem (new ThreadPoolWorkItem (workItemDelegate, null));
		}

		/// <summary>
		/// Adds a work item to the queue and potentially start a new thread.
		/// A thread is started if there are no idle threads or if there is already
		/// something on the queue - but in each case, only if the total number of
		/// threads is less than the maximum.
		/// </summary>
		/// <param name="workItem">The actual work item to add to the queue.</param>
		public void AddWorkItem (ThreadPoolWorkItem workItem)
		{
			if (workItem==null)
			{
				throw new ArgumentNullException("workItem");
			}
			bool startNewThread;
			lock (stateLock)
			{
				lock (queueLock)
				{
					if (queue.Count==0)
					{
						queue.Enqueue(workItem);
					}
					else
					{
						// Work out where in the queue the item should go

						// Common case: it belongs at the end
						if (((ThreadPoolWorkItem)queue[queue.Count-1]).Priority >= workItem.Priority)
						{
							queue.Enqueue(workItem);
						}
						else
						{
							// This will find the complement of the correct position, due to the
							// "interesting" nature of PriorityComparer.
							int position = queue.BinarySearch(workItem, PriorityComparer.Instance);
							queue.Enqueue(workItem, ~position);
						}
					}
					startNewThread = (WorkingThreads + queue.Count > TotalThreads) && 
						             (TotalThreads < MaxThreads);
					// Always pulse the queueLock, whether there's something waiting or not.
					// This is easier than trying to work out for sure whether or not it's
					// worth pulsing, and errs on the side of caution.
					Monitor.Pulse(queueLock);
				}
			}
			if (startNewThread)
			{
				StartWorkerThread();
			}
		}

		/// <summary>
		/// Cancels the first work item with the specified ID, if there is one.
		/// Note that items which have been taken off the queue and are running
		/// or about to be started cannot be cancelled.
		/// </summary>
		/// <param name="id">The ID of the work item to cancel</param>
		public bool CancelWorkItem(object id)
		{
			if (id==null)
			{
				throw new ArgumentNullException("id");
			}
			lock (queueLock)
			{
				for (int i=0; i < queue.Count; i++)
				{
					ThreadPoolWorkItem item = (ThreadPoolWorkItem) queue[i];
					object otherID = item.ID;
					if (otherID != null && id.Equals(otherID))
					{
						queue.RemoveAt(i);
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Cancels all work items in the queue.
		/// Note that items which have been taken off the queue and are running
		/// or about to be started cannot be cancelled.
		/// </summary>
		public void CancelAllWorkItems()
		{
			lock (queueLock)
			{
				queue.Clear();
			}
		}
		/// <summary>
		/// Starts a new worker thread.
		/// </summary>
		private void StartWorkerThread()
		{
			bool background;
			lock (stateLock)
			{
				threadCounter++;
				totalThreads++;
				background = workerThreadsAreBackground;
			}
			Thread thread = new Thread(new ThreadStart(WorkerThreadLoop));
			thread.Name = Name+" thread "+threadCounter;
			thread.IsBackground = background;
			thread.Start();
		}
		#endregion

		#region Worker thread methods
		/// <summary>
		/// Main worker thread loop. This picks jobs off the queue and executes
		/// them, until it's time to die.
		/// </summary>
		private void WorkerThreadLoop()
		{
			// Big try/finally block just to decrement the number of threads whatever happens.
			try
			{
				DateTime lastJob = DateTime.UtcNow;
				while (true)
				{
					lock (stateLock)
					{
						if (TotalThreads > MaxThreads)
						{
							return;
						}
					}
					int waitPeriod = CalculateWaitPeriod (lastJob);

					ThreadPoolWorkItem job = GetNextWorkItem (waitPeriod);

					// No job? Check whether or not we should die
					if (job==null)
					{
						if (CheckIfThreadShouldQuit(lastJob))
						{
							return;
						}
					}
					else
					{
						ExecuteWorkItem(job);
						lastJob = DateTime.UtcNow;
					}
				}
			}
			finally
			{
				OnWorkerThreadExit();
			}
		}

		/// <summary>
		/// Work out how long to wait for in this iteration. If the thread isn't going
		/// to die even if the wait completes, make the idle timeout at least MinWaitPeriod
		/// so we don't end up with lots of threads stealing CPU by checking too often.
		/// </summary>
		/// <param name="lastJob">The time this thread last finished executing a work item.</param>
		/// <returns></returns>
		private int CalculateWaitPeriod (DateTime lastJob)
		{
			lock (stateLock)
			{
				int waitPeriod = IdlePeriod;
				// Work out how long to actually wait for
				if (waitPeriod != Timeout.Infinite)
				{
					waitPeriod = (int)(DateTime.UtcNow-lastJob).TotalMilliseconds;
					// If the idle period has expired, idle for no time.
					if (waitPeriod < 0)
					{
						waitPeriod = 0;
					}
				}
				// Do we need to raise the waiting period?
				if (TotalThreads <= MinThreads && 
					waitPeriod < MinWaitPeriod && 
					waitPeriod != Timeout.Infinite)
				{
					waitPeriod = MinWaitPeriod;
				}

				// Do we need to lower the waiting period?
				if (waitPeriod > MaxWaitPeriod ||
					waitPeriod == Timeout.Infinite)
				{
					waitPeriod = MaxWaitPeriod;
				}
				return waitPeriod;
			}
		}

		/// <summary>
		/// Retrieves the next work item from the queue, pausing for at most
		/// the specified amount of time.
		/// </summary>
		/// <param name="waitPeriod">
		/// The maximum amount of time to wait for a work item to arrive, in ms.
		/// </param>
		/// <returns>
		/// The next work item, or null if there aren't any after waiting
		/// for the specified period.
		/// </returns>
		private ThreadPoolWorkItem GetNextWorkItem(int waitPeriod)
		{
			lock (queueLock)
			{
				if (queue.Count != 0)
				{
					return (ThreadPoolWorkItem) queue.Dequeue();
				}
				else
				{
					Monitor.Wait(queueLock, waitPeriod);
					if (queue.Count != 0)
					{
						return (ThreadPoolWorkItem) queue.Dequeue();
					}
					else
					{
						return null;
					}
				}
			}
		}

		/// <summary>
		/// Checks whether or not this thread should exit, based on the current number
		/// of threads and the time that this thread last finished executing a work item.
		/// </summary>
		/// <param name="lastJob">The time this thread last finished executing a work item.</param>
		/// <returns>Whether or not the thread is "spare" and should thus quit</returns>
		private bool CheckIfThreadShouldQuit (DateTime lastJob)
		{
			lock (stateLock)
			{
				if (TotalThreads > MinThreads)
				{
					TimeSpan actualIdle = DateTime.UtcNow-lastJob;
					if (IdlePeriod != Timeout.Infinite &&
						IdlePeriod < actualIdle.TotalMilliseconds)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Executes the given work item, firing the BeforeWorkItem and AfterWorkItem events,
		/// and incrementing and decrementing the number of working threads.
		/// </summary>
		/// <param name="job">The work item to execute</param>
		private void ExecuteWorkItem(ThreadPoolWorkItem job)
		{
			lock (stateLock)
			{
				workingThreads++;
				Thread.CurrentThread.Priority = workerThreadPriority;
				Thread.CurrentThread.IsBackground = workerThreadsAreBackground;
			}
			try
			{
				bool cancel;
				OnBeforeWorkItem(job, out cancel);
				if (cancel)
				{
					return;
				}
				try
				{
					job.Invoke();
				}
				catch (Exception e)
				{
					OnException (job, e);
					return;
				}
				OnAfterWorkItem(job);
			}
			finally
			{
				lock (stateLock)
				{
					Thread.CurrentThread.Priority = workerThreadPriority;
					Thread.CurrentThread.IsBackground = workerThreadsAreBackground;
					workingThreads--;
				}
			}
		}
		#endregion

		#region PriorityComparer
		/// <summary>
		/// Comparer which compares an integer priority with the priority of a work item.
		/// Must only be used in the appropriate order (CompareTo(int, WorkItem)). Also,
		/// 0 is never returned by the method - effectively, the given priority is raised by
		/// 0.5, so that when a binary search is used, the value is never found but the returned
		/// index is always the bitwise complement of the correct insertion point.
		/// </summary>
		private class PriorityComparer : IComparer<ThreadPoolWorkItem>
		{
			/// <summary>
			/// Access to single instance of PriorityComparer.
			/// </summary>
			internal static readonly IComparer<ThreadPoolWorkItem> Instance = new PriorityComparer();

			/// <summary>
			/// Private constructor to prevent instantiation
			/// </summary>
			private PriorityComparer()
			{
			}

			/// <summary>
			/// Implementation of IComparer.Compare - see class remarks for details.
			/// </summary>
			public int Compare(ThreadPoolWorkItem x, ThreadPoolWorkItem y)
			{
                if (x == null)
                {
                    throw new ArgumentException("x");
                }

				if (y==null)
				{
					throw new ArgumentException("y");
				}

				return x.Priority < y.Priority ? 1 : -1;
			}
		}
		#endregion
	}
}
