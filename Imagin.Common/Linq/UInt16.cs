using System;

namespace Imagin.Common.Linq
{
    public static class UInt16Extensions
    {
        public static ushort Coerce(this UInt16 value, UInt16 maximum, UInt16 minimum = 0) 
            => System.Math.Max(System.Math.Min(value, maximum), minimum);

        public static Decimal Decimal(this UInt16 value)
            => Convert.ToDecimal(value);

        public static Double Double(this UInt16 value)
            => Convert.ToDouble(value);

        public static Int16 Int16(this UInt16 value)
            => Convert.ToInt16(value);

        public static Int32 Int32(this UInt16 value)
            => Convert.ToInt32(value);

        public static Int64 Int64(this UInt16 value)
            => Convert.ToInt64(value);

        public static UInt16 Maximum(this UInt16 input, UInt16 maximum)
            => input.Coerce(maximum, UInt16.MinValue);

        public static UInt16 Minimum(this UInt16 input, UInt16 minimum)
            => input.Coerce(UInt16.MaxValue, minimum);

        public static Single Single(this UInt16 value)
            => Convert.ToSingle(value);

        public static UInt32 UInt32(this UInt16 value)
            => Convert.ToUInt32(value);

        public static UInt64 UInt64(this UInt16 value)
            => Convert.ToUInt64(value);

        public static bool Within(this ushort input, ushort minimum, ushort maximum) => input >= minimum && input <= maximum;
    }
}