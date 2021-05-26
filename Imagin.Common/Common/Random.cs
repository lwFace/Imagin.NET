using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common
{
    public static class Random
    {
        public static readonly System.Random Current = new System.Random();

        public static Boolean NextBoolean()
            => NextInt32(0, 2) == 1;

        public static bool NextBoolean(double probability)
            => NextDouble() < probability / 100.0;

        public static byte NextByte()
            => NextInt32(0, 256).Byte();

        public static Double NextDouble()
            => Current.NextDouble();

        public static Single NextSingle()
            => Current.NextDouble().Single();

        public static Single NextSingle(Single range)
            => (Current.NextDouble() * range).Single();

        public static Single NextSingle(Single min, Single max)
            => ((Current.NextDouble() * (max - min)) + min).Single();

        public static Int32 NextInt32(Int32 min, Int32 max)
            => Current.Next(min, max);

        public static Int32 NextInt32(IEnumerable<Int32> values)
            => NextInt32(values.ToArray());

        public static Int32 NextInt32(params Int32[] values)
            => values[NextInt32(0, values.Length)];

        public static UInt16 NextUInt16(UInt16 minimum, UInt16 maximum)
            => Current.Next(minimum.Int32(), maximum.Int32()).UInt16();

        public static UInt32 NextUInt32(UInt32 minimum, UInt32 maximum)
            => Current.Next(minimum.Int32(), maximum.Int32()).UInt32();

        public static UInt64 NextUInt64(UInt64 minimum, UInt64 maximum)
            => Current.Next(minimum.Int32(), maximum.Int32()).UInt64();

        public static string String(string AllowedCharacters, int MinLength, int MaxLength)
        {
            char[] characters = new char[MaxLength];
            int setLength = AllowedCharacters.Length;

            int length = Current.Next(MinLength, MaxLength + 1);
            for (int i = 0; i < length; ++i)
                characters[i] = AllowedCharacters[Current.Next(setLength)];

            return new string(characters, 0, length);
        }
    }
}