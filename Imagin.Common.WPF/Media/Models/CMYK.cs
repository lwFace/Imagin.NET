using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public sealed class CMYK : LogicalModel
    {
        public override Vector Maximum => Vector.New(1, 1, 1, 1);

        public override Vector Minimum => Vector.New(0, 0, 0, 0);

        public override LogicalModels Name => LogicalModels.CMYK;

        public CMYK() : base(new C(), new M(), new Y(), new K()) { }

        public override IColor Convert(Color<RGB> input)
        {
            var k0 = 1.0 - System.Math.Max(input[0], System.Math.Max(input[1], input[2]));
            var k1 = 1.0 - k0;

            var c = (1.0 - input[0] - k0) / k1;
            var m = (1.0 - input[1] - k0) / k1;
            var y = (1.0 - input[2] - k0) / k1;

            c = double.IsNaN(c) ? 0 : c;
            m = double.IsNaN(m) ? 0 : m;
            y = double.IsNaN(y) ? 0 : y;

            return new Color<CMYK>(c, m, y, k0);
        }

        public override Color<RGB> Convert(Vector input)
        {
            var r = (1.0 - input[0]) * (1.0 - input[3]);
            var g = (1.0 - input[1]) * (1.0 - input[3]);
            var b = (1.0 - input[2]) * (1.0 - input[3]);
            return new Color<RGB>(r, g, b);
        }

        public abstract class Component : Models.Component
        {
            public sealed override DoubleConverter DisplayValueConverter => new DoubleConverter(i => i.Shift(-2), i => i.Shift(2).Round());

            public sealed override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class C : Component, IComponentA
        {
            public override string Label => nameof(C);

            public override string ToString() => "Cyan";
        }

        public sealed class M : Component, IComponentB
        {
            public override string Label => nameof(M);

            public override string ToString() => "Magenta";
        }

        public sealed class Y : Component, IComponentC
        {
            public override string Label => nameof(Y);

            public override string ToString() => "Yellow";
        }

        public sealed class K : Component, IComponentD
        {
            public override string Label => nameof(K);

            public override string ToString() => "Black";
        }
    }
}
