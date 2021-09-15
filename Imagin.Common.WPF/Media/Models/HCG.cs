using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public sealed class HCG : VisualModel
    {
        public override Vector Maximum => Vector.New(359, 100, 100);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public HCG() : base(new H(), new C(), new G()) { }

        public override VisualModels Name => VisualModels.HCG;

        public override IColor Convert(Color<RGB> input)
        {
            double r = input.Value[0], g = input.Value[1], b = input.Value[2];

            var maximum = System.Math.Max(System.Math.Max(r, g), b);
            var minimum = System.Math.Min(System.Math.Min(r, g), b);

            var chroma = maximum - minimum;
            double grayscale = 0;
            double hue;

            if (chroma < 1)
                grayscale = minimum / (1.0 - chroma);

            if (chroma > 0)
            {
                if (maximum == r)
                {
                    hue = ((g - b) / chroma) % 6;
                }
                else if (maximum == g)
                {
                    hue = 2 + (b - r) / chroma;
                }
                else hue = 4 + (r - g) / chroma;

                hue /= 6;
                hue = hue % 1;
            }
            else hue = 0;

            return new Color<HCG>(hue * 360.0, chroma * 100.0, grayscale * 100.0);
        }

        public override Color<RGB> Convert(Vector input)
        {
            double h = input[0] / 360.0, c = input[1] / 100.0, g = input[2] / 100.0;

            if (c == 0)
                return new Color<RGB>(g, g, g);

            var hi = (h % 1.0) * 6.0;
            var v = hi % 1.0;
            var pure = new double[3];
            var w = 1.0 - v;

            switch (System.Math.Floor(hi))
            {
                case 0:
                    pure[0] = 1; pure[1] = v; pure[2] = 0; break;
                case 1:
                    pure[0] = w; pure[1] = 1; pure[2] = 0; break;
                case 2:
                    pure[0] = 0; pure[1] = 1; pure[2] = v; break;
                case 3:
                    pure[0] = 0; pure[1] = w; pure[2] = 1; break;
                case 4:
                    pure[0] = v; pure[1] = 0; pure[2] = 1; break;
                default:
                    pure[0] = 1; pure[1] = 0; pure[2] = w; break;
            }

            var mg = (1.0 - c) * g;

            return new Color<RGB>
            (
                c * pure[0] + mg,
                c * pure[1] + mg,
                c * pure[2] + mg
            );
        }

        public abstract class Component : VisualComponent<HCG>
        {
            public sealed override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i, i => i.Round());
        }

        public sealed class H : Component, IComponentA
        {
            public override ComponentType Type => ComponentType.Static;

            public override string Label => "H";

            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        public sealed class C : Component, IComponentB
        {
            public override string Label => "C";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class G : Component, IComponentC
        {
            public override string Label => "G";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}