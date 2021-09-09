using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that applies a radial blur to the input.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class ZoomBlurEffect : AdjustmentEffect
    {
        public override string Name => "Zoom blur";

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(ZoomBlurEffect), new UIPropertyMetadata(new Point(0.9D, 0.6D), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register("BlurAmount", typeof(double), typeof(ZoomBlurEffect), new UIPropertyMetadata(((double)(0.1D)), PixelShaderConstantCallback(1)));

        public ZoomBlurEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(ZoomBlurEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(BlurAmountProperty);
        }

        /// <summary>The center of the blur.</summary>
        [Hidden(false)]
        public Point Center
        {
            get
            {
                return ((Point)(GetValue(CenterProperty)));
            }
            set
            {
                SetValue(CenterProperty, value);
            }
        }

        /// <summary>The amount of blur.</summary>
        [DisplayName("Amount")]
        [Hidden(false)]
        [Range(0.0, 0.2, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double BlurAmount
        {
            get
            {
                return ((double)(GetValue(BlurAmountProperty)));
            }
            set
            {
                SetValue(BlurAmountProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new ZoomBlurEffect()
            {
                Center = Center,
                BlurAmount = BlurAmount
            };
        }
    }
}
