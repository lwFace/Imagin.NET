using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="TSL"/> (tint/saturation/lightness).
    /// </summary>
    public sealed class TSL : VisualModel
    {
        public override Vector Maximum => Vector.New(1, 1, 1);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public TSL() : base(new T(), new S(), new L()) { }

        public override VisualModels Name => VisualModels.TSL;

        public override IColor Convert(Color<RGB> input)
        {
            double r = input.Value[0], g = input.Value[1], b = input.Value[2];
            double rgb = r + g + b;

            double r_ = (r / rgb) - 1.0 / 3.0, 
                   g_ = (g / rgb) - 1.0 / 3.0;

            var T = .0;

            if (g_ > 0)
            {
                T = .5 * Numbers.PI * System.Math.Atan(r_ / g_) + .25;
            }
            else if (g_ < 0)
            {
                T = .5 * Numbers.PI * System.Math.Atan(r_ / g_) + .75;
            }

            var S = System.Math.Sqrt(9.0 / 5.0 * (r_ * r_ + g_ * g_));

            var L = (r * .299) + (g * .587) + (b * .114);

            return new Color<TSL>(T, S, L);
        }

        public override Color<RGB> Convert(Vector input)
        {
            double T = input[0], S = input[1], L = input[2];

            var x = -(1.0 / System.Math.Tan(2.0 * Numbers.PI * T));

            var r_ = .0;
            var g_ = .0;

            if (T > .5)
            {
                g_ = -System.Math.Sqrt(5.0 / (9.0 * (x.Power(2) + 1.0)));
            }
            else if (T < .5)
            {
                g_ = System.Math.Sqrt(5.0 / (9.0 * (x.Power(2) + 1.0)));
            }
            else if (T == 0)
            {
                g_ = 0;
            }

            if (T == 0)
            {
                r_ = (5.0.SquareRoot() / 3.0 * S).Absolute();
            }
            else
            {
                r_ = x * g_;
            }

            var r = r_ + 1.0 / 3.0;
            var g = g_ + 1.0 / 3.0;

            var k = L / (.185 * r + .473 * g + .114);

            var R = k * r;
            var G = k * g;
            var B = k * (1.0 - r - g);

            return new Color<RGB>(new Vector(R, G, B).Coerce(0, 1));
        }

        public sealed class T : VisualComponent<TSL>, IComponentA
        {
            public sealed override double Increment => 0.01;

            public override string Label => "T";

            public override ComponentType Type => ComponentType.Static;

            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }

        public sealed class S : VisualComponent<TSL>, IComponentB
        {
            public sealed override double Increment => 0.01;

            public override string Label => "S";
            
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
        
        public sealed class L : VisualComponent<TSL>, IComponentC
        {
            public sealed override double Increment => 0.01;

            public override string Label => "L";
            
            public override ComponentUnit Unit => ComponentUnit.Percent;
        }
    }
}