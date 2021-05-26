using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class SignedUpDown<T> : RationalUpDown<T>
    {
        public SignedUpDown() : base() { }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            switch (e.Text)
            {
                case Dash:
                    e.Handled = !(CaretIndex == 0 || !Text.Contains(Dash));
                    break;
                default:
                    e.Handled = !Expression.IsMatch(e.Text);
                    break;
            }
        }
    }
}