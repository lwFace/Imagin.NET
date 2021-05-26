using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    public static class ComboBoxExtensions
    {
        #region Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(ComboBoxExtensions), new PropertyMetadata(string.Empty));
        public static string GetPlaceholder(ComboBox i) => (string)i.GetValue(PlaceholderProperty);
        public static void SetPlaceholder(ComboBox i, string value) => i.SetValue(PlaceholderProperty, value);

        #endregion

        #region SelectedItemTemplate

        public static readonly DependencyProperty SelectedItemTemplateProperty = DependencyProperty.RegisterAttached("SelectedItemTemplate", typeof(DataTemplate), typeof(ComboBoxExtensions), new PropertyMetadata(default(DataTemplate)));
        public static void SetSelectedItemTemplate(ComboBox i, DataTemplate value) => i.SetValue(SelectedItemTemplateProperty, value);
        public static DataTemplate GetSelectedItemTemplate(ComboBox i) => (DataTemplate)i.GetValue(SelectedItemTemplateProperty);

        #endregion

        #region SelectedItemTemplateSelector

        public static readonly DependencyProperty SelectedItemTemplateSelectorProperty = DependencyProperty.RegisterAttached("SelectedItemTemplateSelector", typeof(DataTemplateSelector), typeof(ComboBoxExtensions), new PropertyMetadata(default(DataTemplateSelector)));
        public static void SetSelectedItemTemplateSelector(ComboBox i, DataTemplateSelector value) => i.SetValue(SelectedItemTemplateSelectorProperty, value);
        public static DataTemplateSelector GetSelectedItemTemplateSelector(ComboBox i) => (DataTemplateSelector)i.GetValue(SelectedItemTemplateSelectorProperty);
        
        #endregion
    }
}