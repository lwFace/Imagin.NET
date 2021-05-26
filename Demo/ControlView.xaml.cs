using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Demo
{
    public class ControlCollection : Imagin.Common.Collections.Generic.Collection<Control> { }

    [ContentProperty("Controls")]
    public partial class ControlView : UserControl
    {
        public static DependencyProperty ControlsProperty = DependencyProperty.Register(nameof(Controls), typeof(ControlCollection), typeof(ControlView), new FrameworkPropertyMetadata(null));
        public ControlCollection Controls
        {
            get => (ControlCollection)GetValue(ControlsProperty);
            set => SetValue(ControlsProperty, value);
        }

        public static DependencyProperty CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), typeof(ControlView), new FrameworkPropertyMetadata(0));
        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public static DependencyProperty SelectedControlProperty = DependencyProperty.Register(nameof(SelectedControl), typeof(Control), typeof(ControlView), new FrameworkPropertyMetadata(default(Control)));
        public Control SelectedControl
        {
            get => (Control)GetValue(SelectedControlProperty);
            set => SetValue(SelectedControlProperty, value);
        }
        
        public ControlView() : base()
        {
            SetCurrentValue(ControlsProperty, new ControlCollection());
            Controls.Changed += OnItemsChanged;
            InitializeComponent();
        }

        void OnItemsChanged(object sender)
        {
            SetCurrentValue(CountProperty, Controls.Count);
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(SelectedControlProperty, Controls.FirstOrDefault());
        }
    }
}