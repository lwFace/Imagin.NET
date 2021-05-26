using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that embosses the input.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class EmbossEffect : AdjustmentEffect
    {
        public override string Name => "Emboss";

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register("Amount", typeof(double), typeof(EmbossEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(EmbossEffect), new UIPropertyMetadata(((double)(0.003D)), PixelShaderConstantCallback(1)));

        public EmbossEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(EmbossEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(AmountProperty);
            UpdateShaderValue(WidthProperty);
        }

        /// <summary>The amplitude of the embossing.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Amount
        {
            get
            {
                return ((double)(this.GetValue(AmountProperty)));
            }
            set
            {
                this.SetValue(AmountProperty, value);
            }
        }

        /// <summary>The separation between samples (as a fraction of input size).</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Width
        {
            get
            {
                return ((double)(this.GetValue(WidthProperty)));
            }
            set
            {
                this.SetValue(WidthProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new EmbossEffect()
            {
                Amount = Amount,
                Width = Width
            };
        }
    }
}
