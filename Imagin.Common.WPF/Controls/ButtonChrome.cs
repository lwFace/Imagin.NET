using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class ButtonChrome : ContentControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ButtonChrome), new UIPropertyMetadata(default(CornerRadius), new PropertyChangedCallback(OnCornerRadiusChanged)));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        static void OnCornerRadiusChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is ButtonChrome buttonChrome)
                buttonChrome.OnCornerRadiusChanged(new OldNew<CornerRadius>(e));
        }

        public static readonly DependencyProperty InnerCornerRadiusProperty = DependencyProperty.Register(nameof(InnerCornerRadius), typeof(CornerRadius), typeof(ButtonChrome), new UIPropertyMetadata(default(CornerRadius)));
        public CornerRadius InnerCornerRadius
        {
            get => (CornerRadius)GetValue(InnerCornerRadiusProperty);
            set => SetValue(InnerCornerRadiusProperty, value);
        }

        public static readonly DependencyProperty RenderCheckedProperty = DependencyProperty.Register(nameof(RenderChecked), typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        public bool RenderChecked
        {
            get => (bool)GetValue(RenderCheckedProperty);
            set => SetValue(RenderCheckedProperty, value);
        }

        public static readonly DependencyProperty RenderEnabledProperty = DependencyProperty.Register(nameof(RenderEnabled), typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(true));
        public bool RenderEnabled
        {
            get => (bool)GetValue(RenderEnabledProperty);
            set => SetValue(RenderEnabledProperty, value);
        }

        public static readonly DependencyProperty RenderFocusedProperty = DependencyProperty.Register(nameof(RenderFocused), typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        public bool RenderFocused
        {
            get => (bool)GetValue(RenderFocusedProperty);
            set => SetValue(RenderFocusedProperty, value);
        }

        public static readonly DependencyProperty RenderMouseOverProperty = DependencyProperty.Register(nameof(RenderMouseOver), typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        public bool RenderMouseOver
        {
            get => (bool)GetValue(RenderMouseOverProperty);
            set => SetValue(RenderMouseOverProperty, value);
        }

        public static readonly DependencyProperty RenderNormalProperty = DependencyProperty.Register(nameof(RenderNormal), typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(true));
        public bool RenderNormal
        {
            get => (bool)GetValue(RenderNormalProperty);
            set => SetValue(RenderNormalProperty, value);
        }

        public static readonly DependencyProperty RenderPressedProperty = DependencyProperty.Register(nameof(RenderPressed), typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false));
        public bool RenderPressed
        {
            get => (bool)GetValue(RenderPressedProperty);
            set => SetValue(RenderPressedProperty, value);
        }

        static ButtonChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonChrome), new FrameworkPropertyMetadata(typeof(ButtonChrome)));
        }

        protected virtual void OnCornerRadiusChanged(OldNew<CornerRadius> input)
        {
            var cornerRadius = new CornerRadius
            (
                System.Math.Max(0, input.New.TopLeft - 1),
                System.Math.Max(0, input.New.TopRight - 1),
                System.Math.Max(0, input.New.BottomRight - 1),
                System.Math.Max(0, input.New.BottomLeft - 1)
            );
            SetCurrentValue(InnerCornerRadiusProperty, cornerRadius);
        }
    }
}