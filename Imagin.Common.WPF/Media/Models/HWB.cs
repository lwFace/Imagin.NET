using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="HWB"/> (Hue/Whiteness/Blackness).
    /// </summary>
    public sealed class HWB : VisualModel
    {
        public override Vector Maximum => Vector.New(359, 100, 100);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public HWB() : base(new H(), new W(), new B()) { }

        public override VisualModels Name => VisualModels.HWB;

        public override IColor Convert(Color<RGB> input)
        {
            double r = input.Value[0], g = input.Value[1], b = input.Value[2];

            var _h = 0; //Get HSL.Hue of input
            var _w = 1 / 255 * System.Math.Min(r, System.Math.Min(g, b));
            var _b = 1 - 1 / 255 * System.Math.Max(r, System.Math.Max(g, b));

            return new Color<HWB>(_h, _w * 100, _b * 100);
        }

        public override Color<RGB> Convert(Vector input)
        {
            double h = input[0] / 360, wh = input[1] / 100, bl = input[2] / 100;

            var ratio = wh + bl;

            int i;
            double v, f, n;
            double r, g, b;

            //wh + bl cant be > 1
            if (ratio > 1)
            {
                wh /= ratio;
                bl /= ratio;
            }

            i = System.Math.Floor(6 * h).Int32();
            v = 1 - bl;
            f = 6 * h - i;

            //If it is even...
            if ((i & 0x01) != 0)
                f = 1 - f;

            //Linear interpolation
            n = wh + f * (v - wh);

            switch (i)
            {
                default:
                case 6:
                case 0: r = v; g = n; b = wh; break;
                case 1: r = n; g = v; b = wh; break;
                case 2: r = wh; g = v; b = n; break;
                case 3: r = wh; g = n; b = v; break;
                case 4: r = n; g = wh; b = v; break;
                case 5: r = v; g = wh; b = n; break;
            }

            return new Color<RGB>(r, g, b);
        }

        public sealed class H : VisualComponent<HWB>, IComponentA
        {
            public override ComponentType Type => ComponentType.Static;

            public override string Label => "H";

            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        public sealed class W : VisualComponent<HWB>, IComponentB
        {
            public override string Label => "W";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class B : VisualComponent<HWB>, IComponentC
        {
            public override string Label => "B";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}