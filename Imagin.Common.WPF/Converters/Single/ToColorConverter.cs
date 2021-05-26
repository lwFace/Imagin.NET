using Imagin.Common.Linq;
using Imagin.Common.Math;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Color), typeof(Color))]
    public class ColorWithoutAlphaConverter : Converter<Color, Color>
    {
        protected override Value<Color> ConvertTo(ConverterData<Color> input) => input.ActualValue.A(255);

        protected override Value<Color> ConvertBack(ConverterData<Color> input) => Nothing.Do;
    }

    [ValueConversion(typeof(System.Drawing.Color), typeof(Color))]
    public class ColorToColorConverter : Converter<System.Drawing.Color, Color>
    {
        protected override Value<Color> ConvertTo(ConverterData<System.Drawing.Color> input) => input.ActualValue.Double();

        protected override Value<System.Drawing.Color> ConvertBack(ConverterData<Color> input) => input.ActualValue.Int32();
    }

    [ValueConversion(typeof(Hexadecimal), typeof(Color))]
    public class HexadecimalToColorConverter : Converter<Hexadecimal, Color>
    {
        protected override Value<Color> ConvertTo(ConverterData<Hexadecimal> input) => input.ActualValue.Color();

        protected override Value<Hexadecimal> ConvertBack(ConverterData<Color> input) => input.ActualValue.Hexadecimal();
    }
}