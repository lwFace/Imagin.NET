using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class ExclusionEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static ExclusionEffect()
		{
            pixelShader.UriSource = Resource(nameof(ExclusionEffect));
        }

        public ExclusionEffect()
		{
            PixelShader = pixelShader;
        }
    }
}