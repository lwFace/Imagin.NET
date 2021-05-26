using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Paint
{
    public partial class ToolButton : UserControl
    {
        public static readonly DependencyProperty ArrowVisibilityProperty = DependencyProperty.Register(nameof(ArrowVisibility), typeof(Visibility), typeof(ToolButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility ArrowVisibility
        {
            get => (Visibility)GetValue(ArrowVisibilityProperty);
            set => SetValue(ArrowVisibilityProperty, value);
        }

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), typeof(ToolButton), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnCountChanged));
        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }
        static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ToolButton).ArrowVisibility = (int)e.NewValue > 1 ? Visibility.Visible : Visibility.Collapsed;

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ToolButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(ToolButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }
        
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(ToolButton), new FrameworkPropertyMetadata(default(IList), FrameworkPropertyMetadataOptions.None));
        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ToolButton), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty SelectedItemTemplateProperty = DependencyProperty.Register(nameof(SelectedItemTemplate), typeof(DataTemplate), typeof(ToolButton), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None));
        public DataTemplate SelectedItemTemplate
        {
            get => (DataTemplate)GetValue(SelectedItemTemplateProperty);
            set => SetValue(SelectedItemTemplateProperty, value);
        }
        
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ToolButton), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.None));
        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public ToolButton()
        {
            InitializeComponent();
            PART_Popup.PlacementTarget = this;
        }

        void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetCurrentValue(IsCheckedProperty, true);
        }

        void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Count > 1)
            {
                SetCurrentValue(IsOpenProperty, !IsOpen);
            }
        }

        void ComboBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var tool = (Tool)(sender as ComboBoxItem).Content;
            tool.IsSelected = true;

            SetCurrentValue(SelectedItemProperty, tool);
            SetCurrentValue(IsOpenProperty, false);
        }

        void PART_Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            SetCurrentValue(IsOpenProperty, false);
        }
    }
}