using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class VividLightEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static VividLightEffect()
		{
            pixelShader.UriSource = Resource(nameof(VividLightEffect));
        }

        public VividLightEffect()
		{
			PixelShader = pixelShader;
		}
	}
}