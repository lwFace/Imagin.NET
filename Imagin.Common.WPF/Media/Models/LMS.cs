using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="LMS"/> (each component corresponds to one of the three types of cones in the human eye).
    /// </summary>
    public sealed class LMS : LogicalModel
    {
        public override Vector Maximum => Vector.New(0.941428535, 1.040417467, 1.089532651);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public override LogicalModels Name => LogicalModels.LMS;

        public LMS() : base(new L(), new M(), new S()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToLMS(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<XYZ>(result.L, result.M, result.S);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.LMSColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        public sealed class L : VisualComponent<LMS>, IComponentA
        {
            public override string Label => "L";
        }

        public sealed class M : VisualComponent<LMS>, IComponentB
        {
            public override string Label => "M";
        }

        public sealed class S : VisualComponent<LMS>, IComponentC
        {
            public override string Label => "S";
        }
    }
}