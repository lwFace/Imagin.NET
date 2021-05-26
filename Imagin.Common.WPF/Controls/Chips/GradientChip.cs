using Imagin.Common.Linq;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// A chip for displaying and selecting a <see cref="LinearGradientBrush"/>.
    /// </summary>
    public class GradientChip : Chip<LinearGradientBrush>
    {
        public GradientChip() : base() { }

        public override bool? ShowDialog()
        {
            var dialog = new GradientDialog(Title, (LinearGradientBrush)Value.As<Brush>().Duplicate(), this);
            var result = dialog.ShowDialog();

            if (result != null && dialog.Saved)
                Value = dialog.Value;

            return result;
        }
    }
}