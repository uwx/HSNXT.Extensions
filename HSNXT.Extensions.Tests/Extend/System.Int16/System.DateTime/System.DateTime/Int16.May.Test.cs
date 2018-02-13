﻿#region Usings

using HSNXT;
using System;
using Xunit;

#endregion

namespace Extend.Testing
{
    public partial class Int16ExTest
    {
        [Fact]
        public void MayTest()
        {
            var expected = new DateTime(2000, 5, 10);
            var actual = Extensions.May(10, 2000);
            Assert.Equal(expected, actual);
        }
    }
}