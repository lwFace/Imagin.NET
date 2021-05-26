using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public sealed class RGB : VisualModel
    {
        public override Vector Maximum => new Vector(1, 1, 1);

        public override Vector Minimum => Vector.Zero;

        public RGB() : base(new R(), new G(), new B()) { }

        public override VisualModels Name => VisualModels.RGB;

        ///--------------------------------------------------------------------------------------------------------

        public override IColor Convert(Color<RGB> input) => input;

        public override Color<RGB> Convert(Vector input) => new Color<RGB>(input);

        ///--------------------------------------------------------------------------------------------------------

        /*
        public static Color<RGB> Convert(Color<Linear> input)
        {
            Vector result = new[]
            {
                input.Profile.Compression.Compress(input.Value[0]).Coerce(1),
                input.Profile.Compression.Compress(input.Value[1]).Coerce(1),
                input.Profile.Compression.Compress(input.Value[2]).Coerce(1)
            };
            return new Color<RGB>(result);
        }
        */

        ///--------------------------------------------------------------------------------------------------------

        public abstract class Component : VisualComponent<RGB>
        {
            public override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i /= 255.0, i => (i *= 255.0).Round());

            public override double Increment => 0.01;
        }

        public sealed class R : Component, IComponentA
        {
            public override string Label => nameof(R);
        }

        public sealed class G : Component, IComponentB
        {
            public override string Label => nameof(G);
        }

        public sealed class B : Component, IComponentC
        {
            public override string Label => nameof(B);
        }
    }
}