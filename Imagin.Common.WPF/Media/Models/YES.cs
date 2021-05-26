using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public sealed class YES : VisualModel
    {
        public override Vector Maximum => Vector.New(1, 1, 1);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public YES() : base(new Y(), new E(), new S()) { }

        public override VisualModels Name => VisualModels.YES;

        public override IColor Convert(Color<RGB> input)
        {
            return new Color<YES>
            (
                input[0] * .253 + input[1] * .684   + input[2] * .063,
                input[0] * .500 + input[1] * -.500  + input[2] * .000,
                input[0] * .250 + input[1] * .250   + input[2] * -.500
            );
        }

        public override Color<RGB> Convert(Vector input)
        {
            var r = input[0] * 1 + input[1] * 1.431 + input[2] * 0.126;
            var g = input[0] * 1 + input[1] * -0.569 + input[2] * 0.126;
            var b = input[0] * 1 + input[1] * 0.431 + input[2] * -1.874;
            return new Color<RGB>(new Vector(r, g, b).Coerce(0, 1));
        }

        public sealed class Y : VisualComponent<YES>, IComponentA
        {
            public override double Increment => 0.01;

            public override string Label => "Y";
        }

        public sealed class E : VisualComponent<YES>, IComponentB
        {
            public override double Increment => 0.01;

            public override string Label => "E";
        }

        public sealed class S : VisualComponent<YES>, IComponentC
        {
            public override double Increment => 0.01;

            public override string Label => "S";
        }
    }
}
