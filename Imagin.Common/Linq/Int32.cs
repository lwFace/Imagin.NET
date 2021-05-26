using System;

namespace Imagin.Common.Linq
{
    public static class Int32Extensions
    {
        public static void For(this int input, int whileLessThan, Action<int> action)
        {
            for (var i = input; i < whileLessThan; i++)
                action(i);
        }

        //...............................................................................

        public static Int32 Add(this Int32 input, Int32 Increment) => input + Increment;

        public static Int32 Absolute(this Int32 input) => System.Math.Abs(input);

        public static Byte Byte(this Int32 input) => Convert.ToByte(input);

        public static Int32 Coerce(this Int32 input, Int32 maximum, Int32 minimum = 0) => System.Math.Max(System.Math.Min(input, maximum), minimum);

        public static Decimal Decimal(this Int32 input) => Convert.ToDecimal(input);

        public static Int32 Divide(this Int32 input, Int32 divisor) => input / divisor;

        public static Double Double(this Int32 input) => Convert.ToDouble(input);

        public static Int32 K(this Int32 input) => input * 1024;

        public static Int16 Int16(this Int32 input) => Convert.ToInt16(input);

        public static Int64 Int64(this Int32 input) => Convert.ToInt64(input);

        public static Boolean Even(this Int32 input) => input == 0 ? true : input % 2 == 0;

        public static Boolean Odd(this Int32 input) => !Even(input);

        public static Int32 M(this Int32 input) => input * 1024 * 1024;

        public static Int32 Maximum(this Int32 input, Int32 maximum) => input.Coerce(maximum, int.MinValue);

        public static Int32 Minimum(this Int32 input, Int32 minimum) => input.Coerce(int.MaxValue, minimum);

        public static Int32 Multiply(this Int32 input, Int32 scalar) => input * scalar;

        public static Int32 Negate(this Int32 input) => -input;

        public static String Ordinal(this Int32 input)
        {
            var result = String.Empty;
            switch (input)
            {
                case 1:
                    result = "st";
                    break;
                case 2:
                    result = "nd";
                    break;
                case 3:
                    result = "rd";
                    break;
                default:
                    result = "th";
                    break;
            }
            return $"{input}{result}";
        }

        public static Single Single(this Int32 input) => Convert.ToSingle(input);

        public static Byte[] SplitBytes(this Int32 input)
        {
            String s = input.ToString();
            Byte[] result = new Byte[s.Length];

            Int32 i = 0;
            foreach (char c in s)
            {
                result[i] = c.ToString().Byte();
                i++;
            }
            return result;
        }

        public static Int32 Subtract(this Int32 input, Int32 decrement) => input - decrement;

        public static UInt16 UInt16(this Int32 input) => Convert.ToUInt16(input);

        public static UInt32 UInt32(this Int32 input) => Convert.ToUInt32(input);

        public static UInt64 UInt64(this Int32 input) => Convert.ToUInt64(input);

        public static bool Within(this Int32 input, Int32 minimum, Int32 maximum) => input >= minimum && input <= maximum;
    }
}