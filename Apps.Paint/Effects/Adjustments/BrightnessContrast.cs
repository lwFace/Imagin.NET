using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class BrightnessContrastEffect : AdjustmentEffect
    {
        public override string Name => "Brightness/contrast";

        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(BrightnessContrastEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(-255.0, 255.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }

        public static readonly DependencyProperty ContrastProperty = DependencyProperty.Register("Contrast", typeof(double), typeof(BrightnessContrastEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(-128.0, 128.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Contrast
        {
            get => (double)GetValue(ContrastProperty);
            set => SetValue(ContrastProperty, value);
        }

        public BrightnessContrastEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(BrightnessContrastEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(BrightnessProperty);
            UpdateShaderValue(ContrastProperty);
        }

        public BrightnessContrastEffect(double brightness, double contrast) : this()
        {
            SetCurrentValue(BrightnessProperty, brightness);
            SetCurrentValue(ContrastProperty, contrast);
        }

        public override AdjustmentEffect Copy()
        {
            return new BrightnessContrastEffect()
            {
                Brightness = Brightness,
                Contrast = Contrast
            };
        }
    }
}