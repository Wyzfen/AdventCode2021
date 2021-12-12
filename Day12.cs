using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day12
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<(string, string)> values = Utils.FromFile<string, string>($"{day}.txt", "-");

        Dictionary<string, List<string>> GetNodes(IEnumerable<(string, string)> values)
        {
            Dictionary<string, List<string>> nodes = new();

            foreach (var (left, right) in values)
            {
                var leftNode = nodes[left] = nodes.TryGetValue(left, out var first) ? first : new();
                if(right != "start") leftNode.Add(right);

                var rightNode = nodes[right] = nodes.TryGetValue(right, out var second) ? second : new();
                if(left != "start") rightNode.Add(left);
            }

            return nodes;
        }       

        [TestMethod]
        public void Problem1()
        {
            var nodes = GetNodes(values);

            IEnumerable<List<string>> Recurse(string node, List<string> smallVisited, List<string> path)
            {
                if (char.IsLower(node[0])) smallVisited.Add(node);
                var visited = new List<string>(path) { node };

                if (node != "end")
                {
                    var toVisit = nodes[node].Except(smallVisited);
                    foreach (var sibling in toVisit)
                    {
                        foreach (var subPath in Recurse(sibling, new List<string>(smallVisited), new List<string>(visited)))
                        {
                            yield return subPath;
                        }
                    }
                }
                else
                {
                    yield return visited;
                }
            }

            int result = Recurse("start", new(), new()).Count();

            Assert.AreEqual(result, 3485);
        }

        [TestMethod]
        public void Problem2()
        {
            var nodes = GetNodes(values);

            IEnumerable<List<string>> Recurse(string node, bool doubleVisited, List<string> smallVisited, List<string> path)
            {
                doubleVisited |= smallVisited.Contains(node);
                if (char.IsLower(node[0])) smallVisited.Add(node);

                var visited = new List<string>(path) { node };

                if (node != "end")
                {
                    var toVisit = doubleVisited ? nodes[node].Except(smallVisited) : nodes[node];
                    foreach (var sibling in toVisit)
                    {
                        foreach (var subPath in Recurse(sibling, doubleVisited, new List<string>(smallVisited), new List<string>(visited)))
                        {
                            yield return subPath;
                        }
                    }
                }
                else
                {
                    yield return visited;
                }
            }

            int result = Recurse("start", false, new(), new()).Count();

            Assert.AreEqual(result, 85062);
        }
    }
}
