using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class HSLEffect : AdjustmentEffect
    {
        public override string Name => "HSL";

        public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(double), typeof(HSLEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(-180.0, 180.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(HSLEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(-50.0, 50.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty LightnessProperty = DependencyProperty.Register("Lightness", typeof(double), typeof(HSLEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(-50.0, 50.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Lightness
        {
            get => (double)GetValue(LightnessProperty);
            set => SetValue(LightnessProperty, value);
        }

        public HSLEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(HSLEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(HueProperty);
            UpdateShaderValue(SaturationProperty);
            UpdateShaderValue(LightnessProperty);
        }

        public HSLEffect(double h, double s, double l) : this()
        {
            SetCurrentValue(HueProperty, h);
            SetCurrentValue(SaturationProperty, s);
            SetCurrentValue(LightnessProperty, l);
        }

        public override AdjustmentEffect Copy()
        {
            return new HSLEffect()
            {
                Hue = Hue,
                Saturation = Saturation,
                Lightness = Lightness
            };
        }
    }
}