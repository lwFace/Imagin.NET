using System;
using System.Windows;
using System.Windows.Data;

namespace Paint
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class GridLinesVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var visibility = (Visibility)values?.GetValue(0);
            var threshold = (double)values?.GetValue(1);
            var zoom = (double)values?.GetValue(2);

            if (zoom > threshold)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}