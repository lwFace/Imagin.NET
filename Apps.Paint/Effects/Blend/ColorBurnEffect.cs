using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class ColorBurnEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static ColorBurnEffect()
		{
            pixelShader.UriSource = Resource(nameof(ColorBurnEffect));
        }

        public ColorBurnEffect()
		{
            PixelShader = pixelShader;
        }
	}
}