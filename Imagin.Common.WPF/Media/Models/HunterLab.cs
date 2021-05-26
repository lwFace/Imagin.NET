using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    /// <summary>
    /// Represents a color in <see langword="Hunter Lab"/> (Lightness/a/b).
    /// </summary>
    public sealed class HunterLab : LogicalModel
    {
        public override Vector Maximum => Vector.New(100, 108.405957448357, 58.2087459930753);

        public override Vector Minimum => Vector.New(0, -69.4486345075232, -202.351230669589);

        public override LogicalModels Name => LogicalModels.HunterLab;

        public HunterLab() : base(new L(), new A(), new B()) { }

        public override IColor Convert(Color<RGB> input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToHunterLab(new Colourful.RGBColor(input[0], input[1], input[2]));
            return new Color<HunterLab>(result.L, result.a, result.b);
        }

        public override Color<RGB> Convert(Vector input)
        {
            Colourful.Conversion.ColourfulConverter converter = new Colourful.Conversion.ColourfulConverter();
            var result = converter.ToRGB(new Colourful.HunterLabColor(input[0], input[1], input[2]));
            return new Color<RGB>(result.R, result.G, result.B);
        }

        public sealed class L : VisualComponent<HunterLab>, IComponentA
        {
            public override string Label => "L";

            public override ComponentUnit Unit => ComponentUnit.Percent;
        }

        public sealed class A : VisualComponent<HunterLab>, IComponentB
        {
            public override string Label => "a";
        }

        public sealed class B : VisualComponent<HunterLab>, IComponentC
        {
            public override string Label => "b";
        }
    }
}
