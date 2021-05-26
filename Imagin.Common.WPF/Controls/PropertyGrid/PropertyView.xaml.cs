using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class PropertyView : UserControl
    {
        public static DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(PropertyModel), typeof(PropertyView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public PropertyModel Property
        {
            get => (PropertyModel)GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        public static DependencyProperty ReferencePropertyStringFormatProperty = DependencyProperty.Register(nameof(ReferencePropertyStringFormat), typeof(string), typeof(PropertyView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string ReferencePropertyStringFormat
        {
            get => (string)GetValue(ReferencePropertyStringFormatProperty);
            set => SetValue(ReferencePropertyStringFormatProperty, value);
        }

        public static DependencyProperty ReferencePropertyTemplateProperty = DependencyProperty.Register(nameof(ReferencePropertyTemplate), typeof(DataTemplate), typeof(PropertyView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate ReferencePropertyTemplate
        {
            get => (DataTemplate)GetValue(ReferencePropertyTemplateProperty);
            set => SetValue(ReferencePropertyTemplateProperty, value);
        }

        public static DependencyProperty ReferencePropertyTemplateSelectorProperty = DependencyProperty.Register(nameof(ReferencePropertyTemplateSelector), typeof(DataTemplateSelector), typeof(PropertyView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplateSelector ReferencePropertyTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ReferencePropertyTemplateSelectorProperty);
            set => SetValue(ReferencePropertyTemplateSelectorProperty, value);
        }

        public PropertyView() => InitializeComponent();
    }
}