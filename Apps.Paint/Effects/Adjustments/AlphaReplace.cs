using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that makes pixels of a particular color transparent.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class AlphaReplaceEffect : AdjustmentEffect
    {
        public override string Name => "Alpha replace";

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(AlphaReplaceEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 128, 0), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty ToleranceProperty = DependencyProperty.Register("Tolerance", typeof(double), typeof(AlphaReplaceEffect), new UIPropertyMetadata(((double)(0.3D)), PixelShaderConstantCallback(1)));

        public AlphaReplaceEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(AlphaReplaceEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(ColorProperty);
            UpdateShaderValue(ToleranceProperty);
        }

        /// <summary>The color that becomes transparent.</summary>
        [Hidden(false)]
        public Color Color
        {
            get => ((Color)(GetValue(ColorProperty)));
            set => SetValue(ColorProperty, value);
        }

        /// <summary>The tolerance in color differences.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Tolerance
        {
            get => ((double)(GetValue(ToleranceProperty)));
            set => SetValue(ToleranceProperty, value);
        }

        public override AdjustmentEffect Copy()
        {
            return new AlphaReplaceEffect()
            {
                Color = Color,
                Tolerance = Tolerance
            };
        }
    }
}