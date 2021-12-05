using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day3
    {

        readonly string[] test = new string[]
        {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010",
        };

        readonly static IEnumerable<string> values = Utils.StringsFromFile("day3.txt");

        [TestMethod]
        public void Problem1()
        {
            int gamma = 0, epsilon = 0;

            int length = values.First().Length;

            for (int i = 0; i < length; i++)
            {
                gamma <<= 1;

                gamma += MostCommonInPosition(values, i) == '1' ? 1 : 0;
            }

            epsilon = (1 << length) - 1 - gamma;
            int result = gamma * epsilon;

            Assert.AreEqual(result, 3969000);
        }

        [TestMethod]
        public void Problem2()
        {
            int o2 = 0, co2 = 0;
            int length = values.First().Trim().Length;

            var remaining = values.Where(v => v.Length == length);
            for (int i = 0; i < length; i++)
            {
                var digit = MostCommonInPosition(remaining, i);
                var mostCommon = digit == '0' ? '0' : '1';
                remaining = remaining.Where(v => v[i] == mostCommon).ToArray();
                if (remaining.Count() == 1) break;
            }
            o2 = Convert.ToInt32(remaining.FirstOrDefault(), 2);


            remaining = values.Where(v => v.Length == length);
            for (int i = 0; i < length; i++)
            {
                var digit = MostCommonInPosition(remaining, i);
                var leastCommon = digit == '0' ? '1' : '0';
                remaining = remaining.Where(v => v[i] == leastCommon).ToArray();
                if (remaining.Count() == 1) break;
            }
            co2 = Convert.ToInt32(remaining.FirstOrDefault(), 2);

            int result = co2 * o2;

            Assert.AreEqual(result, 4267809);
        }

        private char MostCommonInPosition(IEnumerable<string> values, int position)
        {
            int sum = values.Sum(v => v[position] == '1' ? 1 : 0);
            int rem = values.Count() - sum;

            if (sum > rem) return '1';
            if (sum == rem) return '*';

            return '0';
        }
    }
}
