using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class ScreenEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static ScreenEffect()
		{
            pixelShader.UriSource = Resource(nameof(ScreenEffect));
        }

        public ScreenEffect()
		{
            PixelShader = pixelShader;
        }
	}
}