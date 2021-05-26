using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class BaseWindow : Window, IPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(nameof(Buttons), typeof(FrameworkElementCollection), typeof(BaseWindow), new FrameworkPropertyMetadata(default(WindowButtonCollection), FrameworkPropertyMetadataOptions.None));
        public FrameworkElementCollection Buttons
        {
            get => (FrameworkElementCollection)GetValue(ButtonsProperty);
            set => SetValue(ButtonsProperty, value);
        }

        public static readonly DependencyProperty TitleIconProperty = DependencyProperty.Register(nameof(TitleIcon), typeof(ImageSource), typeof(BaseWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ImageSource TitleIcon
        {
            get => (ImageSource)GetValue(TitleIconProperty);
            set => SetValue(TitleIconProperty, value);
        }

        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof(TitleTemplate), typeof(DataTemplate), typeof(BaseWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate TitleTemplate
        {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }

        public BaseWindow() : base()
        {
            DefaultStyleKey = typeof(BaseWindow);
            SetCurrentValue(ButtonsProperty, new FrameworkElementCollection());
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        ICommand closeCommand;
        public ICommand CloseCommand => closeCommand = closeCommand ?? new RelayCommand(Close);
    }
}