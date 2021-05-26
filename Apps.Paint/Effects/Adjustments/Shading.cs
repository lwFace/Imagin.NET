using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class ShadingEffect : AdjustmentEffect
    {
        public override string Name => "Shading";

        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(ShadingEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
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

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(ShadingEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
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

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(ShadingEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
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

        public ShadingEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(ShadingEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(RedProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(BlueProperty);
        }

        public ShadingEffect(double red, double green, double blue) : this()
        {
            SetCurrentValue(RedProperty, red);
            SetCurrentValue(GreenProperty, green);
            SetCurrentValue(BlueProperty, blue);
        }

        public override AdjustmentEffect Copy()
        {
            return new ShadingEffect()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }
}