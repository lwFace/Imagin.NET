using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class AboutWindow : BaseWindow
    {
        public static readonly DependencyProperty LogoProperty = DependencyProperty.Register(nameof(Logo), typeof(ImageSource), typeof(AboutWindow), new FrameworkPropertyMetadata(null));
        public ImageSource Logo
        {
            get => (ImageSource)GetValue(LogoProperty);
            set => SetValue(LogoProperty, value);
        }

        public AboutWindow()
        {
            InitializeComponent();
        }
    }
}
