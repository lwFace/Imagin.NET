using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class VibranceEffect : AdjustmentEffect
    {
        public override string Name => "Vibrance";

        public static readonly DependencyProperty VibranceProperty = DependencyProperty.Register("Vibrance", typeof(double), typeof(VibranceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Vibrance
        {
            get => (double)GetValue(VibranceProperty);
            set => SetValue(VibranceProperty, value);
        }

        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register("Threshold", typeof(double), typeof(VibranceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Threshold
        {
            get => (double)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

        public VibranceEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(VibranceEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(VibranceProperty);
            UpdateShaderValue(ThresholdProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new VibranceEffect()
            {
                Vibrance = Vibrance,
                Threshold = Threshold
            };
        }
    }
}