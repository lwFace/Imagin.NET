using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class BalanceEffect : AdjustmentEffect
    {
        public override string Name => "Balance";

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(BalanceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(BalanceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(BalanceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        public static readonly DependencyProperty RangeProperty = DependencyProperty.Register("Range", typeof(double), typeof(BalanceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        [Hidden(false)]
        [Range(0.0, 2.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Range
        {
            get => (double)GetValue(RangeProperty);
            set => SetValue(RangeProperty, value);
        }

        public BalanceEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(BalanceEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(RedProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(BlueProperty);
            UpdateShaderValue(RangeProperty);
        }

        public BalanceEffect(double red, double green, double blue) : this()
        {
            SetCurrentValue(RedProperty, red);
            SetCurrentValue(GreenProperty, green);
            SetCurrentValue(BlueProperty, blue);
        }

        public override AdjustmentEffect Copy()
        {
            return new BalanceEffect()
            {
                Red = Red,
                Green = Green,
                Blue = Blue,
                Range = Range
            };
        }
    }
}