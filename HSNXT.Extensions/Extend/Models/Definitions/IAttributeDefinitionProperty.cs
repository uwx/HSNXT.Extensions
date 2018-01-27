﻿#region Usings
using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using HSNXT.Internal;

//using System;
//using System.Collections.Generic;
//using System.Reflection;

#endregion

namespace HSNXT
{
    /// <summary>
    ///     Interface representing the attribute definitions of a property.
    /// </summary>
    /// <typeparam name="T">The type of the attributes.</typeparam>
    public interface IAttributeDefinitionProperty<T> where T : Attribute
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the property which is decorated with the attributes.
        /// </summary>
        /// <value>The property which is decorated with the attributes.</value>
        PropertyInfo Property { get; set; }

        /// <summary>
        ///     Gets or sets a collection of attributes of the specified type.
        /// </summary>
        /// <value>A collection of attributes of the specified type.</value>
        IEnumerable<T> Attributes { get; set; }

        #endregion
    }
}