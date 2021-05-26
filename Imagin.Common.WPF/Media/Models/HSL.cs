using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="HSL"/> (hue/saturation/lightness).
    /// </summary>
    public sealed class HSL : VisualModel
    {
        public override Vector Maximum => Vector.New(359, 1, 1);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public HSL() : base(new H(), new S(), new L()) { }

        public override VisualModels Name => VisualModels.HSL;

        public override IColor Convert(Color<RGB> input) => From(input);

        public override Color<RGB> Convert(Vector input) => From(new Color<HSL>(input));

        public static Color<HSL> From(Color<RGB> input)
        {
            var maximum = System.Math.Max(System.Math.Max(input.Value[0], input.Value[1]), input.Value[2]);
            var minimum = System.Math.Min(System.Math.Min(input.Value[0], input.Value[1]), input.Value[2]);

            var chroma = maximum - minimum;

            double h = 0, s = 0, l = (maximum + minimum) / 2.0;

            if (chroma != 0)
            {
                s
                    = l < 0.5
                    ? chroma / (2.0 * l)
                    : chroma / (2.0 - 2.0 * l);

                if (input.Value[0] == maximum)
                {
                    h = (input.Value[1] - input.Value[2]) / chroma;
                    h = input.Value[1] < input.Value[2]
                    ? h + 6.0
                    : h;
                }
                else if (input.Value[2] == maximum)
                {
                    h = 4.0 + ((input.Value[0] - input.Value[1]) / chroma);
                }
                else if (input.Value[1] == maximum)
                    h = 2.0 + ((input.Value[2] - input.Value[0]) / chroma);

                h *= 60;
            }

            return new Color<HSL>(h, s, l);
        }

        public static Color<RGB> From(Color<HSL> input)
        {
            double h = input[0] / 60.0, s = input[1], l = input[2];

            double r = l, g = l, b = l;

            if (s > 0)
            {
                var chroma = (1.0 - (2.0 * l - 1.0).Absolute()) * s;
                var x = chroma * (1.0 - ((h % 2.0) - 1).Absolute());

                var result = new Vector(0.0, 0, 0);

                if (0 <= h && h <= 1)
                {
                    result = new Vector(chroma, x, 0);
                }
                else if (1 <= h && h <= 2)
                {
                    result = new Vector(x, chroma, 0);
                }
                else if (2 <= h && h <= 3)
                {
                    result = new Vector(0.0, chroma, x);
                }
                else if (3 <= h && h <= 4)
                {
                    result = new Vector(0.0, x, chroma);
                }
                else if (4 <= h && h <= 5)
                {
                    result = new Vector(x, 0, chroma);
                }
                else if (5 <= h && h <= 6)
                    result = new Vector(chroma, 0, x);

                var m = l - (0.5 * chroma);

                r = result[0] + m;
                g = result[1] + m;
                b = result[2] + m;
            }

            return new Color<RGB>(r, g, b);
        }

        public sealed class H : VisualComponent<HSL>, IComponentA
        {
            public override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i, i => i.Round());

            public override ComponentType Type => ComponentType.Static;

            public override string Label => "H";

            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        public sealed class S : VisualComponent<HSL>, IComponentB
        {
            public override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i /= Maximum.Shift(2), i => (i *= Maximum.Shift(2)).Round());

            public override double Increment => 0.01;

            public override string Label => "S";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class L : VisualComponent<HSL>, IComponentC
        {
            public override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i /= Maximum.Shift(2), i => (i *= Maximum.Shift(2)).Round());

            public override double Increment => 0.01;

            public override string Label => "L";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}