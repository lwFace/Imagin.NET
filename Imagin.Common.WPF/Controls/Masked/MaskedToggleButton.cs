using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class MaskedToggleButton : ToggleButton
    {
        public static DependencyProperty CheckedContentProperty = DependencyProperty.Register(nameof(CheckedContent), typeof(object), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object CheckedContent
        {
            get => GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty CheckedSourceProperty = DependencyProperty.Register(nameof(CheckedSource), typeof(ImageSource), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.None));
        public ImageSource CheckedSource
        {
            get => (ImageSource)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty CheckedToolTipProperty = DependencyProperty.Register(nameof(CheckedToolTip), typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string CheckedToolTip
        {
            get => (string)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof(ContentMargin), typeof(Thickness), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.None));
        public Thickness ContentMargin
        {
            get => (Thickness)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof(Menu), typeof(ContextMenu), typeof(MaskedToggleButton), new UIPropertyMetadata(null, OnMenuChanged));
        public ContextMenu Menu
        {
            get => (ContextMenu)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }
        static void OnMenuChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<MaskedToggleButton>().OnMenuChanged(new OldNew<ContextMenu>(e));

        public static DependencyProperty MenuAnimationProperty = DependencyProperty.Register(nameof(MenuAnimation), typeof(PopupAnimation), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(PopupAnimation.Fade, FrameworkPropertyMetadataOptions.None));
        public PopupAnimation MenuAnimation
        {
            get => (PopupAnimation)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty MenuButtonToolTipProperty = DependencyProperty.Register(nameof(MenuButtonToolTip), typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public string MenuButtonToolTip
        {
            get => (string)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty MenuButtonVisibilityProperty = DependencyProperty.Register(nameof(MenuButtonVisibility), typeof(Visibility), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility MenuButtonVisibility
        {
            get => (Visibility)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty MenuPlacementProperty = DependencyProperty.Register(nameof(MenuPlacement), typeof(PlacementMode), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(PlacementMode.Bottom, FrameworkPropertyMetadataOptions.None));
        public PlacementMode MenuPlacement
        {
            get => (PlacementMode)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty MenuVisibilityProperty = DependencyProperty.Register(nameof(MenuVisibility), typeof(Visibility), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility MenuVisibility
        {
            get => (Visibility)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnGroupNameChanged));
        public string GroupName
        {
            get => (string)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }
        static void OnGroupNameChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<MaskedToggleButton>().OnGroupNameChanged(new OldNew<string>(e));

        public static DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.None));
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register(nameof(SourceColor), typeof(Brush), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Brush SourceColor
        {
            get => (Brush)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty SourceHeightProperty = DependencyProperty.Register(nameof(SourceHeight), typeof(double), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.None));
        public double SourceHeight
        {
            get => (double)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public static DependencyProperty SourceWidthProperty = DependencyProperty.Register(nameof(SourceWidth), typeof(double), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.None));
        public double SourceWidth
        {
            get => (double)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public MaskedToggleButton()
        {
            DefaultStyleKey = typeof(MaskedToggleButton);
        }

        void OnChecked(object sender, EventArgs e)
        {
            Try.Invoke(() =>
            {
                var parent = this.GetParent<DependencyObject>();
                for (int i = 0, Count = VisualTreeHelper.GetChildrenCount(parent); i < Count; i++)
                {
                    var j = VisualTreeHelper.GetChild(parent, i);
                    if (j is MaskedToggleMenu k)
                    {
                        if (k != this)
                            k.IsChecked = false;
                    }
                }
            });
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetCurrentValue(IsCheckedProperty, true);
        }

        protected virtual void OnMenuChanged(OldNew<ContextMenu> input)
        {
            if (input.New != null)
            {
                input.New.PlacementTarget = this;
                input.New.Placement = PlacementMode.Bottom;
                input.New.Bind(ContextMenu.IsOpenProperty, nameof(IsChecked), this, BindingMode.TwoWay);
            }
        }

        protected virtual void OnGroupNameChanged(OldNew<string> input)
        {
            if (input.New.NullOrEmpty())
            {
                Checked -= OnChecked;
            }
            else Checked += OnChecked;
        }
    }
}