using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that turns the input into blocky pixels.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class PixelateEffect : AdjustmentEffect
    {
        public override string Name => "Pixelate";

        public static readonly DependencyProperty PixelCountsProperty = DependencyProperty.Register("PixelCounts", typeof(Size), typeof(PixelateEffect), new UIPropertyMetadata(new Size(60D, 40D), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BrickOffsetProperty = DependencyProperty.Register("BrickOffset", typeof(double), typeof(PixelateEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));

        public PixelateEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(PixelateEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(PixelCountsProperty);
            UpdateShaderValue(BrickOffsetProperty);
        }

        /// <summary>The number of horizontal and vertical pixel blocks.</summary>
        [Hidden(false)]
        public Size PixelCounts
        {
            get
            {
                return ((Size)(GetValue(PixelCountsProperty)));
            }
            set
            {
                SetValue(PixelCountsProperty, value);
            }
        }

        /// <summary>The amount to shift alternate rows (use 1 to get a brick wall look).</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double BrickOffset
        {
            get
            {
                return ((double)(GetValue(BrickOffsetProperty)));
            }
            set
            {
                SetValue(BrickOffsetProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new PixelateEffect()
            {
                PixelCounts = PixelCounts,
                BrickOffset = BrickOffset
            };
        }
    }
}
