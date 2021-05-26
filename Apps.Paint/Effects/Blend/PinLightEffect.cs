using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class PinLightEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static PinLightEffect()
		{
            pixelShader.UriSource = Resource(nameof(PinLightEffect));
        }

        public PinLightEffect()
		{
            PixelShader = pixelShader;
        }
	}
}