using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day14
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt").Where(s => !string.IsNullOrWhiteSpace(s));


        [TestMethod]
        public void Problem1()
        {
            string code = values.First();
            var pairs = Polymerise(code, 10, GetRules());
            var counts = Counts(pairs);
            counts[code.Last()]++;

            var ordered = counts.Values.Where(v => v > 0).OrderByDescending(v => v);

            long result = ordered.First() - ordered.Last();
            Assert.AreEqual(result, 2899);
        }

        [TestMethod]
        public void Problem2()
        {
            string code = values.First();
            var pairs = Polymerise(code, 40, GetRules());
            var counts = Counts(pairs);
            counts[code.Last()]++;

            var ordered = counts.Values.Where(v => v > 0).OrderByDescending(v => v);

            long result = ordered.First() - ordered.Last();
            Assert.AreEqual(result, 3528317079545);
        }

        public static Dictionary<string, string> SplitFromStrings(IEnumerable<string> strings, string split = ")") =>
            strings.Select(i => i.Split(split)).ToDictionary(s => s[0], s => s[1]);


        private Dictionary<string, string> GetRules() => SplitFromStrings(values.Skip(1), " -> ");

        private Dictionary<string, long> Polymerise(string code, int count, Dictionary<string, string> rules)
        {
            var pairs = code.Zip(code.Skip(1), (a, b) => new string(new char [] { a, b })).GroupBy(p => p).ToDictionary(g => g.Key, g => (long)g.Count());
            foreach(var missing in rules.Keys.Except(pairs.Keys).ToArray())
            {
                pairs.Add(missing, 0);
            }

            for(int i = 0; i < count; i++)
            {
                foreach(var (pair, amount) in pairs.ToArray())
                {
                    char first = pair[0];
                    char second = pair[1];
                    char next = rules[pair][0];
                    pairs[pair] -= amount;
                    pairs[$"{first}{next}"] += amount;
                    pairs[$"{next}{second}"] += amount;
                }
            }


            return pairs;
        }

        private Dictionary<char, long> Counts(Dictionary<string, long> pairs)
        {
            var output = Enumerable.Range(0, 26).ToDictionary(v => (char)(v + 'A'), v => 0L);
            foreach(var (key, value) in pairs)
            {
                output[key[0]] += value;
            }

            return output;
        }
    }
}
