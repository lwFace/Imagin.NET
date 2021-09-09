using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class GlowEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static GlowEffect()
		{
            pixelShader.UriSource = Resource(nameof(GlowEffect));
        }

        public GlowEffect()
		{
            PixelShader = pixelShader;
        }
    }
}