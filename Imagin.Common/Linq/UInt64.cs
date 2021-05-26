using Imagin.Common.Data;
using System;

namespace Imagin.Common.Linq
{
    public static class UInt64Extensions
    {
        public static ulong Coerce(this UInt64 value, UInt64 maximum, UInt64 minimum = 0L) 
            => System.Math.Max(System.Math.Min(value, maximum), minimum);

        public static string FileSize(this UInt64 value, FileSizeFormat format, int round = 1)
        {
            if (format == FileSizeFormat.Bytes)
                return value.ToString();

            var Labels = new string[]
            {
                "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"
            };

            if (format == FileSizeFormat.BinaryUsingSI || format == FileSizeFormat.DecimalUsingSI)
            {
                Labels = new string[]
                {
                    "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
                };
            }

            if (value == 0)
                return "0 B";

            var f = format == FileSizeFormat.BinaryUsingSI || format == FileSizeFormat.IECBinary ? (ulong)1024 : 1000;

            var m = (int)System.Math.Log(value, f);
            var a = (decimal)value / (1L << (m * 10));

            if (System.Math.Round(a, round) >= 1000)
            {
                m += 1;
                a /= f;
            }

            var result = string.Format("{0:n" + round + "}", a);

            var j = result.Length;
            for (var i = result.Length - 1; i >= 0; i--)
            {
                if (result[i] == '.')
                {
                    j--;
                    break;
                }
                if (result[i] == '0')
                {
                    j--;
                }
                else break;
            }

            return $"{result.Substring(0, j)} {Labels[m]}"; ;
        }

        public static Decimal Decimal(this UInt64 value)
            => Convert.ToDecimal(value);

        public static Double Double(this UInt64 value)
            => Convert.ToDouble(value);

        public static Int16 Int16(this UInt64 value)
            => Convert.ToInt16(value);

        public static Int32 Int32(this UInt64 value)
            => Convert.ToInt32(value);

        public static Int64 Int64(this UInt64 value)
            => Convert.ToInt64(value);

        public static UInt64 Maximum(this UInt64 input, UInt64 maximum)
            => input.Coerce(maximum, UInt64.MinValue);

        public static UInt64 Minimum(this UInt64 input, UInt64 minimum)
            => input.Coerce(UInt64.MaxValue, minimum);

        public static Single Single(this UInt64 value)
            => Convert.ToSingle(value);

        public static UInt16 UInt16(this UInt64 value)
            => Convert.ToUInt16(value);

        public static UInt32 UInt32(this UInt64 value)
            => Convert.ToUInt32(value);

        public static bool Within(this ulong input, ulong minimum, ulong maximum) => input >= minimum && input <= maximum;
    }
}