using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day9
    {
        readonly static string[] test = new string[]
        {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678"
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly static IEnumerable<string> input = Utils.FromFile<string>($"{day}.txt");
        readonly int [][] values = input.Select(s => s.Select(c => (int)(c - '0')).ToArray()).ToArray();

        [TestMethod]
        public void Problem1()
        {
            int result = GetBasins().Sum(b => values[b.y][b.x] + 1);

            Assert.AreEqual(result, 554);
        }

        [TestMethod]
        public void Problem2()
        {
            int height = values.Length;
            int width = values[0].Length;

            int RecurseBasin(int x, int y, int previous, HashSet<(int x, int y)> set)
            {
                if (set.Contains((x, y))) return 0;
                set.Add((x, y));

                int value = values[y][x];
                if (value == 9) return 0;

                int sum = 1;
                if (previous < value)
                {
                    if (y > 0 && values[y - 1][x] > value) sum += RecurseBasin(x, y - 1, value, set);
                    if (x > 0 && values[y][x - 1] > value) sum += RecurseBasin(x - 1, y, value, set);
                    if (x < width - 1 && values[y][x + 1] > value) sum += RecurseBasin(x + 1, y, value, set);
                    if (y < height - 1 && values[y + 1][x] > value) sum += RecurseBasin(x, y + 1, value, set);
                }

                return sum;
            }

            var basins = GetBasins().ToArray();
            var sizes = new List<int>();
            foreach(var (x, y) in basins)
            {
                var set = new HashSet<(int x, int y)>();
                int count = RecurseBasin(x, y, -1, set);
                sizes.Add(count);
            }

            sizes.Sort();
            sizes.Reverse();

            int result = sizes[0] * sizes[1] * sizes[2];

            Assert.AreEqual(result, 1017792);
        }

        public IEnumerable<(int x, int y)> GetBasins()
        {
            int height = values.Length;
            for (int y = 0; y < height; y++)
            {
                int width = values[y].Length;
                for (int x = 0; x < width; x++)
                {
                    int value = values[y][x];
                    if ((y == 0 || values[y - 1][x] > value) &&
                       (x == 0 || values[y][x - 1] > value) &&
                       (x == width - 1 || values[y][x + 1] > value) &&
                       (y == height - 1 || values[y + 1][x] > value))
                    {
                        yield return (x, y);
                    }
                }
            }
        }
    }
}
