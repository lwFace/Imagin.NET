using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    public static class ListBoxExtensions
    {
        #region LastSelected

        internal static readonly DependencyProperty LastSelectedProperty = DependencyProperty.RegisterAttached("LastSelected", typeof(ListBoxItem), typeof(ListBoxExtensions), new PropertyMetadata(null));
        internal static ListBoxItem GetLastSelected(ListBox i) => (ListBoxItem)i.GetValue(LastSelectedProperty);
        internal static void SetLastSelected(ListBox i, ListBoxItem value) => i.SetValue(LastSelectedProperty, value);

        public static readonly DependencyProperty EnableLastSelectedProperty = DependencyProperty.RegisterAttached("EnableLastSelected", typeof(bool), typeof(ListBoxExtensions), new PropertyMetadata(false, OnEnableLastSelectedChanged));
        public static bool GetEnableLastSelected(ListBox i) => (bool)i.GetValue(EnableLastSelectedProperty);
        public static void SetEnableLastSelected(ListBox i, bool value) => i.SetValue(EnableLastSelectedProperty, value);
        static void OnEnableLastSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            listBox.SelectionChanged -= EnableLastSelected_OnSelectionChanged;
            if ((bool)e.NewValue)
                listBox.SelectionChanged += EnableLastSelected_OnSelectionChanged;
        }

        static void EnableLastSelected_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedItems.Count > 0)
            {
                foreach (var i in listBox.Items)
                {
                    if (listBox.ItemContainerGenerator.ContainerFromItem(i) is ListBoxItem j)
                        ListBoxItemExtensions.SetLastSelected(j, false);
                }

                var last = e.AddedItems?.Last();
                if (listBox.ItemContainerGenerator.ContainerFromItem(last) is ListBoxItem k)
                    SetLastSelected(listBox, k);
            }
            else
            {
                var lastSelected = GetLastSelected(listBox);
                foreach (var i in listBox.Items)
                {
                    if (listBox.ItemContainerGenerator.ContainerFromItem(i) is ListBoxItem j)
                        ListBoxItemExtensions.SetLastSelected(j, j.Equals(lastSelected));
                }
            }
        }

        #endregion
    }
}