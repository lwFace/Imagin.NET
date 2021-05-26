using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class HardMixEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static HardMixEffect()
		{
            pixelShader.UriSource = Resource(nameof(HardMixEffect));
        }

        public HardMixEffect()
		{
            PixelShader = pixelShader;
        }
    }
}