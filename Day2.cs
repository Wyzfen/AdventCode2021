using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day2
    {
        enum Direction
        {
            Forward,
            Up,
            Down
        }

        static string [] test = new string[] 
        { 
            "forward 5",
            "down 5",
            "forward 8",
            "up 3",
            "down 8",
            "forward 2"
        };

        readonly IEnumerable<(Direction direction, int distance)> directions = Utils.FromFile<Direction, int>("day2.txt");

        [TestMethod]
        public void Problem1()
        {
            int x = 0, y = 0;

            foreach (var (direction, distance) in directions)
            {
                switch (direction)
                {
                    case Direction.Forward:
                        x += distance;
                        break;
                    case Direction.Up:
                        y -= distance;
                        if (y < 0) y = 0;
                        break;
                    case Direction.Down:
                        y += distance;
                        break;
                }
            }

            int result = x * y;

            Assert.AreEqual(result, 1690020);
        }

        [TestMethod]
        public void Problem2()
        {
            int x = 0, y = 0, aim = 0;

            foreach (var (direction, distance) in directions)
            {
                switch (direction)
                {
                    case Direction.Forward:
                        x += distance;
                        y += aim * distance;
                        if (y < 0) y = 0;
                        break;
                    case Direction.Up:
                        aim -= distance;
                        break;
                    case Direction.Down:
                        aim += distance;
                        break;
                }
            }

            int result = x * y;

            Assert.AreEqual(result, 1408487760);
        }
    }
}
