using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>Pixel shader: Edge detection using a parametric, symetric, directional convolution kernel</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class ParametricEdgeDetectionEffect : AdjustmentEffect
    {
        public override string Name => "Parametric edge detection";

        public static readonly DependencyProperty ThreshholdProperty = DependencyProperty.Register("Threshhold", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty K00Property = DependencyProperty.Register("K00", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty K01Property = DependencyProperty.Register("K01", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty K02Property = DependencyProperty.Register("K02", typeof(double), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));

        public static readonly DependencyProperty TextureSizeProperty = DependencyProperty.Register("TextureSize", typeof(Point), typeof(ParametricEdgeDetectionEffect), new UIPropertyMetadata(new Point(512D, 512D), PixelShaderConstantCallback(4)));

        public ParametricEdgeDetectionEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(ParametricEdgeDetectionEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(ThreshholdProperty);
            UpdateShaderValue(K00Property);
            UpdateShaderValue(K01Property);
            UpdateShaderValue(K02Property);
            UpdateShaderValue(TextureSizeProperty);
        }

        /// <summary>The threshold of the edge detection.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Threshhold
        {
            get
            {
                return ((double)(GetValue(ThreshholdProperty)));
            }
            set
            {
                SetValue(ThreshholdProperty, value);
            }
        }

        /// <summary>Kernel first column top. Default is the Sobel operator.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double K00
        {
            get
            {
                return ((double)(GetValue(K00Property)));
            }
            set
            {
                SetValue(K00Property, value);
            }
        }

        /// <summary>Kernel first column middle. Default is the Sobel operator.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double K01
        {
            get
            {
                return ((double)(GetValue(K01Property)));
            }
            set
            {
                SetValue(K01Property, value);
            }
        }

        /// <summary>Kernel first column bottom. Default is the Sobel operator.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double K02
        {
            get
            {
                return ((double)(GetValue(K02Property)));
            }
            set
            {
                SetValue(K02Property, value);
            }
        }

        /// <summary>The size of the texture.</summary>
        [Hidden(false)]
        public Point TextureSize
        {
            get
            {
                return ((Point)(GetValue(TextureSizeProperty)));
            }
            set
            {
                SetValue(TextureSizeProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new ParametricEdgeDetectionEffect()
            {
                Threshhold = Threshhold,
                K00 = K00,
                K01 = K01,
                K02 = K02,
                TextureSize = TextureSize
            };
        }
    }
}
