using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="CIE L*C*h°"/> (the cylindrical form of <see langword="CIE L*u*v* (1976)"/>).
    /// </summary>
    public sealed class LChuv : LogicalModel
    {
        public override Vector Maximum => Vector.New(100, 179.041427089396, 359.999941373506);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public override LogicalModels Name => LogicalModels.LChuv;

        public LChuv() : base(new L(), new C(), new H()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToLChuv(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<LChuv>(result.L, result.C, result.h);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.LChuvColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        public sealed class L : VisualComponent<LChuv>, IComponentA
        {
            public override string Label => "L";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class C : VisualComponent<LChuv>, IComponentB
        {
            public override string Label => "C";
        }

        public sealed class H : VisualComponent<LChuv>, IComponentC
        {
            public override string Label => "h";

            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }
    }
}
