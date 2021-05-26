using System;

namespace Imagin.Common.Linq
{
    public static class SingleExtensions
    {
        public static Single Absolute(this Single value) => System.Math.Abs(value);

        public static Single Ceiling(this Single value) => System.Math.Ceiling(value).Single();

        public static Single Coerce(this Single value, Single maximum, Single minimum = 0f) => System.Math.Max(System.Math.Min(value, maximum), minimum);

        public static Decimal Decimal(this Single value) => Convert.ToDecimal(value);

        public static Double Double(this Single value) => Convert.ToDouble(value);

        public static Single Floor(this Single value) => System.Math.Floor(value).Single();

        public static Int16 Int16(this Single value) => Convert.ToInt16(value);

        public static Int32 Int32(this Single value) => Convert.ToInt32(value);

        public static Int64 Int64(this Single value) => Convert.ToInt64(value);

        public static Single Negate(this Single value) => -value;

        public static UInt16 UInt16(this Single value) => Convert.ToUInt16(value);

        public static UInt32 UInt32(this Single value) => Convert.ToUInt32(value);

        public static UInt64 UInt64(this Single value) => Convert.ToUInt64(value);

        public static bool Within(this float input, float minimum, float maximum) => input >= minimum && input <= maximum;
    }
}