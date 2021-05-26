using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Object), typeof(Type))]
    public class ObjectToTypeConverter : Converter<Object, Type>
    {
        protected override Value<Type> ConvertTo(ConverterData<Object> input) => input.ActualValue.GetType();

        protected override Value<Object> ConvertBack(ConverterData<Type> input) => Nothing.Do;
    }
}