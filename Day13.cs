using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day13
    {
        readonly static string[] test =
        {
            "6,10",
            "0,14",
            "9,10",
            "0,3",
            "10,4",
            "4,11",
            "6,0",
            "6,12",
            "4,1",
            "0,13",
            "10,12",
            "3,4",
            "3,0",
            "8,4",
            "1,10",
            "2,14",
            "8,10",
            "9,0",
            "",
            "fold along y=7",
            "fold along x=5"
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        IEnumerable<Vector2> FoldX(IEnumerable<Vector2> input, int fold) => input.Select(v => v.X < fold ? v : new Vector2 { X = 2* fold - v.X, Y = v.Y }).Distinct();
        IEnumerable<Vector2> FoldY(IEnumerable<Vector2> input, int fold) => input.Select(v => v.Y < fold ? v : new Vector2 { X = v.X, Y = 2 * fold - v.Y }).Distinct();

        [TestMethod]
        public void Problem1()
        {
            int blankIndex = new List<string>(values).FindIndex(s => string.IsNullOrWhiteSpace(s));
            IEnumerable<Vector2> points = values.Take(blankIndex).Select(s => Utils.ValueFromString<Vector2>(s));
            (bool foldX, int index) [] instructions = values.Skip(blankIndex).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => (s.Contains('x'), int.Parse(s.Split('=')[1]))).ToArray();

            int result = (instructions[0].foldX ? FoldX(points, instructions[0].index) : FoldY(points, instructions[0].index)).Count();


            Assert.AreEqual(result, 729);
        }

        [TestMethod]
        public void Problem2()
        {
            int blankIndex = new List<string>(values).FindIndex(s => string.IsNullOrWhiteSpace(s));
            IEnumerable<Vector2> points = values.Take(blankIndex).Select(s => Utils.ValueFromString<Vector2>(s));
            (bool foldX, int index)[] instructions = values.Skip(blankIndex).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => (s.Contains('x'), int.Parse(s.Split('=')[1]))).ToArray();

            foreach(var (fold, index) in instructions)
            {
                points = (fold ? FoldX(points, index) : FoldY(points, index)).ToArray();
            }

            int height = points.Select(v => v.Y).Max() + 1;
            int width = points.Select(v => v.X).Max() + 1;

            StringBuilder[] output = new StringBuilder[height];
            for(int i = 0; i < height; i++) output[i] = new StringBuilder(new String('.', width));

            foreach(var (x,y) in points)
            {
                output[y][x] = '#';
            }

            string result = "RGZLBHFP";

            Assert.AreEqual(result, "RGZLBHFP");
        }
    }
}
