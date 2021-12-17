using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day17
    {
        public record Target(int minX, int maxX, int minY, int maxY);

        // target area: x=192..251, y=-89..-59
        static readonly Target input = new Target(192, 251, -89, -59);
        // target area: x=20..30, y=-10..-5
        static readonly Target test = new Target(20, 30, -10, -5);

        static readonly Target values = input;

        IEnumerable<(int x, int y)> Trajectory(int xv, int yv)
        {
            int x = 0, y = 0;
            while(x <= values.maxX && y >= values.minY)
            {
                x += xv;
                y += yv;
                
                if (xv < 0) xv++;
                else if (xv > 0) xv--;

                yv--;

                if (x <= values.maxX && y >= values.minY)
                {
                    yield return (x, y);
                }
            }
        }

        bool InTarget(int x, int y) => x >= values.minX && x <= values.maxX && y >= values.minY && y <= values.maxY;

        [TestMethod]
        public void Problem1()
        {           
            int result = 0;

            for(int x = 1; x <= 100; x++)
            {
                for(int y = 0; y <= 100; y++)
                {
                    var trajectory = Trajectory(x, y).ToArray();
                    if(trajectory.Any(t => InTarget(t.x, t.y)))
                    {
                        var max = trajectory.Max(t => t.y);
                        result = Math.Max(result, max);
                    }
                }
            }

            Assert.AreEqual(result, 3916);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            for (int x = 1; x <= values.maxX; x++)
            {
                for (int y = values.minY; y <= 100; y++)
                {
                    var trajectory = Trajectory(x, y).ToArray();
                    if (trajectory.Any(t => InTarget(t.x, t.y)))
                    {
                        result++;
                    }
                }
            }

            Assert.AreEqual(result, 2986);
        }
    }
}
