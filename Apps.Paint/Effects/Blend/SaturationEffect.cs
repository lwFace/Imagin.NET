using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class SaturationEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static SaturationEffect()
		{
            pixelShader.UriSource = Resource(nameof(SaturationEffect));
        }

        public SaturationEffect()
		{
            PixelShader = pixelShader;
        }

        private static PixelShader _pixelShader = new PixelShader();
	}
}