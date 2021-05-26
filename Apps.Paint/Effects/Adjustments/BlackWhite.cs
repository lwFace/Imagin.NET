using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class BlackWhiteEffect : AdjustmentEffect
    {
        public override string Name => "Black/white";

        public static readonly DependencyProperty RedsProperty = DependencyProperty.Register("Reds", typeof(double), typeof(BlackWhiteEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public double Reds
        {
            get => (double)GetValue(RedsProperty);
            set => SetValue(RedsProperty, value);
        }

        public static readonly DependencyProperty GreensProperty = DependencyProperty.Register("Greens", typeof(double), typeof(BlackWhiteEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public double Greens
        {
            get => (double)GetValue(GreensProperty);
            set => SetValue(GreensProperty, value);
        }

        public static readonly DependencyProperty BluesProperty = DependencyProperty.Register("Blues", typeof(double), typeof(BlackWhiteEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        public double Blues
        {
            get => (double)GetValue(BluesProperty);
            set => SetValue(BluesProperty, value);
        }

        public static readonly DependencyProperty CyansProperty = DependencyProperty.Register("Cyans", typeof(double), typeof(BlackWhiteEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        public double Cyans
        {
            get => (double)GetValue(CyansProperty);
            set => SetValue(CyansProperty, value);
        }

        public static readonly DependencyProperty YellowsProperty = DependencyProperty.Register("Yellows", typeof(double), typeof(BlackWhiteEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));
        public double Yellows
        {
            get => (double)GetValue(YellowsProperty);
            set => SetValue(YellowsProperty, value);
        }

        public static readonly DependencyProperty MagentasProperty = DependencyProperty.Register("Magentas", typeof(double), typeof(BlackWhiteEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(5)));
        public double Magentas
        {
            get => (double)GetValue(MagentasProperty);
            set => SetValue(MagentasProperty, value);
        }

        public BlackWhiteEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(BlackWhiteEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(RedsProperty);
            UpdateShaderValue(GreensProperty);
            UpdateShaderValue(BluesProperty);
            UpdateShaderValue(CyansProperty);
            UpdateShaderValue(YellowsProperty);
            UpdateShaderValue(MagentasProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new BlackWhiteEffect()
            {
                Reds = Reds,
                Greens = Greens,
                Blues = Blues,
                Cyans = Cyans,
                Yellows = Yellows,
                Magentas = Magentas,
            };
        }
    }
}