using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that sharpens the input.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class SharpenEffect : AdjustmentEffect
    {
        public override string Name => "Sharpen";

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register("Amount", typeof(double), typeof(SharpenEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty InputSizeProperty = DependencyProperty.Register("InputSize", typeof(Size), typeof(SharpenEffect), new UIPropertyMetadata(new Size(800D, 600D), PixelShaderConstantCallback(1)));

        public SharpenEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(SharpenEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(AmountProperty);
            UpdateShaderValue(InputSizeProperty);
        }

        /// <summary>The amount of sharpening.</summary>
        [Hidden(false)]
        [Range(0.0, 2.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Amount
        {
            get
            {
                return ((double)(GetValue(AmountProperty)));
            }
            set
            {
                SetValue(AmountProperty, value);
            }
        }

        /// <summary>The size of the input (in pixels).</summary>
        [Hidden(false)]
        public Size InputSize
        {
            get
            {
                return ((Size)(GetValue(InputSizeProperty)));
            }
            set
            {
                SetValue(InputSizeProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new SharpenEffect()
            {
                Amount = Amount,
                InputSize = InputSize
            };
        }
    }
}
