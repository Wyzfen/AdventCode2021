using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day8
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        public IEnumerable<(List<string> inputs, string[] outputs)> Parse()
        {
            IEnumerable<string[]> split = values.Select(v => v.Split('|')).ToArray();
            IEnumerable<(List<string> inputs, string[] outputs)> parsed = split.Select(s => (Split(s[0]).ToList(), Split(s[1]))).ToArray();

            return parsed;

            string[] Split(string s)
            {
                return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Sort()).ToArray();
            }
        }

        [TestMethod]
        public void Problem1()
        {
            int result = Parse().SelectMany(p => p.outputs).Where(o => o.Length < 5 || o.Length == 7).Count();

            Assert.AreEqual(result, 445);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            foreach (var (input, output) in Parse())
            {
                string[] mapping = new string[10];
                mapping[8] = input.Where(i => i.Length == 7).First(); input.Remove(mapping[8]);
                mapping[1] = input.Where(i => i.Length == 2).First(); input.Remove(mapping[1]);
                mapping[4] = input.Where(i => i.Length == 4).First(); input.Remove(mapping[4]);
                mapping[7] = input.Where(i => i.Length == 3).First(); input.Remove(mapping[7]);
                mapping[3] = input.Where(i => i.Length == 5 && i.Intersect(mapping[1]).Count() == 2).First(); input.Remove(mapping[3]);
                mapping[9] = input.Where(i => i.Length == 6 && i.Intersect(mapping[4]).Count() == 4).First(); input.Remove(mapping[9]);
                mapping[0] = input.Where(i => i.Length == 6 && i.Intersect(mapping[1]).Count() == 2).First(); input.Remove(mapping[0]);
                mapping[6] = input.Where(i => i.Length == 6).First(); input.Remove(mapping[6]);
                mapping[5] = input.Where(i => i.Intersect(mapping[9]).Count() == 5).First(); input.Remove(mapping[5]);
                mapping[2] = input.First();

                int value = 0;
                for(int i = 0; i < 4; i++)
                {
                    value = value * 10 + Array.IndexOf(mapping, output[i]);
                }

                result += value;
            }

            Assert.AreEqual(result, 1043101);
        }

    }

    public static class StringExtension
    {
        public static string Sort(this string input)
        {
            char[] characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }
    }
}
