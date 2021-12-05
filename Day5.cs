using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day5
    {
        readonly static string[] test = new string[]
        {
            "0,9 -> 5,9",
            "8,0 -> 0,8",
            "9,4 -> 3,4",
            "2,2 -> 2,1",
            "7,0 -> 7,4",
            "6,4 -> 2,0",
            "0,9 -> 2,9",
            "3,4 -> 1,4",
            "0,0 -> 8,8",
            "5,5 -> 8,2"
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly static IEnumerable<string> input = Utils.FromFile<string>($"{day}.txt");
        readonly IEnumerable<(Vector2 start, Vector2 end)> values = input.Select(v => Utils.FromString<Vector2, Vector2>(v, "->"));
        
        [TestMethod]
        public void Problem1()
        {           
            int result = CalculateLines(false);

            Assert.AreEqual(result, 7380);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = CalculateLines(true);

            Assert.AreEqual(result, 21373);
        }


        int CalculateLines(bool includeDiagonals)
        {
            Dictionary<Vector2, int> counts = new Dictionary<Vector2, int>();

            void Add(Vector2 vector)
            {
                if (!counts.TryGetValue(vector, out int count))
                {
                    counts.Add(vector, 0);
                }

                counts[vector] = count + 1;
            }

            foreach (var (start, end) in values)
            {
                if (start.X == end.X) // Vertical
                {
                    int minY = Math.Min(start.Y, end.Y);
                    int maxY = Math.Max(start.Y, end.Y);
                    for (int y = minY; y <= maxY; y++)
                    {
                        Add(new Vector2
                        {
                            X = start.X,
                            Y = y
                        });
                    }
                }
                else if (start.Y == end.Y) // Vertical
                {
                    int minX = Math.Min(start.X, end.X);
                    int maxX = Math.Max(start.X, end.X);
                    for (int x = minX; x <= maxX; x++)
                    {
                        Add(new Vector2 { X = x, Y = start.Y });
                    }
                }
                else if(includeDiagonals)
                {
                    int minX = start.X;
                    int maxX = end.X;
                    int startY = start.Y;
                    int yDelta = start.Y > end.Y ? -1 : 1;

                    if (minX > maxX)
                    {
                        minX = end.X;
                        maxX = start.X;
                        startY = end.Y;
                        yDelta = start.Y < end.Y ? -1 : 1;
                    }

                    for (int x = minX; x <= maxX; x++)
                    {
                        Add(new Vector2 { X = x, Y = startY + yDelta * (x - minX) });
                    }
                }
            }

            return counts.Values.Where(v => v > 1).Count();
        }
    }
}
