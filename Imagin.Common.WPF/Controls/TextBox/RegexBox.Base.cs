using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class BaseRegexBox : TextBox
    {
        protected virtual Regex regex => default(Regex);

        public BaseRegexBox() : base() { }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
