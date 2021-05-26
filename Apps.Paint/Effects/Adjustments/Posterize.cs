using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class PosterizeEffect : AdjustmentEffect
    {
        public override string Name => "Posterize";

        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register("Threshold", typeof(double), typeof(PosterizeEffect), new UIPropertyMetadata(6.0, PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(3.0, 64.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Threshold
        {
            get => (double)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

        public PosterizeEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(PosterizeEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(ThresholdProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new PosterizeEffect()
            {
                Threshold = Threshold
            };
        }
    }
}