using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

namespace Imagin.Common.Linq
{
    public static class StringExtensions
    {
        public static String After(this String input, String i)
        {
            var pos_a = input.LastIndexOf(i);

            if (pos_a == -1)
                return string.Empty;

            var adjusted = pos_a + i.Length;

            return adjusted >= input.Length ? string.Empty : input.Substring(adjusted);
        }

        public static Boolean AlphaNumeric(this String input)
            => Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");

        public static String Before(this String input, String i)
        {
            var result = input.IndexOf(i);
            return result == -1 ? string.Empty : input.Substring(0, result);
        }

        public static String Between(this String input, String a, String b)
        {
            var pos_a = input.IndexOf(a);
            var pos_b = input.LastIndexOf(b);

            if (pos_a == -1)
                return string.Empty;

            if (pos_b == -1)
                return string.Empty;

            var adjusted = pos_a + a.Length;
            return adjusted >= pos_b ? string.Empty : input.Substring(adjusted, pos_b - adjusted);
        }

        public static Boolean? Boolean(this String input)
        {
            switch (input.ToLower())
            {
                case "true":
                case "t":
                case "1":
                    return true;
                case "false":
                case "f":
                case "0":
                    return false;
            }
            return null;
        }

        public static string Capitalize(this string input)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static bool OnlyContains(this string input, char character)
        {
            foreach (var i in input)
            {
                if (!i.Equals(character))
                    return false;
            }
            return true;
        }

        public static Boolean Empty(this String input)
            => input.Length == 0;

        public static Boolean EndsWithAny(this String input, params Char[] values)
            => input.EndsWithAny(values.Select(i => i.ToString()).ToArray());

        public static Boolean EndsWithAny(this String input, params object[] values)
            => input.EndsWithAny(values.Select(i => i.ToString()).ToArray());

        public static Boolean EndsWithAny(this String input, params String[] values)
        {
            foreach (var i in values)
            {
                if (input.EndsWith(i))
                    return true;
            }
            return false;
        }

        public static String F(this String input, params object[] arguments) => string.Format(input, arguments);

        public static Boolean NullOrEmpty(this String input) => string.IsNullOrEmpty(input);

        public static Boolean NullOrWhitespace(this String input) => string.IsNullOrWhiteSpace(input);

        public static Boolean Numeric(this String input) => Regex.IsMatch(input, @"^[0-9]+$");

        public static string PadLeft(this string input, char i, int repeat) => input.PadLeft(input.Length + repeat, i);

        /// <summary>
        /// Gets the number of times the given <see cref="char"/> repeats.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static int Repeats(this string input, char character)
        {
            int result = 0;
            foreach (var i in input)
            {
                if (i.Equals(character))
                    result++;
            }
            return result;
        }

        public static String ReplaceBetween(this String input, char a, char b, String replace)
        {
            int? i0 = null;
            int? i1 = null;
            int length = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (i0 == null)
                {
                    if (input[i] == a)
                        i0 = i;
                }
                else
                {
                    if (input[i] == b)
                    {
                        i1 = i;
                        break;
                    }

                    length++;
                }
            }

            if (i0 != null && i1 != null)
                return input.Substring(0, i0.Value + 1) + replace + input.Substring(i1.Value, input.Length - i1.Value);

            return string.Empty;
        }

        public static String SplitCamel(this String input)
        {
            //New way: Previous character must be lowercase and current character must be uppercase.
            var result = new System.Text.StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                if (result.Length > 0)
                {
                    if (char.IsLetter(input[i - 1]) && char.IsLetter(input[i]))
                    {
                        if (char.IsLower(input[i - 1]) && char.IsUpper(input[i]))
                            result.Append(' ');
                    }
                }
                result.Append(input[i]);
            }
            return result.ToString();
            //Old way: Regex.Replace(Regex.Replace(input, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        //.................................................................................

        public static Byte Byte(this String input)
        {
            byte.TryParse(input, out byte result);
            return result;
        }

        public static Char Char(this String input)
        {
            char.TryParse(input, out char result);
            return result;
        }

        public static DateTime DateTime(this String input)
        {
            System.DateTime.TryParse(input, out DateTime result);
            return result;
        }

        public static Decimal Decimal(this String input)
        {
            decimal.TryParse(input, out decimal result);
            return result;
        }

        public static Double Double(this String input)
        {
            double.TryParse(input, out double result);
            return result;
        }

        public static Int16 Int16(this String input)
        {
            short.TryParse(input, out short result);
            return result;
        }

        public static Int32 Int32(this String input)
        {
            int.TryParse(input, out int result);
            return result;
        }

        public static Int64 Int64(this String input)
        {
            long.TryParse(input, out long result);
            return result;
        }

        public static IEnumerable<int> Int32Array(this String input, Char separator = ',')
            => input.Int32Array(separator as char?);

        public static IEnumerable<int> Int32Array(this String input, Char? separator)
        {
            if (String.IsNullOrEmpty(input))
                yield break;

            if (separator == null)
            {
                foreach (var i in input.ToArray())
                    yield return i.ToString().Int32();
            }
            else
            {
                foreach (var i in input.Split(separator.Value))
                    yield return i.Int32();
            }
        }

        public static SecureString SecureString(this string input)
        {
            var result = new SecureString();
            if (!input.NullOrWhitespace())
            {
                foreach (char c in input)
                    result.AppendChar(c);
            }
            return result;
        }

        public static Single Single(this String input)
        {
            float.TryParse(input, out float result);
            return result;
        }

        public static TimeSpan TimeSpan(this String input)
        {
            System.TimeSpan.TryParse(input, out TimeSpan result);
            return result;
        }

        public static Boolean TryParse<T>(this String input, out T result, Boolean ignoreCase = true) where T : struct, IFormattable, IComparable, IConvertible => Enum.TryParse(input, ignoreCase, out result);

        public static UDouble UDouble(this String input)
        {
            Common.UDouble.TryParse(input, out UDouble result);
            return result;
        }

        public static UInt16 UInt16(this String input)
        {
            ushort.TryParse(input, out ushort Result);
            return Result;
        }

        public static UInt32 UInt32(this String input)
        {
            uint.TryParse(input, out uint Result);
            return Result;
        }

        public static UInt64 UInt64(this String input)
        {
            ulong.TryParse(input, out ulong Result);
            return Result;
        }

        public static Uri Uri(this String input, UriKind kind = UriKind.RelativeOrAbsolute)
        {
            System.Uri.TryCreate(input, kind, out Uri result);
            return result;
        }

        public static Version Version(this String input, Char delimiter = '.')
        {
            int major = 0, minor = 0, build = 0;
            string[] tokens = input.Split(delimiter);
            if (tokens.Length > 0)
            {
                int.TryParse(tokens[0], out major);
                if (tokens.Length > 1)
                {
                    int.TryParse(tokens[1], out minor);
                    if (tokens.Length > 2)
                        int.TryParse(tokens[2], out build);
                }
            }
            return new Version(major, minor, build);
        }
    }
}
