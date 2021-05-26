using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>
    /// An effect that blurs in a single direction.
    /// </summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class DirectionalBlurEffect : AdjustmentEffect
    {
        public override string Name => "Directional blur";

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(DirectionalBlurEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        /// <summary>The direction of the blur (in degrees).</summary>
        [Hidden(false)]
        [Range(0.0, 359.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register("BlurAmount", typeof(double), typeof(DirectionalBlurEffect), new UIPropertyMetadata(((double)(0.003D)), PixelShaderConstantCallback(1)));
        /// <summary>The scale of the blur (as a fraction of the input size).</summary>
        [Hidden(false)]
        [Range(0.0, 0.01, 0.001)]
        [RangeFormat(RangeFormat.Slider)]
        public double BlurAmount
        {
            get => (double)GetValue(BlurAmountProperty);
            set => SetValue(BlurAmountProperty, value);
        }

        public DirectionalBlurEffect()
        {
            var pixelShader = new PixelShader() { UriSource = Resource(nameof(DirectionalBlurEffect)) };
            PixelShader = pixelShader;

            UpdateShaderValue(AngleProperty);
            UpdateShaderValue(BlurAmountProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new DirectionalBlurEffect()
            {
                Angle = Angle,
                BlurAmount = BlurAmount
            };
        }
    }
}