using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class TreeViewTextColumn : TreeViewColumn
    {
        public static DependencyProperty ConverterProperty = DependencyProperty.Register(nameof(Converter), typeof(IValueConverter), typeof(TreeViewColumn), new FrameworkPropertyMetadata(default(IValueConverter), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public static DependencyProperty MemberPathProperty = DependencyProperty.Register(nameof(MemberPath), typeof(string), typeof(TreeViewTextColumn), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));
        public string MemberPath
        {
            get => (string)GetValue(MemberPathProperty);
            set => SetValue(MemberPathProperty, value);
        }

        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register(nameof(TextTrimming), typeof(TextTrimming), typeof(TreeViewTextColumn), new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }
        
        public TreeViewTextColumn() : base() { }
    }
}