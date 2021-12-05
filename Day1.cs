using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day1
    {
        readonly IEnumerable<int> values = Utils.IntsFromFile("day1.txt");

        [TestMethod]
        public void Problem1()
        {
            int result = values.Skip(1).Zip(values, (second, first) => second > first).Where(a => a).Count();

            Assert.AreEqual(result, 1559);
        }

        [TestMethod]
        public void Problem2()
        {
            var window = values.Skip(2).Zip(values.Skip(1).Zip(values, (second, first) => second + first), (third, sum) => third + sum);
            int result = window.Skip(1).Zip(window, (second, first) => second > first).Where(a => a).Count();

            Assert.AreEqual(result, 1600);
        }
    }
}
