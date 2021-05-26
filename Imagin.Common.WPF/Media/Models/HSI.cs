using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="HSI"/> (Hue/Saturation/Intensity).
    /// </summary>
    public sealed class HSI : VisualModel
    {
        public override Vector Maximum => Vector.New(359, 100, 255);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public HSI() : base(new H(), new S(), new I()) { }

        public override VisualModels Name => VisualModels.HSI;

        public override IColor Convert(Color<RGB> input)
        {
            var _input = input.Value * 255;
            var sum = _input.Sum();

            var r = _input[0] / sum;
            var g = _input[1] / sum;
            var b = _input[2] / sum;

            var h = System.Math.Acos
            (
                (0.5 * ((r - g) + (r - b)))
                /
                System.Math.Sqrt
                (
                    (r - g) * (r - g) + (r - b) * (g - b)
                )
            );

            if (b > g)
                h = 2 * Numbers.PI - h;

            var s = 1 - 3 * System.Math.Min(r, System.Math.Min(g, b));
            var i = sum / 3;

            return new Color<HSI>(h * 180 / Numbers.PI, s * 100, i);
        }

        public override Color<RGB> Convert(Vector input)
        {
            var h = input[0].Modulo(0, 360) * Numbers.PI / 180.0;
            var s = input[1].Coerce(100) / 100.0;
            var i = input[2].Coerce(255) / 255.0;

            var pi3 = Numbers.PI / 3;

            double r, g, b;
            if (h < (2 * pi3))
            {
                b = i * (1 - s);
                r = i * (1 + (s * System.Math.Cos(h) / System.Math.Cos(pi3 - h)));
                g = i * (1 + (s * (1 - System.Math.Cos(h) / System.Math.Cos(pi3 - h))));
            }
            else if (h < (4 * pi3))
            {
                h = h - 2 * pi3;
                r = i * (1 - s);
                g = i * (1 + (s * System.Math.Cos(h) / System.Math.Cos(pi3 - h)));
                b = i * (1 + (s * (1 - System.Math.Cos(h) / System.Math.Cos(pi3 - h))));
            }
            else
            {
                h = h - 4 * pi3;
                g = i * (1 - s);
                b = i * (1 + (s * System.Math.Cos(h) / System.Math.Cos(pi3 - h)));
                r = i * (1 + (s * (1 - System.Math.Cos(h) / System.Math.Cos(pi3 - h))));
            }

            return new Color<RGB>(new Vector(r, g, b).Coerce(0, 1));
        }

        public abstract class Component : VisualComponent<HSI>
        {
            public sealed override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i, i => i.Round());
        }

        public sealed class H : Component, IComponentA
        {
            public override ComponentType Type => ComponentType.Static;

            public override string Label => "H";

            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        public sealed class S : Component, IComponentB
        {
            public override string Label => "S";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class I : Component, IComponentC
        {
            public override string Label => "I";

            public override ComponentUnit Unit => ComponentUnit.None;
        }
    }
}