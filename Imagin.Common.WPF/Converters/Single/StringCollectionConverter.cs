using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Object), typeof(StringCollection))]
    public class StringCollectionConverter : Converter<Object, StringCollection>
    {
        protected override Value<StringCollection> ConvertTo(ConverterData<Object> input)
        {
            var result = new StringCollection();
            if (input.ActualValue is IEnumerable a)
                a.ForEach(i => result.Add(i.ToString()));

            if (input.ActualValue is IList b)
                b.ForEach(i => result.Add(i.ToString()));

            return result;
        }

        protected override Value<Object> ConvertBack(ConverterData<StringCollection> input) => Nothing.Do;
    }
}