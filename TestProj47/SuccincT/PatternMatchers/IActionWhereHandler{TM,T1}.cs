using System;

namespace HSNXT.SuccincT.PatternMatchers
{
    public interface IActionWhereHandler<out TMatcher, out T1>
    {
        TMatcher Do(Action<T1> action);
    }
}