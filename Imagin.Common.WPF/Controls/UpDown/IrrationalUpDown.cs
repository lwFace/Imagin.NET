using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class IrrationalUpDown<T> : NumericUpDown<T>
    {
        public IrrationalUpDown() : base() { }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            switch (e.Text)
            {
                case Dash:
                    e.Handled = !(CaretIndex == 0 || !Text.Contains(Dash));
                    break;
                case Period:
                    e.Handled = !(CaretIndex > 0 || !Text.Contains(Period));
                    break;
                default:
                    e.Handled = !Expression.IsMatch(e.Text);
                    break;
            }
        }
    }
}