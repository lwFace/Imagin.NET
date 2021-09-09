using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class MultiplyEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static MultiplyEffect()
		{
            pixelShader.UriSource = Resource(nameof(MultiplyEffect));
        }

        public MultiplyEffect()
		{
            PixelShader = pixelShader;
        }
	}
}