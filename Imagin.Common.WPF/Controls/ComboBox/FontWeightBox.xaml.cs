using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class FontWeightBox : ComboBox
    {
        public static DependencyProperty ShowPreviewProperty = DependencyProperty.Register(nameof(ShowPreview), typeof(bool), typeof(FontWeightBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowPreview
        {
            get => (bool)GetValue(ShowPreviewProperty);
            set => SetValue(ShowPreviewProperty, value);
        }

        public FontWeightBox()
        {
            InitializeComponent();
            var result = new ObservableCollection<FontWeight>();
            result.Add(FontWeights.Black);
            result.Add(FontWeights.Bold);
            result.Add(FontWeights.DemiBold);
            result.Add(FontWeights.ExtraBlack);
            result.Add(FontWeights.ExtraBold);
            result.Add(FontWeights.ExtraLight);
            result.Add(FontWeights.Heavy);
            result.Add(FontWeights.Light);
            result.Add(FontWeights.Medium);
            result.Add(FontWeights.Normal);
            result.Add(FontWeights.Regular);
            result.Add(FontWeights.SemiBold);
            result.Add(FontWeights.Thin);
            result.Add(FontWeights.UltraBlack);
            result.Add(FontWeights.UltraBold);
            result.Add(FontWeights.UltraLight);
            SetCurrentValue(ItemsSourceProperty, result);
        }
    }
}