using Imagin.Common.Linq;
using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Object), typeof(Object))]
    public class NullConverter : Converter<Object, Object>
    {
        protected override bool AllowNull => true;

        protected override Value<Object> ConvertTo(ConverterData<Object> input)
        {
            if (input.Value == null || (input.Value is string i && i.Empty()))
                return Nothing.Do;

            return input.Value;
        }

        protected override Value<Object> ConvertBack(ConverterData<Object> input) => Nothing.Do;
    }
}