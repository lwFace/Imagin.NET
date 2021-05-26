using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that magnifies a circular region with a smooth boundary.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class SmoothMagnifyEffect : AdjustmentEffect
    {
        public override string Name => "Smooth magnify";

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(((double)(0.2D)), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register("OuterRadius", typeof(double), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(((double)(0.4D)), PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty MagnificationProperty = DependencyProperty.Register("Magnification", typeof(double), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(3)));

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(4)));

        public SmoothMagnifyEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(SmoothMagnifyEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(InnerRadiusProperty);
            UpdateShaderValue(OuterRadiusProperty);
            UpdateShaderValue(MagnificationProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        /// <summary>The center point of the magnified region.</summary>
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

        /// <summary>The inner radius of the magnified region.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double InnerRadius
        {
            get
            {
                return ((double)(GetValue(InnerRadiusProperty)));
            }
            set
            {
                SetValue(InnerRadiusProperty, value);
            }
        }

        /// <summary>The outer radius of the magnified region.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double OuterRadius
        {
            get
            {
                return ((double)(GetValue(OuterRadiusProperty)));
            }
            set
            {
                SetValue(OuterRadiusProperty, value);
            }
        }

        /// <summary>The magnification factor.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Magnification
        {
            get
            {
                return ((double)(GetValue(MagnificationProperty)));
            }
            set
            {
                SetValue(MagnificationProperty, value);
            }
        }

        /// <summary>The aspect ratio (width / height) of the input.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double AspectRatio
        {
            get
            {
                return ((double)(GetValue(AspectRatioProperty)));
            }
            set
            {
                SetValue(AspectRatioProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new SmoothMagnifyEffect()
            {
                Center = Center,
                InnerRadius = InnerRadius,
                OuterRadius = OuterRadius,
                Magnification = Magnification,
                AspectRatio = AspectRatio
            };
        }
    }
}
