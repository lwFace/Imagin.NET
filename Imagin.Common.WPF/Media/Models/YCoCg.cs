using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public sealed class YCoCg : VisualModel
    {
        public override Vector Maximum => Vector.New(1, 0.5, 0.5);

        public override Vector Minimum => Vector.New(0, -0.5, -0.5);

        public YCoCg() : base(new Y(), new Co(), new Cg()) { }

        public override VisualModels Name => VisualModels.YCoCg;

        public override IColor Convert(Color<RGB> input)
        {
            return new Color<YCoCg>
            (
                 
                 0.25 * input[0] + 0.5 * input[1] + 0.25 * input[2],
                -0.25 * input[0] + 0.5 * input[1] - 0.25 * input[2],
                 0.5  * input[0] - 0.5 * input[2]
            );
        }

        public override Color<RGB> Convert(Vector input)
        {
            double ycg = input[0] - input[2];
            var r = ycg + input[1];
            var g = input[0] + input[2];
            var b = ycg - input[1];
            return new Color<RGB>(r, g, b);
        }

        public abstract class Component : VisualComponent<YUV>
        {
            public sealed override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i, i => i.Round(2));

            public sealed override double Increment => 0.01;
        }

        public sealed class Y : Component, IComponentA
        {
            public override string Label => "Y";
        }

        public sealed class Co : Component, IComponentB
        {
            public override string Label => "Co";
        }

        public sealed class Cg : Component, IComponentC
        {
            public override string Label => "Cg";
        }
    }
}
