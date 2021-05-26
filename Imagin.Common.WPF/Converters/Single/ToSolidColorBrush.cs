using Imagin.Common.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToSolidColorBrushConverter : Converter<Color, SolidColorBrush>
    {
        protected override Value<SolidColorBrush> ConvertTo(ConverterData<Color> input) => new SolidColorBrush(input.ActualParameter == 0 ? input.ActualValue : input.ActualValue.A(255));

        protected override Value<Color> ConvertBack(ConverterData<SolidColorBrush> input) => input.ActualValue.Color;
    }
}