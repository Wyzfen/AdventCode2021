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
        
        public readonly record struct Vector2(int X, int Y);

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly static IEnumerable<string> input = Utils.FromFile<string>($"{day}.txt");
        readonly int [][] values = input.Select(s => s.Select(c => (int)(c - '0')).ToArray()).ToArray();

        [TestMethod]
        public void Problem1()
        {
            int result = GetBasins().Sum(b => values[b.Y][b.X] + 1);

            Assert.AreEqual(result, 554);
        }

        [TestMethod]
        public void Problem2()
        {
            int height = values.Length;
            int width = values[0].Length;

            int RecurseBasin(Vector2 v, int previous, HashSet<Vector2> set)
            {
                if (set.Contains(v)) return set.Count;

                int value = values[v.Y][v.X];
                if (value == 9) return set.Count;

                set.Add(v);

                if (previous < value)
                {
                    if (v.Y > 0 && values[v.Y - 1][v.X] > value) RecurseBasin(new Vector2(v.X, v.Y - 1), value, set);
                    if (v.X > 0 && values[v.Y][v.X - 1] > value) RecurseBasin(new Vector2(v.X - 1, v.Y), value, set);
                    if (v.X < width - 1 && values[v.Y][v.X + 1] > value) RecurseBasin(new Vector2(v.X + 1, v.Y), value, set);
                    if (v.Y < height - 1 && values[v.Y + 1][v.X] > value) RecurseBasin(new Vector2(v.X, v.Y + 1), value, set);
                }

                return set.Count;
            }

            int result = GetBasins().Select(v => RecurseBasin(v, -1, new HashSet<Vector2>()))
                                    .OrderByDescending(v => v)
                                    .Take(3).Aggregate((a, b) => a * b);

            Assert.AreEqual(result, 1017792);
        }

        public IEnumerable<Vector2> GetBasins()
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
                        yield return new Vector2(x, y);
                    }
                }
            }
        }
    }
}
