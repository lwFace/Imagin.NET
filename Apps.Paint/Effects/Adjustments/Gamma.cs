using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class GammaEffect : AdjustmentEffect
    {
        public override string Name => "Gamma";

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(GammaEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));

        public GammaEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(GammaEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(ValueProperty);
        }

        public GammaEffect(double value) : this()
        {
            SetCurrentValue(ValueProperty, value);
        }

        [Hidden(false)]
        [Range(0.2, 5.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public override AdjustmentEffect Copy()
        {
            return new GammaEffect()
            {
                Value = Value
            };
        }
    }
}