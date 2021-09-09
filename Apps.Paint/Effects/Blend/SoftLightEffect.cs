using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class SoftLightEffect : BlendEffect
	{
        static PixelShader pixelShader = new PixelShader();

        static SoftLightEffect()
		{
            pixelShader.UriSource = Resource(nameof(SoftLightEffect));
        }

        public SoftLightEffect()
		{
            PixelShader = pixelShader;
        }
    }
}