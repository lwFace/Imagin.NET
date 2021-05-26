using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(String), typeof(PathCollection))]
    public class PathCollectionConverter : Converter<String, PathCollection>
    {
        protected override Value<PathCollection> ConvertTo(ConverterData<String> input)
        {
            if (!input.ActualValue.NullOrEmpty())
            {
                var path = Path.GetFirstExtension(input.ActualValue).NullOrEmpty() ? input.ActualValue : System.IO.Path.GetDirectoryName(input.ActualValue);
                var result = new PathCollection(new Filter(ItemType.File));
                result.Refresh(path);
                return result;
            }
            return Nothing.Do;
        }

        protected override Value<String> ConvertBack(ConverterData<PathCollection> input) => Nothing.Do;
    }
}