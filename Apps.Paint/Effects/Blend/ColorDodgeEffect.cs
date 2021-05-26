using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class ColorDodgeEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static ColorDodgeEffect()
		{
			pixelShader.UriSource = Resource(nameof(ColorDodgeEffect));
		}

		public ColorDodgeEffect()
		{
			PixelShader = pixelShader;
		}
	}
}