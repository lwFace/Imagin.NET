using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// CIE XYZ (1931)
    /// </summary>
    public sealed class XYZ : LogicalModel
    {
        public override Vector Maximum => Vector.New(0.95047, 1, 1.08883);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public override LogicalModels Name => LogicalModels.XYZ;

        public XYZ() : base(new X(), new Y(), new Z()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToXYZ(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<XYZ>(result.X, result.Y, result.Z);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.XYZColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        public sealed class X : VisualComponent<XYZ>, IComponentA
        {
            public override string Label => "X";
        }

        public sealed class Y : VisualComponent<XYZ>, IComponentB
        {
            public override string Label => "Y";
        }

        public sealed class Z : VisualComponent<XYZ>, IComponentC
        {
            public override string Label => "Z";
        }
    }
}
