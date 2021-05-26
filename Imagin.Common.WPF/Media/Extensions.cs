using Imagin.Common.Linq;
using Imagin.Common.Media.Models;
using System;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    public static class Extensions
    {
        public static Color<RGB> Convert(this Color input) => new Color<RGB>(input.R.Double() / Byte.MaxValue.Double(), input.G.Double() / Byte.MaxValue.Double(), input.B.Double() / Byte.MaxValue.Double());

        public static Color Convert(this Color<RGB> input)
        {
            var result = input.Value.Transform(i => i.Multiply(Byte.MaxValue.Double()).Round().Coerce(Byte.MaxValue).Byte());
            return Color.FromArgb(Byte.MaxValue, result[0], result[1], result[2]);
        }
    }
}