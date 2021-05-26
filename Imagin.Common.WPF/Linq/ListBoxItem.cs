using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class ListBoxItemExtensions
    {
        #region IsEditable

        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.RegisterAttached("IsEditable", typeof(bool), typeof(ListBoxItemExtensions), new UIPropertyMetadata(false, OnIsEditableChanged));
        public static bool GetIsEditable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEditableProperty);
        }
        public static void SetIsEditable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEditableProperty, value);
        }
        static void OnIsEditableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var ListBoxItem = sender as ListBoxItem;
            if (ListBoxItem != null && (bool)e.NewValue)
            {
                ListBoxItem.PreviewMouseDown += ListBoxItem_PreviewMouseDown;
            }
            else
            {
                ListBoxItem.PreviewMouseDown -= ListBoxItem_PreviewMouseDown;
            }
        }

        static void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var Item = sender as ListBoxItem;
            
            if (Item.IsSelected && Item.DataContext is IEditable && !(Item.DataContext as IEditable).IsEditable)
                (Item.DataContext as IEditable).IsEditable = true;
        }

        #endregion

        #region LastSelected

        public static readonly DependencyProperty LastSelectedProperty = DependencyProperty.RegisterAttached("LastSelected", typeof(bool), typeof(ListBoxItemExtensions), new PropertyMetadata(false));
        public static bool GetLastSelected(ListBoxItem i) => (bool)i.GetValue(LastSelectedProperty);
        public static void SetLastSelected(ListBoxItem i, bool value) => i.SetValue(LastSelectedProperty, value);

        #endregion
    }
}