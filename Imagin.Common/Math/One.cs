using Imagin.Common.Linq;

namespace Imagin.Common.Math
{
    /// <summary>
    /// Represents a number in the range of [0, 1] (input is truncated if outside of range).
    /// </summary>
    public struct One
    {
        public readonly double Value;

        /// ...............................................................

        public One(double value) => Value = value.Coerce(1);

        /// ...............................................................

        public static implicit operator One(double input) => new One(input);

        public static implicit operator double(One input) => input.Value;

        /// ...............................................................

        public static bool operator <(One a, One b) => a.Value < b.Value;

        public static bool operator >(One a, One b) => a.Value > b.Value;

        /// ...............................................................

        public static bool operator ==(One a, One b) => a.Value == b.Value;

        public static bool operator !=(One a, One b) => a.Value != b.Value;

        public static bool operator <=(One a, One b) => a.Value <= b.Value;

        public static bool operator >=(One a, One b) => a.Value >= b.Value;

        /// ...............................................................

        public static double operator +(One a, One b) => a.Value + b.Value;

        public static double operator -(One a, One b) => a.Value - b.Value;

        public static double operator /(One a, One b) => a.Value / b.Value;

        public static double operator *(One a, One b) => a.Value * b.Value;

        /// ...............................................................

        public static double operator +(One a, double b) => a.Value + b;

        public static double operator -(One a, double b) => a.Value - b;

        public static double operator /(One a, double b) => a.Value / b;

        public static double operator *(One a, double b) => a.Value * b;

        /// ...............................................................

        public override bool Equals(object i) => i is One ? this == (One)i : i is double || i is float || i is int ? this == new One(System.Convert.ToDouble(i)) : false;

        public override int GetHashCode() => Value.GetHashCode();

        /// ...............................................................

        public static One Parse(string input) => double.Parse(input);

        public static bool TryParse(string input, out One result)
        {
            var r = double.TryParse(input, out double d);
            result = d;
            return r;
        }

        /// ...............................................................

        public string ToString(string format) => Value.ToString(format);

        public override string ToString() => Value.ToString();
    }
}