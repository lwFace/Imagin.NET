using System.Windows.Controls;

namespace Alarm.Linq
{
    public static class TextBoxExtensions
    {
        public static void End(this TextBox textBox)
        {
            textBox.Focus();
            textBox.Select(textBox.Text.Length, 0);
        }
    }
}