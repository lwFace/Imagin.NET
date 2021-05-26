using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class NegationEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static NegationEffect()
		{
            pixelShader.UriSource = Resource(nameof(NegationEffect));
        }

        public NegationEffect()
		{
            PixelShader = pixelShader;
        }
	}
}