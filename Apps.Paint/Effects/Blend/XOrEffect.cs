using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class XOrEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static XOrEffect()
		{
			pixelShader.UriSource = Resource(nameof(XOrEffect));
		}

		public XOrEffect()
		{
			PixelShader = pixelShader;
		}
	}
}