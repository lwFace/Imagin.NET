using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class UnsignedUpDown<T> : RationalUpDown<T>
    {
        public UnsignedUpDown() : base() { }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = !Expression.IsMatch(e.Text);
        }
    }
}