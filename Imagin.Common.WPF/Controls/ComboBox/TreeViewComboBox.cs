using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Content), Type = typeof(ContentControl))]
    [TemplatePart(Name = nameof(PART_TreeView), Type = typeof(TreeView))]
    public class TreeViewComboBox : ComboBox
    {
        public event EventHandler<EventArgs<object>> SelectedItemChanged;

        Handle handleSelectedItem = false;

        Handle handleSelection = false;

        ContentControl PART_Content;

        TreeView PART_TreeView;
        
        public static DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(TreeViewComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public TreeViewComboBox() : base()
        {
            DefaultStyleKey = typeof(TreeViewComboBox);
            SelectionChanged += OnSelectionChanged;
        }

        protected virtual void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!handleSelection && e.AddedItems.Count > 0)
            {
                var i = e.AddedItems[0];

                handleSelectedItem = true;
                PART_TreeView.Select(i);

                if (PART_Content != null)
                    PART_Content.Content = i;

                handleSelectedItem = false;
            }
        }

        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!handleSelectedItem && !e.Handled)
            {
                e.Handled = true;

                handleSelection = true;
                SelectedItem = e.NewValue;
                handleSelection = false;

                PART_Content.Content = e.NewValue;

                //IsDropDownOpen = false;
            }

            SelectedItemChanged?.Invoke(this, new EventArgs<object>(e.NewValue));
        }

        public override void OnApplyTemplate()
        {
            PART_Content = Template.FindName(nameof(PART_Content), this) as ContentControl;
            PART_Content.Content = SelectedItem;

            PART_TreeView = Template.FindName(nameof(PART_TreeView), this) as TreeView;
            if (PART_TreeView != null)
            {
                PART_TreeView.Resources = Resources;
                PART_TreeView.SelectedItemChanged += OnSelectedItemChanged;
            }
        }
    }
}