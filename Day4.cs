using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2019
{
    [TestClass]
    public class Day4
    {
        static string[] test = new string[]
        {
            "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
            "",
            "22 13 17 11  0",
            " 8  2 23  4 24",
            "21  9 14 16  7",
            " 6 10  3 18  5",
            " 1 12 20 15 19",
            "              ",
            " 3 15  0  2 22",
            " 9 18 13 17  5",
            "19  8  7 25 23",
            "20 11 10 24  4",
            "14 21 16 12  6",
            "              ",
            "14 21 17 24  4",
            "10 16 15  9 19",
            "18  8 23 26 20",
            "22 11 13  6  5",
            " 2  0 12  3  7",
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        public class Bingo
        {
            private int[] cells = new int[25];
            private bool[] set = new bool[100];

            public Bingo(int [] cells)
            {
                this.cells = cells;
            }

            public bool Set(int value) => set[value] = true;

            public bool HasBingo()
            {
                for(int i = 0; i < 5; i++)
                {
                    bool hasRow = cells.Skip(i * 5).Take(5).All(v => set[v]);
                    if (hasRow) return true;

                    bool hasColumn = cells.Where((v, j) => j % 5 == i).All(v => set[v]);
                    if (hasColumn) return true;
                }

                return false;
            }

            public int UnsetSum() => cells.Where(v => !set[v]).Sum();
        }

        public (IEnumerable<int> input, IEnumerable<Bingo> boards) ParseInput(string [] values)
        {
            var input = Utils.FromString<int>(values[0], split:',');
            int count = (values.Length - 1) / 6;
            var boards = new List<Bingo>();

            for(int i = 0; i < count; i++)
            {
                var lines = values.Skip(i * 6 + 2).Take(5).SelectMany(l => Utils.FromString<int>(l)).ToArray();
                boards.Add(new Bingo(lines));
            }

            return (input, boards);
        }

        [TestMethod]
        public void Problem1()
        {
            var (input, boards) = ParseInput(values.ToArray());

            int calculate()
            {
                foreach (var value in input)
                {
                    foreach (var board in boards)
                    {
                        board.Set(value);

                        if (board.HasBingo())
                        {
                            return board.UnsetSum() * value;
                        }
                    }
                }

                return 0;
            }

            int result = calculate();

            Assert.AreEqual(result, 58374);
        }

        [TestMethod]
        public void Problem2()
        {
            var (input, allboards) = ParseInput(values.ToArray());

            int calculate()
            {
                var boards = new List<Bingo>(allboards);

                foreach (var value in input)
                {
                    for(int i = boards.Count() - 1; i >= 0; i--)
                    {
                        var board = boards[i];
                        board.Set(value);

                        if (board.HasBingo())
                        {
                            boards.Remove(board);
                            if (boards.Count() == 0)
                            {
                                return board.UnsetSum() * value;
                            }                            
                        }                        
                    }
                }

                return 0;
            }

            int result = calculate();

            Assert.AreEqual(result, 11377);
        }
    }
}
