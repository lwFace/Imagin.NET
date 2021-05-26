using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class LightenEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static LightenEffect()
		{
            pixelShader.UriSource = Resource(nameof(LightenEffect));
        }

        public LightenEffect()
		{
            PixelShader = pixelShader;
        }
    }
}