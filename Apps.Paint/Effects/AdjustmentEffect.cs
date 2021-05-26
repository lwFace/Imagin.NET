using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    public abstract class AdjustmentEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(AdjustmentEffect), 0);
        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public static DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool), typeof(AdjustmentEffect), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        public abstract string Name { get; }

        Guid? index;
        public Guid Index => (index = index ?? Guid.NewGuid()).Value;

        public AdjustmentEffect() : base()
        {
            UpdateShaderValue(InputProperty);
        }

        /// <summary>
        /// Change <see langword="name.Replace(nameof(Effect), string.Empty)"/> to something simpler at some point...
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Uri Resource(string name) => new Uri($"/Paint;component/Effects/Adjustments/{name.Replace(nameof(Effect), string.Empty)}.ps", UriKind.Relative);

        public abstract AdjustmentEffect Copy();

        public override string ToString() => Name;
    }
}