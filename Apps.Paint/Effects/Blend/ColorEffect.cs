using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class ColorEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static ColorEffect()
		{
            pixelShader.UriSource = Resource(nameof(ColorEffect));
        }

        public ColorEffect()
		{
            PixelShader = pixelShader;
        }
	}
}