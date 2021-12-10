using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day10
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        enum Brackets
        {
            RoundStart = '(',
            RoundEnd = ')',
            SquareStart = '[',
            SquareEnd = ']',
            CurlyStart = '{',
            CurlyEnd = '}',
            TriangleStart = '<',
            TriangleEnd = '>'
        }

        Dictionary<Brackets, int> points = new Dictionary<Brackets, int> { { Brackets.RoundEnd, 3 }, { Brackets.SquareEnd, 57 }, { Brackets.CurlyEnd, 1197 }, { Brackets.TriangleEnd, 25137 } };
        Dictionary<Brackets, int> completion = new Dictionary<Brackets, int> { { Brackets.RoundStart, 1 }, { Brackets.SquareStart, 2 }, { Brackets.CurlyStart, 3 }, { Brackets.TriangleStart, 4 } };

        private bool CheckMatch(Brackets c, Stack<Brackets> stack)
        {
            switch (c)
            {
                case Brackets.RoundStart:
                case Brackets.SquareStart:
                case Brackets.CurlyStart:
                case Brackets.TriangleStart:
                    stack.Push(c);
                    return true;
                default:
                    if (stack.Count == 0) return false;
                    var oldc = stack.Pop();

                    return (c == Brackets.CurlyEnd && oldc == Brackets.CurlyStart) ||
                           (c == Brackets.RoundEnd && oldc == Brackets.RoundStart) ||
                           (c == Brackets.SquareEnd && oldc == Brackets.SquareStart) ||
                           (c == Brackets.TriangleEnd && oldc == Brackets.TriangleStart);
            }
        }

        private int CheckLine(string line, Stack<Brackets> stack)
        {
            foreach (Brackets c in line)
            {
                if (!CheckMatch(c, stack))
                {
                    return points[c];
                }
            }

            return 0;
        }

        [TestMethod]
        public void Problem1()
        {           
            int result = values.Select(v => CheckLine(v, new ())).Sum();

            Assert.AreEqual(result, 323691);
        }


        [TestMethod]
        public void Problem2()
        {
            List<long> results = new();

            foreach (var line in values)
            {
                var stack = new Stack<Brackets>();
                if (CheckLine(line, stack) > 0)
                {                    
                    continue;
                }

                results.Add(stack.Aggregate((long)0, (a, v) => a * 5 + completion[v]));
            }

            results.Sort();
            long result = results[results.Count / 2];

            Assert.AreEqual(result, 2858785164);
        }
    }
}
