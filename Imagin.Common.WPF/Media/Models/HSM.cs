using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="HSM"/> (Hue/Saturation/Mixture).
    /// </summary>
    /// <remarks>
    /// http://seer.ufrgs.br/rita/article/viewFile/rita_v16_n2_p141/7428
    /// </remarks>
    public sealed class HSM : VisualModel
    {
        public override Vector Maximum => Vector.New(1, 1, 1);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public HSM() : base(new H(), new S(), new M()) { }

        public override VisualModels Name => VisualModels.HSM;

        public override IColor Convert(Color<RGB> input)
        {
            var m = (4.0 * input.Value[0] + 2.0 * input.Value[1] + input.Value[2]) / 7.0;

            var a = 3.0 * (input.Value[0] - m) - 4.0 * (input.Value[1] - m) - 4.0 * (input.Value[2] - m);
            var b = 41.0.SquareRoot();
            var c = a / b;

            var d = (input.Value[0] - m).Power(2) + (input.Value[1] - m).Power(2) + (input.Value[2] - m).Power(2);
            var e = d.SquareRoot();

            var f = c / e;

            var theta = System.Math.Acos(f);
            var w = .0;

            if (input.Value[2] <= input.Value[1])
            {
                w = theta;
            }
            else if (input.Value[2] > input.Value[1])
            {
                w = 2.0 * Numbers.PI - theta;
            }

            var hue = w / (2.0 * Numbers.PI);
            var saturation = .0;
            var mixture = m;

            double q = .0, r = .0, s = .0;

            if (0 <= m && m <= 1.0 / 7.0)
            {
                q = 0.0;
                r = 0.0;
                s = 7.0;
            }
            else if (1.0 / 7.0 <= m && m <= 3.0 / 7.0)
            {
                q = .0;
                r = ((7.0 * m) - 1.0) / 2.0;
                s = 1.0;
            }
            else if (3.0 / 7.0 <= m && m <= 1.0 / 2.0)
            {
                q = ((7.0 * m) - 3.0) / 2.0;
                r = 1.0;
                s = 1.0;
            }
            else if (1.0 / 2.0 <= m && m <= 4.0 / 7.0)
            {
                q = (7.0 * m) / 4.0;
                r = 0.0;
                s = 0.0;
            }
            else if (4.0 / 7.0 <= m && m <= 6.0 / 7.0)
            {
                q = 1.0;
                r = ((7.0 * m) - 4.0) / 2.0;
                s = 0.0;
            }
            else if (6.0 / 7.0 <= m && m <= 1)
            {
                q = 1.0;
                r = 1.0;
                s = (7.0 * m) - 6.0;
            }

            var x = ((input.Value[0] - m).Power(2) + (input.Value[1] - m).Power(2) + (input.Value[2] - m).Power(2)).SquareRoot();
            var y = (q - m).Power(2) + (r - m).Power(2) + (s - m).Power(2);

            saturation = x.SquareRoot() / y.SquareRoot();

            return new Color<HSM>(hue, saturation, mixture);
        }

        public override Color<RGB> Convert(Vector input)
        {
            double x = System.Math.Cos(input[0]);
            double w = 41.0.SquareRoot() * input[1] * x;

            double a = .0, b = .0, c = .0;

            a = 3.0 / 41.0 * input[1] * x;
            b = input[2];
            c = 4.0 / 861.0 * (861.0 * input[1].Power(2) * (1.0 - x.Power(2))).SquareRoot();

            var R = a + b - c;

            a = w;
            b = 23.0 * input[2];
            c = 19.0 * R;

            var G = (a + b - c) / 4.0;

            a = 11 * R;
            b = 9.0 * input[2];
            c = w;

            var B = (a - b - c) / 2.0;

            return new Color<RGB>(new Vector(R, G, B).Coerce(0, 1));
        }

        public sealed class H : VisualComponent<HSM>, IComponentA
        {
            public override double Increment => 0.01;

            public override ComponentType Type => ComponentType.Static;

            public override string Label => "H";
        }

        public sealed class S : VisualComponent<HSM>, IComponentB
        {
            public override double Increment => 0.01;

            public override string Label => "S";
        }

        public sealed class M : VisualComponent<HSM>, IComponentC
        {
            public override double Increment => 0.01;

            public override string Label => "M";
        }
    }
}