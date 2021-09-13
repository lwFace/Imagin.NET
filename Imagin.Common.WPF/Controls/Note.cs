using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class Note : System.Windows.Controls.UserControl
    {
        public static DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(Note), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Note() => DefaultStyleKey = typeof(Note);
    }
}