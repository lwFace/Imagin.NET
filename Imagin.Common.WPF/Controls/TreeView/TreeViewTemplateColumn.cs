using System.Windows;

namespace Imagin.Common.Controls
{
    public class TreeViewTemplateColumn : TreeViewColumn
    {
        public static DependencyProperty TemplateProperty = DependencyProperty.Register(nameof(Template), typeof(DataTemplate), typeof(TreeViewTemplateColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate Template
        {
            get => (DataTemplate)GetValue(TemplateProperty);
            set => SetValue(TemplateProperty, value);
        }

        public TreeViewTemplateColumn() : base() { }
    }
}