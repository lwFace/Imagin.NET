using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="CIE L*C*h°"/> (the cylindrical form of <see langword="CIE L*a*b* (1976)"/>).
    /// </summary>
    public sealed class LChab : LogicalModel
    {
        public override Vector Maximum => Vector.New(100, 131.207048717117, 359.99995699368);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public override LogicalModels Name => LogicalModels.LChab;

        public LChab() : base(new L(), new C(), new H()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToLChab(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<LChab>(result.L, result.C, result.h);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.LChabColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        public sealed class L : VisualComponent<LChab>, IComponentA
        {
            public override string Label => "L";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class C : VisualComponent<LChab>, IComponentB
        {
            public override string Label => "C";
        }

        public sealed class H : VisualComponent<LChab>, IComponentC
        {
            public override string Label => "h";

            public override ComponentUnit Unit => ComponentUnit.Degrees;
        }
    }
}
