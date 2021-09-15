using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// xyY 
    /// </summary>
    public sealed class xyY : LogicalModel
    {
        public override Vector Maximum => Vector.New(0.64, 0.6, 1);

        public override Vector Minimum => Vector.New(0, 0, 0);

        public override LogicalModels Name => LogicalModels.xyY;

        public xyY() : base(new x(), new y(), new Y()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToxyY(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<xyY>(result.x, result.y, result.Luminance);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.xyYColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        public sealed class x : VisualComponent<xyY>, IComponentA
        {
            public override string Label => "x";
        }

        public sealed class y : VisualComponent<xyY>, IComponentB
        {
            public override string Label => "y";
        }

        public sealed class Y : VisualComponent<xyY>, IComponentC
        {
            public override string Label => "Y";
        }
    }
}
