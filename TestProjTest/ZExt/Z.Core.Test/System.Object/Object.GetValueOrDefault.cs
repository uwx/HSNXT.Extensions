// Description: C# Extension Methods Library to enhances the .NET Framework by adding hundreds of new methods. It drastically increases developers productivity and code readability. Support C# and VB.NET
// Website & Documentation: https://github.com/zzzprojects/Z.ExtensionMethods
// Forum: https://github.com/zzzprojects/Z.ExtensionMethods/issues
// License: https://github.com/zzzprojects/Z.ExtensionMethods/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HSNXT.Z.Core.Test
{
    [TestClass]
    public class System_Object_GetValueOrDefault
    {
        [TestMethod]
        public void GetValueOrDefault()
        {
            // Type
            var @this = new XmlDocument();

            // Exemples
            var result1 = @this.GetValueOrDefault(x => x.FirstChild.InnerXml, "FizzBuzz"); // return "FizzBuzz";
            var result2 = @this.GetValueOrDefault(x => x.FirstChild.InnerXml, () => "FizzBuzz"); // return "FizzBuzz"

            // Unit Test
            Assert.AreEqual("FizzBuzz", result1);
            Assert.AreEqual("FizzBuzz", result2);
        }
    }
}