using System;
using System.Windows.Data;

namespace Paint
{
    [ValueConversion(typeof(object[]), typeof(LayerTool))]
    public class LayerToolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new LayerTool(values?.GetValue(1) as Layer, values?.GetValue(0) as Tool);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
