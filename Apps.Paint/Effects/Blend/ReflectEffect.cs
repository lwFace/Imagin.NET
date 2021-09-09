using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class ReflectEffect : BlendEffect
    {
        static PixelShader pixelShader = new PixelShader();

        static ReflectEffect()
		{
            pixelShader.UriSource = Resource(nameof(ReflectEffect));
        }

        public ReflectEffect()
		{
            PixelShader = pixelShader;
        }
	}
}
