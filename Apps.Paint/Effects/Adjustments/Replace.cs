using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that makes pixels of a particular color another color.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class ReplaceEffect : AdjustmentEffect
    {
        public override string Name => "Replace";

        public static readonly DependencyProperty Color1Property = DependencyProperty.Register("Color1", typeof(Color), typeof(ReplaceEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 128, 0), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty Color2Property = DependencyProperty.Register("Color2", typeof(Color), typeof(ReplaceEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty ToleranceProperty = DependencyProperty.Register("Tolerance", typeof(double), typeof(ReplaceEffect), new UIPropertyMetadata(((double)(0.9D)), PixelShaderConstantCallback(2)));

        public ReplaceEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(ReplaceEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(Color1Property);
            UpdateShaderValue(Color2Property);
            UpdateShaderValue(ToleranceProperty);
        }

        [Hidden(false)]
        public Color Color1
        {
            get
            {
                return ((Color)(this.GetValue(Color1Property)));
            }
            set
            {
                this.SetValue(Color1Property, value);
            }
        }

        [Hidden(false)]
        public Color Color2
        {
            get
            {
                return ((Color)(this.GetValue(Color2Property)));
            }
            set
            {
                this.SetValue(Color2Property, value);
            }
        }

        /// <summary>The tolerance in color differences.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Tolerance
        {
            get
            {
                return ((double)(this.GetValue(ToleranceProperty)));
            }
            set
            {
                this.SetValue(ToleranceProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new ReplaceEffect()
            {
                Color1 = Color1,
                Color2 = Color2,
                Tolerance = Tolerance
            };
        }
    }
}