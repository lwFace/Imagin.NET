using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect mimics the look of glass tiles.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class GlassTilesEffect : AdjustmentEffect
    {
        public override string Name => "Glass tiles";

        public static readonly DependencyProperty TilesProperty = DependencyProperty.Register("Tiles", typeof(double), typeof(GlassTilesEffect), new UIPropertyMetadata(((double)(5D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BevelWidthProperty = DependencyProperty.Register("BevelWidth", typeof(double), typeof(GlassTilesEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(GlassTilesEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty GroutColorProperty = DependencyProperty.Register("GroutColor", typeof(Color), typeof(GlassTilesEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(2)));

        public GlassTilesEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(GlassTilesEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(TilesProperty);
            UpdateShaderValue(BevelWidthProperty);
            UpdateShaderValue(OffsetProperty);
            UpdateShaderValue(GroutColorProperty);
        }

        /// <summary>The approximate number of tiles per row/column.</summary>
        [Hidden(false)]
        [Range(0.0, 20.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Tiles
        {
            get
            {
                return ((double)(GetValue(TilesProperty)));
            }
            set
            {
                SetValue(TilesProperty, value);
            }
        }

        [Hidden(false)]
        [Range(0.0, 10.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double BevelWidth
        {
            get
            {
                return ((double)(GetValue(BevelWidthProperty)));
            }
            set
            {
                SetValue(BevelWidthProperty, value);
            }
        }

        [Hidden(false)]
        [Range(0.0, 3.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Offset
        {
            get
            {
                return ((double)(GetValue(OffsetProperty)));
            }
            set
            {
                SetValue(OffsetProperty, value);
            }
        }

        [Hidden(false)]
        public Color GroutColor
        {
            get
            {
                return ((Color)(GetValue(GroutColorProperty)));
            }
            set
            {
                SetValue(GroutColorProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new GlassTilesEffect()
            {
                Tiles = Tiles,
                BevelWidth = BevelWidth,
                Offset = Offset,
                GroutColor = GroutColor
            };
        }
    }
}
