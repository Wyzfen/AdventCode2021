using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day16
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly string values = Utils.FromFile<string>($"{day}.txt").First().HexStringToBinary();

        public class StringStream
        {
            private String str;
            private int index = 0;

            public StringStream(String str) => this.str = str;

            public String Take(int count)
            {
                if (index + count > str.Length) count = str.Length - index;
                var value = str.Substring(index, count);
                index += count;
                return value;
            }

            public bool IsEmpty => index >= str.Length;
        }

        public class Packet
        {
            public int Version;
            public PacketType Type;

            public Packet[] SubPackets;
            
            BigInteger Value = 0; // only for type = 4

            public enum PacketType: int
            {
                Sum = 0,
                Product = 1,
                Min = 2,
                Max = 3,
                Value = 4,
                Greater = 5,
                Less = 6,
                Equal = 7
            }

            private static BigInteger ParseValue(StringStream input)
            {
                var str = new StringBuilder();
                string current;
                do
                {
                    current = input.Take(5);
                    str.Append(current.Substring(1, 4));
                } while (current[0] == '1');

                return str.ToString().BinaryToInt();
            }

            private static Packet [] ParseOperator(StringStream input)
            {
                var packets = new List<Packet>();
                if(input.Take(1) == "0")
                {
                    int length = (int) input.Take(15).BinaryToInt();
                    var subStream = new StringStream(input.Take(length));
                    while(!subStream.IsEmpty)
                    {
                        packets.Add(Parse(subStream));
                    }
                }
                else
                {
                    int packetCount = (int) input.Take(11).BinaryToInt();
                    for(int i = 0; i < packetCount; i++)
                    {
                        packets.Add(Parse(input));
                    }
                }

                return packets.ToArray();
            }

            public static Packet Parse(StringStream input)
            {
                int version = (int) input.Take(3).BinaryToInt();
                PacketType type = (PacketType)(int) input.Take(3).BinaryToInt();
                BigInteger value = 0;
                Packet[] packets = Array.Empty<Packet>();

                switch(type)
                {
                    case PacketType.Value:
                        value = ParseValue(input);
                        break;
                    default: // operator
                        packets = ParseOperator(input);
                        break;
                }

                return new Packet { Version = version, Type = type, Value = value, SubPackets = packets };
            }

            private BigInteger Left => SubPackets[0].Process();
            private BigInteger Right => SubPackets[1].Process();

            public BigInteger Process() => Type switch
            {
                PacketType.Sum => SubPackets.Aggregate((BigInteger)0, (s, p) => s + p.Process()),
                PacketType.Product => SubPackets.Aggregate((BigInteger)1, (s, p) => s * p.Process()),
                PacketType.Min => SubPackets.Skip(1).Aggregate(Left, (s, p) => BigInteger.Min(s, p.Process())),
                PacketType.Max => SubPackets.Aggregate((BigInteger)0, (s, p) => BigInteger.Max(s, p.Process())),
                PacketType.Value => Value,
                PacketType.Greater => Left > Right ? 1 : 0,
                PacketType.Less => Left < Right ? 1 : 0,
                PacketType.Equal => Left == Right ? 1 : 0,
                _ => 0,
            };
        }


        [TestMethod]
        public void Problem1()
        {
            var packet = Packet.Parse(new StringStream(values));

            int SumVersions(Packet packet) => packet.Version + packet.SubPackets.Sum(p => SumVersions(p));

            int result = SumVersions(packet);

            Assert.AreEqual(result, 981);
        }

        [TestMethod]
        public void Problem2()
        {
            var packet = Packet.Parse(new StringStream(values));

            BigInteger result = packet.Process();

            Assert.AreEqual(result, 299227024091);
        }
    }

    public static class StringExtensions
    {

        public static string HexStringToBinary(this string hexstring) => String.Join(String.Empty,
              hexstring.Select(
                c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
              )
            );

        public static BigInteger BinaryToInt(this string binarystring) => binarystring.Aggregate(new BigInteger(0), (b, c) => (b << 1) + (c == '1' ? 1 : 0));
    }
}
