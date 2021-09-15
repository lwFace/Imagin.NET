using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="CIE L*a*b* (1976)"/>.
    /// </summary>
    public sealed class Lab : LogicalModel
    {
        public override Vector Maximum => Vector.New(100, 93.5500223896542, 93.3884709589055);

        public override Vector Minimum => Vector.New(0, -79.2872792009571, -112.029413049155);

        public override LogicalModels Name => LogicalModels.Lab;

        public Lab() : base(new L(), new A(), new B()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToLab(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<Lab>(result.L, result.a, result.b);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.LabColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        /// ------------------------------------------------------------------------------------

        public sealed class L : VisualComponent<Lab>, IComponentA
        {
            public override string Label => "L";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class A : VisualComponent<Lab>, IComponentB
        {
            public override string Label => "a";
        }

        public sealed class B : VisualComponent<Lab>, IComponentC
        {
            public override string Label => "b";
        }
    }
}
