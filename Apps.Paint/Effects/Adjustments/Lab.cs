using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class LabEffect : AdjustmentEffect
    {
        public override string Name => "Lab";

        public static readonly DependencyProperty LProperty = DependencyProperty.Register("L", typeof(double), typeof(LabEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty AProperty = DependencyProperty.Register("A", typeof(double), typeof(LabEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty BProperty = DependencyProperty.Register("B", typeof(double), typeof(LabEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));

        public LabEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(LabEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(LProperty);
            UpdateShaderValue(AProperty);
            UpdateShaderValue(BProperty);
        }

        public LabEffect(double l, double a, double b) : this()
        {
            SetCurrentValue(LProperty, l);
            SetCurrentValue(AProperty, a);
            SetCurrentValue(BProperty, b);
        }

        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double L
        {
            get => (double)GetValue(LProperty);
            set => SetValue(LProperty, value);
        }

        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double A
        {
            get => (double)GetValue(AProperty);
            set => SetValue(AProperty, value);
        }

        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double B
        {
            get => (double)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        public override AdjustmentEffect Copy()
        {
            return new LabEffect()
            {
                L = L,
                A = A,
                B = B
            };
        }
    }
}