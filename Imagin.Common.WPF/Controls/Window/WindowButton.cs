using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class WindowButton : Button
    {
        public static readonly DependencyProperty ContentSizeProperty = DependencyProperty.Register(nameof(ContentSize), typeof(DoubleSize), typeof(WindowButton), new FrameworkPropertyMetadata(default(DoubleSize), FrameworkPropertyMetadataOptions.None));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize ContentSize
        {
            get => (DoubleSize)GetValue(ContentSizeProperty);
            set => SetValue(ContentSizeProperty, value);
        }

        public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(nameof(IsCheckable), typeof(bool), typeof(WindowButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsCheckable
        {
            get => (bool)GetValue(IsCheckableProperty);
            set => SetValue(IsCheckableProperty, value);
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(WindowButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, null, OnIsCheckedCoerced));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
        static object OnIsCheckedCoerced(DependencyObject d, object value) => !(d as WindowButton).IsCheckable ? false : value ?? false;

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof(Menu), typeof(ContextMenu), typeof(WindowButton), new UIPropertyMetadata(null, OnMenuChanged));
        public ContextMenu Menu
        {
            get => (ContextMenu)GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }
        static void OnMenuChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<WindowButton>().OnMenuChanged(new OldNew<ContextMenu>(e));

        public WindowButton()
        {
            DefaultStyleKey = typeof(WindowButton);
            SetCurrentValue(ContentSizeProperty, new DoubleSize(16, 16));
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (IsCheckable)
                SetCurrentValue(IsCheckedProperty, !IsChecked);
        }

        protected virtual void OnMenuChanged(OldNew<ContextMenu> input)
        {
            if (input.New != null)
            {
                input.New.Placement = PlacementMode.Bottom;
                input.New.PlacementTarget = this;
                input.New.Bind(ContextMenu.IsOpenProperty, nameof(IsChecked), this, BindingMode.TwoWay);
            }
        }
    }
}