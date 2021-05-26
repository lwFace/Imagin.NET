using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public sealed class YIQ : VisualModel
    {
        public override Vector Maximum => Vector.New(1, 0.5957, 0.5226);

        public override Vector Minimum => Vector.New(0, -0.5957, -0.5226);

        public YIQ() : base(new Y(), new I(), new Q()) { }

        public override VisualModels Name => VisualModels.YIQ;

        public override IColor Convert(Color<RGB> input)
        {
            var y = (input[0] * 0.299) + (input[1] * 0.587) + (input[2] * 0.114);

            double i = 0, q = 0;
            if (input[0] != input[1] || input[1] != input[2])
            {
                i = (input[0] * 0.596) + (input[1] * -0.275) + (input[2] * -0.321);
                q = (input[0] * 0.212) + (input[1] * -0.528) + (input[2] * 0.311);
            }

            return new Color<YIQ>(y, i, q);
        }

        public override Color<RGB> Convert(Vector input)
        {
            double r, g, b;

            r = (input[0] * 1.0) + (input[1] * 0.956) + (input[2] * 0.621);
            g = (input[0] * 1.0) + (input[1] * -0.272) + (input[2] * -0.647);
            b = (input[0] * 1.0) + (input[1] * -1.108) + (input[2] * 1.705);

            r = System.Math.Min(System.Math.Max(0, r), 1);
            g = System.Math.Min(System.Math.Max(0, g), 1);
            b = System.Math.Min(System.Math.Max(0, b), 1);

            return new Color<RGB>(new Vector(r, g, b).Coerce(0, 1));
        }

        public abstract class Component : VisualComponent<YIQ>
        {
            public sealed override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i, i => i.Round(2));

            public sealed override double Increment => 0.01;
        }

        public sealed class Y : VisualComponent<YIQ>, IComponentA
        {
            public override string Label => "Y";
        }

        public sealed class I : VisualComponent<YIQ>, IComponentB
        {
            public override string Label => "I";
        }

        public sealed class Q : VisualComponent<YIQ>, IComponentC
        {
            public override string Label => "Q";
        }
    }
}
