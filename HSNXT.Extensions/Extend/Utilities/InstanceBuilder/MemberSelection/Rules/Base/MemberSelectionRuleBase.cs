﻿using System;
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


namespace HSNXT
{
    /// <summary>
    ///     Base class for member selection rules.
    /// </summary>
    public abstract class MemberSelectionRuleBase : IMemberSelectionRule
    {
        #region Ctor

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemberSelectionRuleBase" /> class.
        /// </summary>
        /// <param name="name">The name of the rule.</param>
        /// <param name="description">The description of the rule.</param>
        protected MemberSelectionRuleBase( string name, string description )
        {
            RuleName = name;
            RuleDescription = description;
        }

        #endregion

        #region Implementation of IMemberSelectionRule

        /// <summary>
        ///     Gets the selection result for the given member.
        /// </summary>
        /// <param name="member">The member to get the selection result for.</param>
        /// <returns>Returns the selection result for the given member.</returns>
        public abstract MemberSelectionResult GetSelectionResult( IMemberInformation member );

        /// <summary>
        ///     Gets the name of the rule.
        /// </summary>
        /// <value>The name of the rule.</value>
        public string RuleName { get; }

        /// <summary>
        ///     Gets the description of the rule.
        /// </summary>
        /// <value>The description of the rule.</value>
        public string RuleDescription { get; }

        #endregion
    }
}