using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Effects
{
    public class ChannelsEffect : BaseEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(ChannelsEffect), 0);

        public static readonly DependencyProperty RProperty = DependencyProperty.Register("R", typeof(double), typeof(ChannelsEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty GProperty = DependencyProperty.Register("G", typeof(double), typeof(ChannelsEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty BProperty = DependencyProperty.Register("B", typeof(double), typeof(ChannelsEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(2)));

        public static DependencyProperty RedProperty = DependencyProperty.Register(nameof(Red), typeof(bool), typeof(ChannelsEffect), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None, OnRedChanged));
        public bool Red
        {
            get => (bool)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }
        protected static void OnRedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChannelsEffect).SetCurrentValue(RProperty, (bool)e.NewValue ? 1.0 : 0.0);
        }

        public static DependencyProperty GreenProperty = DependencyProperty.Register(nameof(Green), typeof(bool), typeof(ChannelsEffect), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None, OnGreenChanged));
        public bool Green
        {
            get => (bool)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }
        protected static void OnGreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChannelsEffect).SetCurrentValue(GProperty, (bool)e.NewValue ? 1.0 : 0.0);
        }

        public static DependencyProperty BlueProperty = DependencyProperty.Register(nameof(Blue), typeof(bool), typeof(ChannelsEffect), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None, OnBlueChanged));
        public bool Blue
        {
            get => (bool)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }
        protected static void OnBlueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChannelsEffect).SetCurrentValue(BProperty, (bool)e.NewValue ? 1.0 : 0.0);
        }

        public ChannelsEffect()
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/Paint;component/Effects/Channels.ps", UriKind.Relative);
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(RProperty);
            UpdateShaderValue(GProperty);
            UpdateShaderValue(BProperty);
        }

        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }

        public double R
        {
            get
            {
                return ((double)(this.GetValue(RProperty)));
            }
            set
            {
                this.SetValue(RProperty, value);
            }
        }

        public double G
        {
            get
            {
                return ((double)(this.GetValue(GProperty)));
            }
            set
            {
                this.SetValue(GProperty, value);
            }
        }

        public double B
        {
            get
            {
                return ((double)(this.GetValue(BProperty)));
            }
            set
            {
                this.SetValue(BProperty, value);
            }
        }
    }
}