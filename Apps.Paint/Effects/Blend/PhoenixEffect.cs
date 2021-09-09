using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class PhoenixEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static PhoenixEffect()
		{
            pixelShader.UriSource = Resource(nameof(PhoenixEffect));
        }

        public PhoenixEffect()
		{
            PixelShader = pixelShader;
        }
    }
}