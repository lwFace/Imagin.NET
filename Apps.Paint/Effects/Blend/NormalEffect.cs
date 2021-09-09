using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class NormalEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static NormalEffect()
		{
            pixelShader.UriSource = Resource(nameof(NormalEffect));
        }

        public NormalEffect()
		{
            PixelShader = pixelShader;
        }
	}
}