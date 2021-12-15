using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day15
    {
        readonly static string[] test = new string[]
        {
            "1163751742",
            "1381373672",
            "2136511328",
            "3694931569",
            "7463417111",
            "1319128137",
            "1359912421",
            "3125421639",
            "1293138521",
            "2311944581"
        };
        
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly static IEnumerable<string> input = Utils.FromFile<string>($"{day}.txt");
        readonly int[] values = input.Where(s => !string.IsNullOrWhiteSpace(s)).SelectMany(s => s.Select(c => (int)(c - '0')).ToArray()).ToArray();
        readonly static int size = input.First().Length;

        int Index(int x, int y, int scale) => y * size * scale + x;
        (int x, int y) Coordinates(int index, int scale) => (index % (size * scale), index / (size * scale));

        IEnumerable<(int index, int x, int y)> Neighbours(int x, int y, int scale = 1, bool diagonals = false)
        {
            if (x > 0)
            {
                if (diagonals && y > 0) yield return new (Index(x - 1, y - 1, scale), x - 1, y - 1);
                yield return new(Index(x - 1, y, scale), x - 1, y);
                if (diagonals && y < (size * scale - 1)) yield return new (Index(x - 1, y + 1, scale), x - 1, y + 1);
            }

            if (y > 0) yield return new(Index(x, y - 1, scale), x, y - 1);
            if (y < (size * scale - 1)) yield return new(Index(x, y + 1, scale), x, y + 1);

            if (x < (size * scale - 1))
            {
                if (diagonals && y > 0) yield return new(Index(x + 1, y - 1, scale), x + 1, y - 1);
                yield return new(Index(x + 1, y, scale), x + 1, y);
                if (diagonals && y < (size * scale - 1)) yield return new(Index(x + 1, y + 1, scale), x + 1, y + 1);
            }
        }

        int [] Navigate(int scale)
        {
            int[] costs = new int[values.Length * scale * scale];
            Queue<int> toVisit = new();
            toVisit.Enqueue(0);

            while (toVisit.Count > 0)
            {
                int nextIndex = toVisit.Dequeue();

                var (x, y) = Coordinates(nextIndex, scale);
                var cost = costs[nextIndex];

                foreach (var (neighbour, nx, ny) in Neighbours(x, y, scale))
                {
                    int subIndex = Index(nx % size, ny % size, 1);
                    int newCost = cost + (((nx / size) + (ny / size) + values[subIndex] - 1) % 9) + 1;
                    if (costs[neighbour] == 0 || newCost < costs[neighbour])
                    {
                        costs[neighbour] = newCost;
                        toVisit.Enqueue(neighbour);
                    }
                }
            }

            return costs;
        }

        [TestMethod]
        public void Problem1()
        {
            int[] costs = Navigate(1);
            int endIndex = Index(size - 1, size - 1, 1);
            int result = costs[endIndex];

            Assert.AreEqual(result, 602);
        }

        [TestMethod]
        public void Problem2()
        {
            int[] costs = Navigate(5);
            int endIndex = Index(size * 5 - 1, size * 5 - 1, 5);
            int result = costs[endIndex];

            Assert.AreEqual(result, 2935);
        }
    }
}
