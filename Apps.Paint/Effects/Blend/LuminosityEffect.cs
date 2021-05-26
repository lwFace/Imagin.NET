using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class LuminosityEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static LuminosityEffect()
		{
            pixelShader.UriSource = Resource(nameof(LuminosityEffect));
        }

        public LuminosityEffect()
		{
            PixelShader = pixelShader;
        }
	}
}