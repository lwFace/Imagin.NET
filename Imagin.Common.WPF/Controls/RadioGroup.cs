using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class RadioGroup : ItemsControl
    {
        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RadioGroup), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.None));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(RadioGroup), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnGroupNameChanged));
        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }
        static void OnGroupNameChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            RadioGroup RadioGroup = (RadioGroup)Object;
            foreach (RadioButton r in RadioGroup.Items)
                r.GroupName = RadioGroup.GroupName;
        }

        public RadioGroup() : base() => DefaultStyleKey = typeof(RadioGroup);
    }
}