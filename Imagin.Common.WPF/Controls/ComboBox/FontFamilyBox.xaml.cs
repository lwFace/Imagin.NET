using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class FontFamilyBox : ComboBox
    {
        public static DependencyProperty ShowPreviewProperty = DependencyProperty.Register(nameof(ShowPreview), typeof(bool), typeof(FontFamilyBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ShowPreview
        {
            get => (bool)GetValue(ShowPreviewProperty);
            set => SetValue(ShowPreviewProperty, value);
        }

        public FontFamilyBox()
        {
            InitializeComponent();
        }
    }
}