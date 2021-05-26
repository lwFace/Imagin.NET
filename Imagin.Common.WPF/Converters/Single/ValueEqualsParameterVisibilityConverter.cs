using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ValueEqualsParameterVisibilityConverter : Converter<Object, Visibility>
    {
        protected override Value<Visibility> ConvertTo(ConverterData<Object> input) => input.ActualValue.Equals(input.Parameter).Visibility();

        protected override Value<Object> ConvertBack(ConverterData<Visibility> input) => input.Parameter;
    }
}