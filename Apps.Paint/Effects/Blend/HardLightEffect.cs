using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class HardLightEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static HardLightEffect()
		{
            pixelShader.UriSource = Resource(nameof(HardLightEffect));
        }

        public HardLightEffect()
		{
            PixelShader = pixelShader;
        }
    }
}