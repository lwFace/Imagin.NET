using Imagin.Common.Media;
using System;

namespace Imagin.Common.Linq
{
    public static class DoubleExtensions
    {        
        /// <summary>
             /// Gets a <see cref="TimeSpan"/> representing the given number of days.
             /// </summary>
        public static TimeSpan Days(this Double input) => TimeSpan.FromDays(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of hours.
        /// </summary>
        public static TimeSpan Hours(this Double input) => TimeSpan.FromHours(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of milliseconds.
        /// </summary>
        public static TimeSpan Milliseconds(this Double input) => TimeSpan.FromMilliseconds(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of minutes.
        /// </summary>
        public static TimeSpan Minutes(this Double input) => TimeSpan.FromMinutes(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of months.
        /// </summary>
        public static TimeSpan Months(this Double input) => TimeSpan.FromDays(30 * input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of seconds.
        /// </summary>
        public static TimeSpan Seconds(this Double input) => TimeSpan.FromSeconds(input);

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> representing the given number of years.
        /// </summary>
        public static TimeSpan Years(this Double input) => TimeSpan.FromDays(365 * input);

        //.......................................................................................

        public static Double Add(this Double value, Double increment)
            => value + increment;

        /// <summary>
        /// Gets the absolute value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double Absolute(this Double value)
            => System.Math.Abs(value);

        /// <summary>
        /// Converts to <see cref="byte"/>.
        /// </summary>
        public static Byte Byte(this Double value) => System.Convert.ToByte(value);

        /// <summary>
        /// Rounds up to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double Ceiling(this Double value)
            => System.Math.Ceiling(value);

        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="i">The value to coerce.</param>
        /// <param name="maximum">The maximum to coerce to.</param>
        /// <param name="minimum">The minimum to coerce to.</param>
        /// <returns></returns>
        public static Double Coerce(this Double i, Double maximum, Double minimum = 0)
            => System.Math.Max(System.Math.Min(i, maximum), minimum);

        /// <summary>
        /// Converts a value to the given <see cref="GraphicalUnit"/> (to) based on the given <see cref="GraphicalUnit"/> (from).
        /// </summary>
        /// <param name="value">Number of units.</param>
        /// <param name="from">The unit to convert from.</param>
        /// <param name="to">The unit to convert to.</param>
        /// <param name="ppi">Pixels per inch.</param>
        public static Double Convert(this Double value, GraphicalUnit from, GraphicalUnit to, Double ppi = 72.0)
        {
            var pixels = 0d;
            switch (from)
            {
                case GraphicalUnit.Pixel:
                    pixels = System.Math.Round(value, 0);
                    break;
                case GraphicalUnit.Inch:
                    pixels = System.Math.Round(value * ppi, 0);
                    break;
                case GraphicalUnit.Centimeter:
                    pixels = System.Math.Round((value * ppi) / 2.54, 0);
                    break;
                case GraphicalUnit.Millimeter:
                    pixels = System.Math.Round((value * ppi) / 25.4, 0);
                    break;
                case GraphicalUnit.Point:
                    pixels = System.Math.Round((value * ppi) / 72, 0);
                    break;
                case GraphicalUnit.Pica:
                    pixels = System.Math.Round((value * ppi) / 6, 0);
                    break;
                case GraphicalUnit.Twip:
                    pixels = System.Math.Round((value * ppi) / 1140, 0);
                    break;
                case GraphicalUnit.Character:
                    pixels = System.Math.Round((value * ppi) / 12, 0);
                    break;
                case GraphicalUnit.En:
                    pixels = System.Math.Round((value * ppi) / 144.54, 0);
                    break;
            }

            var inches = pixels / ppi;
            var result = pixels;

            switch (to)
            {
                case GraphicalUnit.Inch:
                    result = inches;
                    break;
                case GraphicalUnit.Centimeter:
                    result = inches * 2.54;
                    break;
                case GraphicalUnit.Millimeter:
                    result = inches * 25.4;
                    break;
                case GraphicalUnit.Point:
                    result = inches * 72.0;
                    break;
                case GraphicalUnit.Pica:
                    result = inches * 6.0;
                    break;
                case GraphicalUnit.Twip:
                    result = inches * 1140.0;
                    break;
                case GraphicalUnit.Character:
                    result = inches * 12.0;
                    break;
                case GraphicalUnit.En:
                    result = inches * 144.54;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the cubic root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double CubicRoot(this Double value) 
            => value.Power(1.0 / 3.0);

        /// <summary>
        /// Converts to <see cref="float"/>.
        /// </summary>
        public static Decimal Decimal(this Double value)
            => System.Convert.ToDecimal(value);

        /// <summary>
        /// Divides by the given divisor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static Double Divide(this Double value, Double divisor) 
            => value / divisor;

        /// <summary>
        /// Rounds down to the nearest whole number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double Floor(this Double value) 
            => System.Math.Floor(value);

        public static double FromDegreeToRadian(this double input)
            => Math.Angle.GetRadian(input);

        public static double FromRadianToDegree(this double input)
            => Math.Angle.GetDegree(input);

        public static Int16 Int16(this Double value)
            => System.Convert.ToInt16(value);

        /// <summary>
        /// Converts to <see cref="int"/>.
        /// </summary>
        public static Int32 Int32(this Double value)
            => System.Convert.ToInt32(value);

        /// <summary>
        /// Converts to <see cref="long"/>.
        /// </summary>
        public static Int64 Int64(this Double value)
            => System.Convert.ToInt64(value);

        public static Double Maximum(this Double input, Double maximum)
            => input.Coerce(maximum, Double.MinValue);

        public static Double Minimum(this Double input, Double minimum)
            => input.Coerce(Double.MaxValue, minimum);

        public static Double Modulo(this Double value, Double left, Double? right = null)
        {
            //Detect single-argument case
            if (right == null)
            {
                right = left;
                left = 0;
            }

            //Swap frame order
            if (left > right)
            {
                var tmp = right.Value;
                right = left;
                left = tmp;
            }

            var frame = right.Value - left;
            value = ((value + left) % frame) - left;

            if (value < left)
                value += frame;

            if (value > right)
                value -= frame;

            return value;
        }

        public static byte Multiply(this double input, byte b)
            => (input * b.Double()).Round().Coerce(byte.MaxValue).Byte();

        public static double Multiply(this double a, double b)
            => a * b;

        public static Double Negate(this Double value)
            => -value;

        public static Double NearestFactor(this Double value, Double factor)
            => System.Math.Round((value / factor), MidpointRounding.AwayFromZero) * factor;

        /// <summary>
        /// Raises to the specified power.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Double Power(this Double value, Double power) 
            => System.Math.Pow(value, power);

        /// <summary>
        /// Rounds to the given digits.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Double Round(this Double value, Int32 digits = 0) 
            => System.Math.Round(value, digits);

        /// <summary>
        /// Shifts the decimal point. <para>If negative, shift left; otherwise, shift right.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        public static Double Shift(this Double value, Int32 shifts = 1)
        {
            var LeftOrRight = shifts < 0;

            for (var i = 0; i < shifts.Absolute(); i++)
                value = LeftOrRight ? value / 10 : value * 10;

            return value;
        }

        /// <summary>
        /// Shifts the decimal point and rounds to the given digits. <para>If negative, shift left; otherwise, shift right.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="shifts"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Double ShiftRound(this Double value, Int32 shifts = 1, Int32 digits = 0) 
            => value.Shift(shifts).Round(digits);

        public static Single Single(this Double value)
            => System.Convert.ToSingle(value);

        /// <summary>
        /// Gets the square root.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double SquareRoot(this Double value) 
            => System.Math.Sqrt(value);

        /// <summary>
        /// Subtracts by the given decrement.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static Double Subtract(this Double value, Double decrement) 
            => value - decrement;

        public static UInt16 UInt16(this Double value)
            => System.Convert.ToUInt16(value);

        public static UInt32 UInt32(this Double value) 
            => System.Convert.ToUInt32(value);

        public static UInt64 UInt64(this Double value)
            => System.Convert.ToUInt64(value);

        public static bool Within(this double input, double minimum, double maximum) => input >= minimum && input <= maximum;
    }
}