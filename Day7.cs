using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day7
    {
        readonly int[] test = new int[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<int> values = Utils.FromCSVFile<int>($"{day}.txt")[0];

        [TestMethod]
        public void Problem1()
        {
            int min = values.Min();
            int max = values.Max();

            int result = int.MaxValue;

            for(int i = min; i <= max; i++)
            {
                var val = values.Sum(v => Math.Abs(v - i));
                if (val < result) result = val;
            }

            Assert.AreEqual(result, 328187);
        }

        [TestMethod]
        public void Problem2()
        {
            int min = values.Min();
            int max = values.Max();

            int result = int.MaxValue;

            for (int i = min; i <= max; i++)
            {
                var val = values.Sum(v => (Math.Abs(v - i) * (Math.Abs(v - i) + 1)) / 2);
                if (val < result) result = val;
            }

            Assert.AreEqual(result, 91257582);
        }
    }
}
