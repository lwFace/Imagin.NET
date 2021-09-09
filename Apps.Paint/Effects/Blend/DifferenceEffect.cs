using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class DifferenceEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static DifferenceEffect()
		{
            pixelShader.UriSource = Resource(nameof(DifferenceEffect));
        }

        public DifferenceEffect()
		{
            PixelShader = pixelShader;
        }
    }
}