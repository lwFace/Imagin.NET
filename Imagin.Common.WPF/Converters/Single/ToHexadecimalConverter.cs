using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(System.Drawing.Color), typeof(String))]
    public class Int32ColorToHexadecimalConverter : Converter<System.Drawing.Color, String>
    {
        protected override Value<String> ConvertTo(ConverterData<System.Drawing.Color> input) => input.ActualValue.Double().Hexadecimal(input.ActualParameter == 1).ToString(input.ActualParameter == 1);

        protected override Value<System.Drawing.Color> ConvertBack(ConverterData<String> input)
        {
            var result = new Hexadecimal(input.ActualValue).Color().Int32();
            return input.ActualParameter == 0 ? System.Drawing.Color.FromArgb(255, result.R, result.G, result.B) : input.ActualParameter == 1 ? result : throw input.InvalidParameter;
        }
    }

    [ValueConversion(typeof(Color), typeof(String))]
    public class ColorToHexadecimalConverter : Converter<Color, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Color> input) => input.ActualValue.Hexadecimal(input.ActualParameter == 1).ToString(input.ActualParameter == 1);

        protected override Value<Color> ConvertBack(ConverterData<String> input)
        {
            var result = new Hexadecimal(input.ActualValue).Color();
            return input.ActualParameter == 0 ? Color.FromArgb(255, result.R, result.G, result.B) : input.ActualParameter == 1 ? result : throw input.InvalidParameter;
        }
    }

    [ValueConversion(typeof(SolidColorBrush), typeof(Hexadecimal))]
    public class SolidColorBrushToHexadecimalConverter : Converter<SolidColorBrush, Hexadecimal>
    {
        protected override Value<Hexadecimal> ConvertTo(ConverterData<SolidColorBrush> input) => input.ActualValue.Color.Hexadecimal();

        protected override Value<SolidColorBrush> ConvertBack(ConverterData<Hexadecimal> input) => input.ActualValue.SolidColorBrush();
    }
}