using Imagin.Common.Data;
using System;

namespace Imagin.Common.Linq
{
    public static class Int64Extensions
    {
        public static Int64 Add(this Int64 value, Int64 increment) => value + increment;

        public static Int64 Absolute(this Int64 value) => System.Math.Abs(value);

        public static Int64 Coerce(this Int64 value, Int64 maximum, Int64 minimum = 0L) => System.Math.Max(System.Math.Min(value, maximum), minimum);

        public static Int64 Divide(this Int64 value, Int64 divisor) => value / divisor;

        public static double Double(this Int64 value) => Convert.ToDouble(value);

        public static string FileSize(this Int64 value, FileSizeFormat format, int round = 1) => value.Coerce(Int64.MaxValue).UInt64().FileSize(format, round);

        public static Int16 Int16(this Int64 value) => Convert.ToInt16(value);

        public static Int32 Int32(this Int64 value) => Convert.ToInt32(value);

        public static Int64 K(this Int64 value) => value * 1024L;

        public static Int64 M(this Int64 value) => value * 1024L * 1024L;

        public static Int64 Maximum(this Int64 input, Int64 maximum) => input.Coerce(maximum, Int64.MinValue);

        public static Int64 Minimum(this Int64 input, Int64 minimum) => input.Coerce(Int64.MaxValue, minimum);

        public static Int64 Multiply(this Int64 value, Int64 scalar) => value * scalar;

        public static Int64 Negate(this Int64 value) => -value;

        public static Single Single(this Int64 value) => Convert.ToSingle(value);

        public static Int64 Subtract(this Int64 value, Int64 decrement) => value - decrement;

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of ticks.
        /// </summary>
        public static TimeSpan Ticks(this Int64 input) => TimeSpan.FromTicks(input);

        public static UInt16 UInt16(this Int64 value) => Convert.ToUInt16(value);

        public static UInt32 UInt32(this Int64 value) => Convert.ToUInt32(value);

        public static UInt64 UInt64(this Int64 value) => Convert.ToUInt64(value);

        public static bool Within(this long input, long minimum, long maximum) => input >= minimum && input <= maximum;
    }
}