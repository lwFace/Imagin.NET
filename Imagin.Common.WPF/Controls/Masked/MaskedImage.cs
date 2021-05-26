using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Image), Type = typeof(Rectangle))]
    public class MaskedImage : UserControl
    {
        Rectangle PART_Image;

        public static DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(MaskedImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnSourceChanged));
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        static void OnSourceChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<MaskedImage>().OnSourceChanged(new OldNew<ImageSource>(e));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(MaskedImage), new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.None, OnColorChanged));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static void OnColorChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<MaskedImage>().OnColorChanged(new OldNew<Color>(e));

        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register(nameof(SourceColor), typeof(Brush), typeof(MaskedImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Brush SourceColor
        {
            get => (Brush)GetValue(SourceColorProperty);
            set => SetValue(SourceColorProperty, value);
        }

        public static DependencyProperty SourceHeightProperty = DependencyProperty.Register(nameof(SourceHeight), typeof(double), typeof(MaskedImage), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.None));
        public double SourceHeight
        {
            get => (double)GetValue(SourceHeightProperty);
            set => SetValue(SourceHeightProperty, value);
        }

        public static DependencyProperty SourceWidthProperty = DependencyProperty.Register(nameof(SourceWidth), typeof(double), typeof(MaskedImage), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.None));
        public double SourceWidth
        {
            get => (double)GetValue(SourceWidthProperty);
            set => SetValue(SourceWidthProperty, value);
        }

        public MaskedImage() => DefaultStyleKey = typeof(MaskedImage);

        protected virtual void OnColorChanged(OldNew<Color> input) => SetCurrentValue(SourceColorProperty, new SolidColorBrush(input.New));

        protected virtual void OnSourceChanged(OldNew<ImageSource> input) => PART_Image.If(i => i != null, i => i.OpacityMask = new ImageBrush(input.New));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Image = Template.FindName(nameof(PART_Image), this) as Rectangle;
            PART_Image.OpacityMask = new ImageBrush(Source);
        }
    }
}