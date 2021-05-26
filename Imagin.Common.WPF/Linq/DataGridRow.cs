using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    public static class DataGridRowExtensions
    {
        #region SelectionVisibility

        public static readonly DependencyProperty SelectionVisibilityProperty = DependencyProperty.RegisterAttached("SelectionVisibility", typeof(Visibility), typeof(DataGridRowExtensions), new PropertyMetadata(Visibility.Collapsed));
        public static Visibility GetSelectionVisibility(DataGridRow i) => (Visibility)i.GetValue(SelectionVisibilityProperty);
        public static void SetSelectionVisibility(DataGridRow i, Visibility value) => i.SetValue(SelectionVisibilityProperty, value);

        #endregion
    }
}