using Imagin.Common.Text;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class BulletView : UserControl
    {
        public static DependencyProperty OriginFillProperty = DependencyProperty.Register(nameof(Bullet), typeof(Bullets), typeof(BulletView), new FrameworkPropertyMetadata(Bullets.Square, FrameworkPropertyMetadataOptions.None));
        public Bullets Bullet
        {
            get => (Bullets)GetValue(OriginFillProperty);
            set => SetValue(OriginFillProperty, value);
        }

        public static DependencyProperty CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), typeof(BulletView), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None));
        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(BulletView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public BulletView()
        {
            InitializeComponent();
        }
    }
}