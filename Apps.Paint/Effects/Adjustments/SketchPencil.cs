using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>A pencil stroke effect.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class SketchPencilEffect : AdjustmentEffect
    {
        public override string Name => "Sketch (pencil)";

        public static readonly DependencyProperty BrushSizeProperty = DependencyProperty.Register("BrushSize", typeof(double), typeof(SketchPencilEffect), new UIPropertyMetadata(((double)(0.005D)), PixelShaderConstantCallback(0)));

        public SketchPencilEffect()
        {
            PixelShader pixelShader = new PixelShader() { UriSource = Resource(nameof(SketchPencilEffect)) };
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
            return new SketchPencilEffect()
            {
                BrushSize = BrushSize
            };
        }
    }
}
