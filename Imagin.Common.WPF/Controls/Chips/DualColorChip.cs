using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    public class DualColorChip : Control
    {
        Grid PART_Reset;

        Rectangle PART_Switch;

        public event EventHandler<EventArgs<Color>> ForegroundColorChanged;

        public event EventHandler<EventArgs<Color>> BackgroundColorChanged;

        public static DependencyProperty BackgroundColorProperty = DependencyProperty.Register(nameof(BackgroundColor), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White, OnBackgroundColorChanged));
        public Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        static void OnBackgroundColorChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DualColorChip>().OnBackgroundColorChanged(new OldNew<Color>(e));

        public static DependencyProperty BackgroundToolTipProperty = DependencyProperty.Register(nameof(BackgroundToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Background"));
        public string BackgroundToolTip
        {
            get => (string)GetValue(BackgroundToolTipProperty);
            set => SetValue(BackgroundToolTipProperty, value);
        }

        public static DependencyProperty DefaultBackgroundProperty = DependencyProperty.Register(nameof(DefaultBackground), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.White));
        public Color DefaultBackground
        {
            get => (Color)GetValue(DefaultBackgroundProperty);
            set => SetValue(DefaultBackgroundProperty, value);
        }

        public static DependencyProperty DefaultForegroundProperty = DependencyProperty.Register(nameof(DefaultForeground), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black));
        public Color DefaultForeground
        {
            get => (Color)GetValue(DefaultForegroundProperty);
            set => SetValue(DefaultForegroundProperty, value);
        }

        public static DependencyProperty ForegroundColorProperty = DependencyProperty.Register(nameof(ForegroundColor), typeof(Color), typeof(DualColorChip), new PropertyMetadata(Colors.Black, OnForegroundColorChanged));
        public Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }
        static void OnForegroundColorChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DualColorChip>().OnForegroundColorChanged(new OldNew<Color>(e));

        public static DependencyProperty ForegroundToolTipProperty = DependencyProperty.Register(nameof(ForegroundToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Foreground"));
        public string ForegroundToolTip
        {
            get => (string)GetValue(ForegroundToolTipProperty);
            set => SetValue(ForegroundToolTipProperty, value);
        }

        public static DependencyProperty ResetToolTipProperty = DependencyProperty.Register(nameof(ResetToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Reset"));
        public string ResetToolTip
        {
            get => (string)GetValue(ResetToolTipProperty);
            set => SetValue(ResetToolTipProperty, value);
        }

        public static DependencyProperty SwitchToolTipProperty = DependencyProperty.Register(nameof(SwitchToolTip), typeof(string), typeof(DualColorChip), new PropertyMetadata("Swap"));
        public string SwitchToolTip
        {
            get => (string)GetValue(SwitchToolTipProperty);
            set => SetValue(SwitchToolTipProperty, value);
        }

        public DualColorChip() => DefaultStyleKey = typeof(DualColorChip);

        void OnResetMouseDown(object sender, MouseButtonEventArgs e)
        {
            BackgroundColor = DefaultBackground;
            ForegroundColor = DefaultForeground;
        }

        void OnSwitchMouseDown(object sender, MouseButtonEventArgs e)
        {
            var a = ForegroundColor;
            var b = BackgroundColor;

            ForegroundColor = b;
            BackgroundColor = a;
        }

        protected virtual void OnBackgroundColorChanged(OldNew<Color> input) => BackgroundColorChanged?.Invoke(this, new EventArgs<Color>(input.New));

        protected virtual void OnForegroundColorChanged(OldNew<Color> input) => ForegroundColorChanged?.Invoke(this, new EventArgs<Color>(input.New));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Reset = Template.FindName("PART_Reset", this) as Grid;
            PART_Reset.PreviewMouseDown += OnResetMouseDown;

            PART_Switch = Template.FindName("PART_Switch", this) as Rectangle;
            PART_Switch.PreviewMouseDown += OnSwitchMouseDown;
        }
    }
}