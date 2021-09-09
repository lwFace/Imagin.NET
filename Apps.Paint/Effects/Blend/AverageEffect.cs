using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class AverageEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static AverageEffect()
		{
            pixelShader.UriSource = Resource(nameof(AverageEffect));
        }

        public AverageEffect()
		{
            PixelShader = pixelShader;
        }
	}
}