using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class OverlayEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static OverlayEffect()
		{
            pixelShader.UriSource = Resource(nameof(OverlayEffect));
        }

        public OverlayEffect()
		{
			PixelShader = pixelShader;
		}
	}
}