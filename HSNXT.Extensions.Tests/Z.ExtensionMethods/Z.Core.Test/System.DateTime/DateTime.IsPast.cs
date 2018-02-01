// Description: C# Extension Methods Library to enhances the .NET Framework by adding hundreds of new methods. It drastically increases developers productivity and code readability. Support C# and VB.NET
// Website & Documentation: https://github.com/zzzprojects/Z.ExtensionMethods
// Forum: https://github.com/zzzprojects/Z.ExtensionMethods/issues
// License: https://github.com/zzzprojects/Z.ExtensionMethods/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HSNXT.Z.Core.Test
{
    [TestClass]
    public class System_DateTime_IsPast
    {
        [TestMethod]
        public void IsPast()
        {
            // Type
            var @this = DateTime.Now.AddDays(1);

            // Examples
            var value1 = @this.IsPast(); // return false;
            var value2 = @this.AddYears(-1).IsPast(); // return true;

            // Unit Test
            Assert.IsFalse(value1);
            Assert.IsTrue(value2);
        }
    }
}