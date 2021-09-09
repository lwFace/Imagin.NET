using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class HueEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static HueEffect()
		{
            pixelShader.UriSource = Resource(nameof(HueEffect));
        }

        public HueEffect()
		{
            PixelShader = pixelShader;
        }
    }
}