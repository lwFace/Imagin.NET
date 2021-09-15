using Imagin.Common.Math;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    public static class HexadecimalExtensions
    {
        public static Color Color(this Hexadecimal input)
        {
            var result = input.Convert();
            return System.Windows.Media.Color.FromArgb(result.A, result.R, result.G, result.B);
        }

        public static SolidColorBrush SolidColorBrush(this Hexadecimal input) => new BrushConverter().ConvertFrom($"#{input}") as SolidColorBrush;
    }
}