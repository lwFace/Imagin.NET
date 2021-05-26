using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class DateTimeStringFormatMultiConverter : MultiConverter<string>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2)
            {
                if (values[0] is DateTime a)
                {
                    if (values[1] is string b)
                        return a.ToString(b);
                }
            }
            return Binding.DoNothing;
        }
    }
}