using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class LinearBurnEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static LinearBurnEffect()
		{
            pixelShader.UriSource = Resource(nameof(LinearBurnEffect));
        }

        public LinearBurnEffect()
		{
            PixelShader = pixelShader;
        }
	}
}
