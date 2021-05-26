using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class DifferenceEffect : AdjustmentEffect
    {
        public override string Name => "Difference";

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(DifferenceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(0.0, 255.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Red
        {
            get
            {
                return ((double)(GetValue(RedProperty)));
            }
            set
            {
                SetValue(RedProperty, value);
            }
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(DifferenceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(0.0, 255.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Green
        {
            get
            {
                return ((double)(GetValue(GreenProperty)));
            }
            set
            {
                SetValue(GreenProperty, value);
            }
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(DifferenceEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(0.0, 255.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Blue
        {
            get
            {
                return ((double)(GetValue(BlueProperty)));
            }
            set
            {
                SetValue(BlueProperty, value);
            }
        }

        public DifferenceEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(DifferenceEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(RedProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(BlueProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new DifferenceEffect()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }
}