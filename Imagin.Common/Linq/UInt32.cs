using System;

namespace Imagin.Common.Linq
{
    public static class UInt32Extensions
    {
        public static UInt32 Coerce(this UInt32 value, UInt32 maximum, UInt32 minimum = 0)
            => System.Math.Max(System.Math.Min(value, maximum), minimum);

        public static Decimal Decimal(this UInt32 value)
            => Convert.ToDecimal(value);

        public static Double Double(this UInt32 value)
            => Convert.ToDouble(value);

        public static Int16 Int16(this UInt32 value)
            => Convert.ToInt16(value);

        public static Int32 Int32(this UInt32 value)
            => Convert.ToInt32(value);

        public static Int64 Int64(this UInt32 value)
            => Convert.ToInt64(value);

        public static UInt32 Maximum(this UInt32 input, UInt32 maximum)
            => input.Coerce(maximum, UInt32.MinValue);

        public static UInt32 Minimum(this UInt32 input, UInt32 minimum)
            => input.Coerce(UInt32.MaxValue, minimum);

        public static Single Single(this UInt32 value)
            => Convert.ToSingle(value);

        public static UInt16 UInt16(this UInt32 value)
            => Convert.ToUInt16(value);

        public static UInt64 UInt64(this UInt32 value)
            => Convert.ToUInt64(value);

        public static bool Within(this uint input, uint minimum, uint maximum) => input >= minimum && input <= maximum;
    }
}