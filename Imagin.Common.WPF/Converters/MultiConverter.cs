using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(object))]
    public class MultiConverter<Result> : IMultiValueConverter
    {
        readonly Func<object[], Result> To;

        public MultiConverter() : base() { }

        public MultiConverter(Func<object[], Result> to) => To = to;

        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => To.Invoke(values);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}