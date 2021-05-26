using Imagin.Common.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class RegexBox : BaseRegexBox
    {
        protected override Regex regex
        {
            get
            {
                return new Regex(Pattern);
            }
        }

        public static DependencyProperty PatternProperty = DependencyProperty.Register(nameof(Pattern), typeof(string), typeof(RegexBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Pattern
        {
            get => (string)GetValue(PatternProperty);
            set => SetValue(PatternProperty, value);
        }

        public RegexBox() : base() { }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !Pattern.NullOrEmpty() && !regex.IsMatch(e.Text);
        }
    }
}