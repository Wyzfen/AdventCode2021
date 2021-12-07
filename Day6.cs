using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day6
    {
        readonly static int[] test = new int[]
        {
            3,4,3,1,2
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<int> values = Utils.FromCSVFile<int>($"{day}.txt")[0];

        [TestMethod]
        public void Problem1()
        {           
            var dict = values.GroupBy(v => v).ToDictionary(k => k.Key, v => v.Count());
            var input = Enumerable.Range(0, 9).Select(v => dict.TryGetValue(v, out int r) ? r : 0).ToArray();
            for(int index = 1; index < 80; index++)
            {
                input[(index + 7) % 9] += input[index % 9]; // existing critters age 6
            }

            int result = input.Sum();

            Assert.AreEqual(result, 360761);
        }

        [TestMethod]
        public void Problem2()
        {
            var dict = values.GroupBy(v => v).ToDictionary(k => k.Key, v => (long) v.Count());
            var input = Enumerable.Range(0, 9).Select(v => dict.TryGetValue(v, out long r) ? r : 0).ToArray();
            for (int index = 1; index < 256; index++)
            {
                input[(index + 7) % 9] += input[index % 9]; // existing critters age 6
            }

            long result = input.Sum();

            Assert.AreEqual(result, 1632779838045);
        }
    }
}
