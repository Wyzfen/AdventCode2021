using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day11
    {
        readonly static string[] test = new string[]
        {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526"
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly static IEnumerable<string> input = Utils.FromFile<string>($"{day}.txt");
        readonly int[] values = input.SelectMany(s => s.Select(c => (int)(c - '0')).ToArray()).ToArray();

        int Index(int x, int y) => y * 10 + x;
        (int x, int y) Coordinates(int index) => (index % 10, index / 10);

        string[] To2D() => Enumerable.Range(0, 10).Select(i => String.Join(" ", values.Skip(i * 10).Take(10).Select(v => $"{v,2:#0}").ToArray())).ToArray();

        IEnumerable<int> Neighbours(int x, int y)
        {
            if(x > 0)
            {
                if (y > 0) yield return Index(x - 1, y - 1);
                yield return Index(x - 1, y);
                if (y < 9) yield return Index(x - 1, y + 1);
            }

            if (y > 0) yield return Index(x, y - 1);
            if (y < 9) yield return Index(x, y + 1);

            if (x < 9)
            {
                if (y > 0) yield return Index(x + 1, y - 1);
                yield return Index(x + 1, y);
                if (y < 9) yield return Index(x + 1, y + 1);
            }
        }

        IEnumerable<int> Flash(int index)
        {
            var (x, y) = Coordinates(index);
            foreach(var neighbour in Neighbours(x, y))
            {
                values[neighbour]++;
                if (values[neighbour] == 10) yield return neighbour;
            }
        }

        [TestMethod]
        public void Problem1()
        {   
            int result = 0;

            for(int i = 0; i < 100; i++)
            {
                // Increase
                for (int j = 0; j < values.Length; j++)
                {
                    values[j]++;
                }

                // Flash
                var toFlash = values.Select((value, index) => (value, index)).Where(v => v.value == 10).Select(v => v.index).ToArray();
                while (toFlash.Length > 0)
                {
                    toFlash = toFlash.SelectMany(index => Flash(index)).Distinct().ToArray();
                } 

                result += values.Count(v => v > 9);

                // Reset
                for (int j = 0; j < values.Length; j++)
                {
                    if (values[j] > 9) values[j] = 0;
                }
            }

            Assert.AreEqual(result, 1691);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 1;
            for (;; result++)
            {
                // Increase
                for (int j = 0; j < values.Length; j++)
                {
                    values[j]++;
                }

                // Flash
                var toFlash = values.Select((value, index) => (value, index)).Where(v => v.value == 10).Select(v => v.index).ToArray();
                while (toFlash.Length > 0)
                {
                    toFlash = toFlash.SelectMany(index => Flash(index)).Distinct().ToArray();
                }

                // Reset
                for (int j = 0; j < values.Length; j++)
                {
                    if (values[j] > 9) values[j] = 0;
                }

                if (values.All(v => v == 0)) break;
            }

            Assert.AreEqual(result, 1656);
        }
    }
}
