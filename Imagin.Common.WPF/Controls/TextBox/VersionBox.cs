using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class VersionBox : ParseBox<Version>
    {
        public static DependencyProperty DelimiterProperty = DependencyProperty.Register(nameof(Delimiter), typeof(char), typeof(VersionBox), new FrameworkPropertyMetadata('.', FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public char Delimiter
        {
            get => (char)GetValue(DelimiterProperty);
            set => SetValue(DelimiterProperty, value);
        }

        public VersionBox() : base()
        {
        }

        protected override Version GetValue(string value)
        {
            return value.Version(Delimiter);
        }

        protected override string ToString(Version value)
        {
            return value?.ToString();
        }
    }
}