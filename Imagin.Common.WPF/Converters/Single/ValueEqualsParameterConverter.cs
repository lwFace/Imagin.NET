using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class ValueEqualsParameterConverter : Converter<Object, Boolean>
    {
        protected override Value<Boolean> ConvertTo(ConverterData<Object> input) => input.ActualValue.Equals(input.Parameter);

        protected override Value<Object> ConvertBack(ConverterData<Boolean> input)
        {
            if (!input.ActualValue || input.Parameter == null)
                return Nothing.Do;

            return input.Parameter;
        }
    }
}