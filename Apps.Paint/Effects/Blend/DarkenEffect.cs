using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class DarkenEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static DarkenEffect()
		{
            pixelShader.UriSource = Resource(nameof(DarkenEffect));
        }

        public DarkenEffect()
		{
            PixelShader = pixelShader;
        }
	}
}