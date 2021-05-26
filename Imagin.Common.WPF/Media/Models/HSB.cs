using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Hue/Saturation/Brightness
    /// </summary>
    public sealed class HSB : VisualModel
    {
        public override Vector Maximum => Vector.New(359, 100, 100);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public HSB() : base(new H(), new S(), new B()) { }

        public override VisualModels Name => VisualModels.HSB;

        public override IColor Convert(Color<RGB> input) => From(input);

        public override Color<RGB> Convert(Vector input) => From(new Color<HSB>(input));

        public static Color<RGB> From(Color<HSB> input)
        {
            var _color = new HSB().Normalize(input);
            double _h = _color.X, _s = _color.Y, _b = _color.Z;

            double r = 0, g = 0, b = 0;

            if (_s == 0)
            {
                r = g = b = _b;
            }
            else
            {
                _h *= new HSB().Maximum[0];

                //The color wheel consists of 6 sectors: Figure out which sector we're in...
                var SectorPosition = _h / 60.0;
                var SectorNumber = System.Math.Floor(SectorPosition).Int32();

                //Get the fractional part of the sector
                var FractionalSector = SectorPosition - SectorNumber;

                //Calculate values for the three axes of the color. 
                var p = _b * (1.0 - _s);
                var q = _b * (1.0 - (_s * FractionalSector));
                var t = _b * (1.0 - (_s * (1.0 - FractionalSector)));

                //Assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (SectorNumber)
                {
                    case 0:
                        r = _b;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = _b;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = _b;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = _b;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = _b;
                        break;
                    case 5:
                        r = _b;
                        g = p;
                        b = q;
                        break;
                }
            }
            return new Color<RGB>(r, g, b);
        }

        public static Color<HSB> From(Color<RGB> input)
        {
            double r = input.Value[0], g = input.Value[1], b = input.Value[2];

            var minimum = System.Math.Min(input.Value[0], System.Math.Min(input.Value[1], input.Value[2]));
            var maximum = System.Math.Max(input.Value[0], System.Math.Max(input.Value[1], input.Value[2]));

            var chroma = maximum - minimum;

            var _h = 0.0;
            var _s = 0.0;
            var _b = maximum;

            if (chroma == 0)
            {
                _h = 0;
                _s = 0;
            }
            else
            {
                _s = chroma / maximum;

                if (input.Value[0] == maximum)
                {
                    _h = (input.Value[1] - input.Value[2]) / chroma;
                    _h = input.Value[1] < input.Value[2] ? _h + 6 : _h;
                }
                else if (input.Value[1] == maximum)
                {
                    _h = 2.0 + ((input.Value[2] - input.Value[0]) / chroma);
                }
                else if (input.Value[2] == maximum)
                    _h = 4.0 + ((input.Value[0] - input.Value[1]) / chroma);

                _h *= 60;
            }

            return new Color<HSB>(_h, _s.Shift(2), _b.Shift(2));
        }

        public abstract class Component : VisualComponent<HSB>
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

        public sealed class B : Component, IComponentC
        {
            public override string Label => "B";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}