using Imagin.Common.Math;
using Imagin.Common.Media;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    public static class ColorExtensions
    {
        #region System.Drawing

        public static Color Double(this System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion

        #region System.Windows.Media

        public static Color A(this Color color, byte a) => Color.FromArgb(a, color.R, color.G, color.B);

        public static Color R(this Color color, byte r) => Color.FromArgb(color.A, r, color.G, color.B);

        public static Color G(this Color color, byte g) => Color.FromArgb(color.A, color.R, g, color.B);

        public static Color B(this Color color, byte b) => Color.FromArgb(color.A, color.R, color.G, b);

        public static Color Blend(this Color color1, Color color2)
        {
            var a1 = color1.A.Double() / 255.0;
            var r1 = color1.R.Double() / 255.0;
            var g1 = color1.G.Double() / 255.0;
            var b1 = color1.B.Double() / 255.0;

            var a2 = color2.A.Double() / 255.0;
            var r2 = color2.R.Double() / 255.0;
            var g2 = color2.G.Double() / 255.0;
            var b2 = color2.B.Double() / 255.0;

            var a = 1.0 - (1.0 - a2) * (1.0 - a1);
            var r = r2 * a2 / a + r1 * a1 * (1.0 - a2) / a;
            var g = g2 * a2 / a + g1 * a1 * (1.0 - a2) / a;
            var b = b2 * a2 / a + b1 * a1 * (1.0 - a2) / a;

            a *= 255.0;
            r *= 255.0;
            g *= 255.0;
            b *= 255.0;

            return Color.FromArgb(a.Byte(), r.Byte(), g.Byte(), b.Byte());
        }

        /// <summary>
        /// Gets distance to a color from given color.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static double DistanceFrom(this Color First, Color Second)
        {
            return System.Math.Sqrt(System.Math.Pow(First.R - Second.R, 2) + System.Math.Pow(First.G - Second.G, 2) + System.Math.Pow(First.B - Second.B, 2));
        }

        public static double GetHue(this Color color) => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B).GetHue();

        public static System.Drawing.Color Int32(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Hexadecimal Hexadecimal(this Color color, bool alpha = true)
            => new Hexadecimal(color.R, color.G, color.B, alpha ? color.A : (byte)255);

        public static Color Swap(this Color input, ComponentSwap colorSwap)
        {
            switch (colorSwap)
            {
                case ComponentSwap.BGR:
                    return Color.FromArgb(input.A, input.B, input.G, input.R);
                case ComponentSwap.BRG:
                    return Color.FromArgb(input.A, input.B, input.R, input.G);
                case ComponentSwap.GBR:
                    return Color.FromArgb(input.A, input.G, input.B, input.R);
                case ComponentSwap.GRB:
                    return Color.FromArgb(input.A, input.G, input.R, input.B);
                case ComponentSwap.RBG:
                    return Color.FromArgb(input.A, input.R, input.B, input.G);
            }
            return input;
        }

        #endregion
    }
}