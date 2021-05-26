using Imagin.Common.Text;
using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class BulletMultiConverter : MultiConverter<string>
    {
        string Letter(int index)
        {
            const int columns = 26;
            //ceil(log26(Int32.Max))
            const int digitMaximum = 7;

            const string digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            if (index <= columns)
                return digits[index - 1].ToString();

            var result = new StringBuilder().Append(' ', digitMaximum);

            var current = index;
            var offset = digitMaximum;
            while (current > 0)
            {
                result[--offset] = digits[--current % columns];
                current /= columns;
            }

            return result.ToString(offset, digitMaximum - offset);
        }

        string Roman(int value)
        {
            if ((value < 0) || (value > 3999))
                throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");

            if (value < 1)
                return string.Empty;

            if (value >= 1000)
                return "M" + Roman(value - 1000);

            if (value >= 900)
                return "CM" + Roman(value - 900);

            if (value >= 500)
                return "D" + Roman(value - 500);

            if (value >= 400)
                return "CD" + Roman(value - 400);

            if (value >= 100)
                return "C" + Roman(value - 100);

            if (value >= 90)
                return "XC" + Roman(value - 90);

            if (value >= 50)
                return "L" + Roman(value - 50);

            if (value >= 40)
                return "XL" + Roman(value - 40);

            if (value >= 10)
                return "X" + Roman(value - 10);

            if (value >= 9)
                return "IX" + Roman(value - 9);

            if (value >= 5)
                return "V" + Roman(value - 5);

            if (value >= 4)
                return "IV" + Roman(value - 4);

            if (value >= 1)
                return "I" + Roman(value - 1);

            throw new ArgumentOutOfRangeException();
        }

        string Do(Bullets bullet, int index)
        {
            switch (bullet)
            {
                case Bullets.LetterUpperPeriod:
                    return $"{Letter(index).ToString().ToUpper()}.";

                case Bullets.LetterUpperParenthesis:
                    return $"{Letter(index).ToString().ToUpper()})";

                case Bullets.LetterLowerPeriod:
                    return $"{Letter(index).ToString().ToLower()}.";

                case Bullets.LetterLowerParenthesis:
                    return $"{Letter(index).ToString().ToLower()})";

                case Bullets.NumberPeriod:
                    return $"{index}.";

                case Bullets.NumberParenthesis:
                    return $"{index})";

                case Bullets.RomanNumberUpperPeriod:
                    return $"{Roman(index).ToString().ToUpper()}.";

                case Bullets.RomanNumberUpperParenthesis:
                    return $"{Roman(index).ToString().ToUpper()})";

                case Bullets.RomanNumberLowerPeriod:
                    return $"{Roman(index).ToString().ToLower()}.";

                case Bullets.RomanNumberLowerParenthesis:
                    return $"{Roman(index).ToString().ToLower()})";
            }
            return null;
        }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 1)
            {
                if (values[0] is Bullets)
                    return Do((Bullets)values[0], 1);
            }
            else if (values?.Length == 3)
            {
                if (values[2] is int)
                {
                    if (values[1] is Bullets)
                    {
                        var index = 0;
                        if (values[0] is FrameworkElement)
                        {
                            var indexConverter = new IndexConverter();
                            index = (int)indexConverter.Convert(values[0], null, 1, null);
                        }
                        else if (values[0] is int)
                        {
                            index = (int)values[0];
                        }
                        else if (values[0] is string)
                        {
                            int.TryParse(values[0].ToString(), out index);
                        }
                        return Do((Bullets)values[1], index);
                    }
                }
            }
            return string.Empty;
        }
    }
}