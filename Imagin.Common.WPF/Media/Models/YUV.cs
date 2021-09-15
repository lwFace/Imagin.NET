using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public sealed class YUV : VisualModel
    {
        public override Vector Maximum => Vector.New(1, .436, .615);

        public override Vector Minimum => Vector.New(0, -.436, -.615);

        public YUV() : base(new Y(), new U(), new V()) { }

        public override VisualModels Name => VisualModels.YUV;

        public override IColor Convert(Color<RGB> input)
        {
            var y = 0.299   * input[0] + 0.587  * input[1] + 0.114 * input[2];
            var u = -0.147  * input[0] - 0.289  * input[1] + 0.436 * input[2];
            var v = 0.615   * input[0] - 0.515  * input[1] - 0.100 * input[2];
            return new Color<YUV>(y, u, v);
        }

        public override Color<RGB> Convert(Vector input)
        {
            var Y = input[0];
            var u = input[1];
            var v = input[2];

            var R = Y + 1.140 * v;
            var G = Y - 0.395 * u - 0.581 * v;
            var B = Y + 2.032 * u;

            return new Color<RGB>(new Vector(R, G, B).Coerce(0, 1));
        }

        public abstract class Component : VisualComponent<YUV>
        {
            public sealed override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i, i => i.Round(2));

            public sealed override double Increment => 0.01;
        }

        public sealed class Y : VisualComponent<YUV>, IComponentA
        {
            public override string Label => "Y";
        }

        public sealed class U : VisualComponent<YUV>, IComponentB
        {
            public override string Label => "U";
        }

        public sealed class V : VisualComponent<YUV>, IComponentC
        {
            public override string Label => "V";
        }
    }
}
