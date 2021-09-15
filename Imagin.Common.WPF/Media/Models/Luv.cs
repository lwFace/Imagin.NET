using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="CIE L*u*v* (1976)"/>
    /// </summary>
    public sealed class Luv : LogicalModel
    {
        public override Vector Maximum => Vector.New(100, 175.01510209029, 107.398532986784);

        public override Vector Minimum => Vector.New(0, -83.0775614430081, -134.103003220307);

        public override LogicalModels Name => LogicalModels.Luv;

        public Luv() : base(new L(), new U(), new V()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToLuv(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<Luv>(result.L, result.u, result.v);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.LuvColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        /// ------------------------------------------------------------------------------------

        public sealed class L : VisualComponent<Luv>, IComponentA
        {
            public override string Label => "L";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class U : VisualComponent<Luv>, IComponentB
        {
            public override string Label => "u";
        }

        public sealed class V : VisualComponent<Luv>, IComponentC
        {
            public override string Label => "v";
        }
    }
}
