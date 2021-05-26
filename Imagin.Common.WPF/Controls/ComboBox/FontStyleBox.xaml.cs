using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class FontStyleBox : ComboBox
    {
        public static DependencyProperty ShowPreviewProperty = DependencyProperty.Register(nameof(ShowPreview), typeof(bool), typeof(FontStyleBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowPreview
        {
            get => (bool)GetValue(ShowPreviewProperty);
            set => SetValue(ShowPreviewProperty, value);
        }

        public FontStyleBox()
        {
            InitializeComponent();

            var fontStyles = new ObservableCollection<FontStyle>();
            fontStyles.Add(FontStyles.Italic);
            fontStyles.Add(FontStyles.Normal);
            fontStyles.Add(FontStyles.Oblique);
            ItemsSource = fontStyles;
        }
    }
}