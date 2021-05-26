using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class LinearLightEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static LinearLightEffect()
		{
            pixelShader.UriSource = Resource(nameof(LinearLightEffect));
        }

        public LinearLightEffect()
		{
            PixelShader = pixelShader;
        }
	}
}