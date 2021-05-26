using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An paper sketch effect.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class SketchGraniteEffect : AdjustmentEffect
    {
        public override string Name => "Sketch (granite)";

        public static readonly DependencyProperty BrushSizeProperty = DependencyProperty.Register("BrushSize", typeof(double), typeof(SketchGraniteEffect), new UIPropertyMetadata(((double)(0.003D)), PixelShaderConstantCallback(0)));

        public SketchGraniteEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(SketchGraniteEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(BrushSizeProperty);
        }

        /// <summary>The brush size of the sketch effect.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double BrushSize
        {
            get
            {
                return ((double)(GetValue(BrushSizeProperty)));
            }
            set
            {
                SetValue(BrushSizeProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new SketchGraniteEffect()
            {
                BrushSize = BrushSize
            };
        }
    }
}
