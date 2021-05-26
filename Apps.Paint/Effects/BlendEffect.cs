using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Paint.Effects
{
    public abstract class BlendEffect : BaseEffect
    {
        public static Uri Resource(string name)
            => new Uri($"pack://application:,,,/Paint;component/Effects/Blend/{name}.ps", UriKind.RelativeOrAbsolute);

        public static readonly DependencyProperty AInputProperty = RegisterPixelShaderSamplerProperty(nameof(AInput), typeof(BlendEffect), 0);
        [Browsable(false)]
		public Brush AInput
		{
            get => (Brush)GetValue(AInputProperty);
            set => SetValue(AInputProperty, value);
        }

		public static readonly DependencyProperty BInputProperty = RegisterPixelShaderSamplerProperty(nameof(BInput), typeof(BlendEffect), 1);
        public Brush BInput
        {
            get => (Brush)GetValue(BInputProperty);
            set => SetValue(BInputProperty, value);
        }

        public BlendEffect() : base()
        {
            UpdateShaderValue(AInputProperty);
            UpdateShaderValue(BInputProperty);
        }
    }
}