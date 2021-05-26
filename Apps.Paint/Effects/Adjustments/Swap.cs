using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class SwapEffect : AdjustmentEffect
    {
        public override string Name => "Swap";

        public static readonly DependencyProperty ChannelsProperty = DependencyProperty.Register("Channels", typeof(double), typeof(SwapEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public double Channels
        {
            get => (double)GetValue(ChannelsProperty);
            set => SetValue(ChannelsProperty, value);
        }

        public SwapEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(SwapEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(ChannelsProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new SwapEffect()
            {
                Channels = Channels
            };
        }
    }
}