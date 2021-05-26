using System.Windows;

namespace Imagin.Common.Controls
{
    public partial class InputWindow : BaseWindow
    {
        public const string DefaultCancelButtonLabel = "Cancel";

        public const string DefaultSaveButtonLabel = "Save";
        
        string result = string.Empty;
        public string Result
        {
            get => result;
            set => this.Change(ref result, value); 
        }

        public static DependencyProperty CancelButtonLabelProperty = DependencyProperty.Register(nameof(CancelButtonLabel), typeof(string), typeof(InputWindow), new FrameworkPropertyMetadata(DefaultCancelButtonLabel, FrameworkPropertyMetadataOptions.None));
        public string CancelButtonLabel
        {
            get => (string)GetValue(CancelButtonLabelProperty);
            set => SetValue(CancelButtonLabelProperty, value);
        }

        public static DependencyProperty SaveButtonLabelProperty = DependencyProperty.Register(nameof(SaveButtonLabel), typeof(string), typeof(InputWindow), new FrameworkPropertyMetadata(DefaultSaveButtonLabel, FrameworkPropertyMetadataOptions.None));
        public string SaveButtonLabel
        {
            get => (string)GetValue(SaveButtonLabelProperty);
            set => SetValue(SaveButtonLabelProperty, value);
        }

        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(InputWindow), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public InputWindow()
        {
            InitializeComponent();
        }

        void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void Cancel(object sender, RoutedEventArgs e)
        {
            Result = string.Empty;
            Close();
        }
    }
}