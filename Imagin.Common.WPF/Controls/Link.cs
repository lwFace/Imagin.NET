using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Opens up a web page in the default browser when clicked.
    /// </summary>
    public partial class Link : Button
    {
        public static DependencyProperty TextDecorationsProperty = DependencyProperty.Register(nameof(TextDecorations), typeof(TextDecorationCollection), typeof(Link), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public TextDecorationCollection TextDecorations
        {
            get => (TextDecorationCollection)GetValue(TextDecorationsProperty);
            set => SetValue(TextDecorationsProperty, value);
        }

        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register(nameof(TextTrimming), typeof(TextTrimming), typeof(Link), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.None));
        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }

        public static DependencyProperty TextWrappingProperty = DependencyProperty.Register(nameof(TextWrapping), typeof(TextWrapping), typeof(Link), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.None));
        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty);
            set => SetValue(TextWrappingProperty, value);
        }

        public static DependencyProperty UriProperty = DependencyProperty.Register(nameof(Uri), typeof(string), typeof(Link), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public string Uri
        {
            get => (string)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }

        public Link() => DefaultStyleKey = typeof(Link);

        protected override void OnClick()
        {
            base.OnClick();
            try
            {
                Process.Start(Uri);
            }
            catch { }
        }
    }
}