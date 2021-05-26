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
    public class MaskedToggleMenu : MaskedToggleButton
    {
        MaskedImage PART_MaskedImage;

        public static DependencyProperty ButtonSizeProperty = DependencyProperty.Register(nameof(ButtonSize), typeof(DoubleSize), typeof(MaskedToggleMenu), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize ButtonSize
        {
            get => (DoubleSize)GetValue(ButtonSizeProperty);
            set => SetValue(ButtonSizeProperty, value);
        }

        public static DependencyProperty ButtonSourceProperty = DependencyProperty.Register(nameof(ButtonSource), typeof(ImageSource), typeof(MaskedToggleMenu), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ImageSource ButtonSource
        {
            get => (ImageSource)GetValue(ButtonSourceProperty);
            set => SetValue(ButtonSourceProperty, value);
        }

        public static DependencyProperty ButtonSourceColorProperty = DependencyProperty.Register(nameof(ButtonSourceColor), typeof(Brush), typeof(MaskedToggleMenu), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush ButtonSourceColor
        {
            get => (Brush)GetValue(ButtonSourceColorProperty);
            set => SetValue(ButtonSourceColorProperty, value);
        }

        public MaskedToggleMenu()
        {
            DefaultStyleKey = typeof(MaskedToggleMenu);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetCurrentValue(IsCheckedProperty, !IsChecked);
        }

        protected override void OnMenuChanged(OldNew<ContextMenu> input)
        {
            if (input.New != null)
            {
                input.New.Placement = PlacementMode.Bottom;
                input.New.PlacementTarget = this;
                input.New.Bind(ContextMenu.IsOpenProperty, nameof(MenuVisibility), this, BindingMode.TwoWay, new Converters.BooleanToVisibilityConverter());
            }
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetCurrentValue(MenuVisibilityProperty, Visibility.Visible);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_MaskedImage = Template.FindName(nameof(PART_MaskedImage), this) as MaskedImage;
            PART_MaskedImage.If(i => i != null, i => i.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown);
        }
    }
}