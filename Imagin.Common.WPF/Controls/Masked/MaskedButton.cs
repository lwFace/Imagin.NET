using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_MaskedImage), Type = typeof(MaskedImage))]
    public class MaskedButton : Button
    {
        MaskedImage PART_MaskedImage;
        
        public static DependencyProperty ButtonSizeProperty = DependencyProperty.Register(nameof(ButtonSize), typeof(DoubleSize), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize ButtonSize
        {
            get => (DoubleSize)GetValue(ButtonSizeProperty);
            set => SetValue(ButtonSizeProperty, value);
        }

        public static DependencyProperty ButtonSourceProperty = DependencyProperty.Register(nameof(ButtonSource), typeof(ImageSource), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ImageSource ButtonSource
        {
            get => (ImageSource)GetValue(ButtonSourceProperty);
            set => SetValue(ButtonSourceProperty, value);
        }

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof(ContentMargin), typeof(Thickness), typeof(MaskedButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.None));
        public Thickness ContentMargin
        {
            get => (Thickness)GetValue(ContentMarginProperty);
            set => SetValue(ContentMarginProperty, value);
        }

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof(Menu), typeof(ContextMenu), typeof(MaskedButton), new UIPropertyMetadata(null, OnMenuChanged));
        public ContextMenu Menu
        {
            get => (ContextMenu)GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }
        static void OnMenuChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<MaskedButton>().OnMenuChanged(new OldNew<ContextMenu>(e));

        public static DependencyProperty MenuAnimationProperty = DependencyProperty.Register(nameof(MenuAnimation), typeof(PopupAnimation), typeof(MaskedButton), new FrameworkPropertyMetadata(PopupAnimation.Fade, FrameworkPropertyMetadataOptions.None));
        public PopupAnimation MenuAnimation
        {
            get => (PopupAnimation)GetValue(MenuAnimationProperty);
            set => SetValue(MenuAnimationProperty, value);
        }

        public static DependencyProperty MenuButtonToolTipProperty = DependencyProperty.Register(nameof(MenuButtonToolTip), typeof(string), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public string MenuButtonToolTip
        {
            get => (string)GetValue(MenuButtonToolTipProperty);
            set => SetValue(MenuButtonToolTipProperty, value);
        }

        public static DependencyProperty MenuButtonVisibilityProperty = DependencyProperty.Register(nameof(MenuButtonVisibility), typeof(Visibility), typeof(MaskedButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility MenuButtonVisibility
        {
            get => (Visibility)GetValue(MenuButtonVisibilityProperty);
            set => SetValue(MenuButtonVisibilityProperty, value);
        }

        public static DependencyProperty DropDownPlacementProperty = DependencyProperty.Register(nameof(MenuPlacement), typeof(PlacementMode), typeof(MaskedButton), new FrameworkPropertyMetadata(PlacementMode.Bottom, FrameworkPropertyMetadataOptions.None));
        public PlacementMode MenuPlacement
        {
            get => (PlacementMode)GetValue(DropDownPlacementProperty);
            set => SetValue(DropDownPlacementProperty, value);
        }

        public static DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(MaskedButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register(nameof(SourceColor), typeof(Brush), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Brush SourceColor
        {
            get => (Brush)GetValue(SourceColorProperty);
            set => SetValue(SourceColorProperty, value);
        }

        public static DependencyProperty SourceHeightProperty = DependencyProperty.Register(nameof(SourceHeight), typeof(double), typeof(MaskedButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.None));
        public double SourceHeight
        {
            get => (double)GetValue(SourceHeightProperty);
            set => SetValue(SourceHeightProperty, value);
        }

        public static DependencyProperty SourceWidthProperty = DependencyProperty.Register(nameof(SourceWidth), typeof(double), typeof(MaskedButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.None));
        public double SourceWidth
        {
            get => (double)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public MaskedButton() : base()
        {
            DefaultStyleKey = typeof(MaskedButton);
            SetCurrentValue(ButtonSizeProperty, new DoubleSize(16, 16));
        }

        void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetCurrentValue(IsCheckedProperty, true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_MaskedImage = Template.FindName(nameof(PART_MaskedImage), this) as MaskedImage;
            PART_MaskedImage.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
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