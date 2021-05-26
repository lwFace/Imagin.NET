using System;
using System.Windows;
using System.Windows.Markup;

namespace Demo
{
    [ContentProperty(nameof(Instance))]
    public class Control : DependencyObject
    {
        public static DependencyProperty InstanceProperty = DependencyProperty.Register(nameof(Instance), typeof(object), typeof(Control), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object Instance
        {
            get => GetValue(InstanceProperty);
            set => SetValue(InstanceProperty, value);
        }

        public static DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(Type), typeof(Control), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Type Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public Control() : base() { }
    }
}