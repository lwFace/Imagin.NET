using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class LinearDodgeEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static LinearDodgeEffect()
		{
            pixelShader.UriSource = Resource(nameof(LinearDodgeEffect));
        }

        public LinearDodgeEffect()
		{
            PixelShader = pixelShader;
        }
	}
}