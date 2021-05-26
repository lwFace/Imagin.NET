using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Controls
{
    public sealed partial class Dialog : BaseWindow
    {
        int result = 0;
        public int Result
        {
            get => result;
            set => result = value;
        }

        public static DependencyProperty ActionsProperty = DependencyProperty.Register(nameof(Actions), typeof(ObservableCollection<DialogButton>), typeof(Dialog), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ObservableCollection<DialogButton> Actions
        {
            get => (ObservableCollection<DialogButton>)GetValue(ActionsProperty);
            set => SetValue(ActionsProperty, value);
        }

        public static DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(Uri), typeof(Dialog), new FrameworkPropertyMetadata(DialogImage.Information, FrameworkPropertyMetadataOptions.None));
        public Uri Image
        {
            get => (Uri)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static DependencyProperty ImageHeightProperty = DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(Dialog), new FrameworkPropertyMetadata(64.0, FrameworkPropertyMetadataOptions.None));
        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public static DependencyProperty ImageWidthProperty = DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(Dialog), new FrameworkPropertyMetadata(64.0, FrameworkPropertyMetadataOptions.None));
        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public static DependencyProperty NeverShowProperty = DependencyProperty.Register(nameof(NeverShow), typeof(GetSetBoolean), typeof(Dialog), new FrameworkPropertyMetadata(default(GetSetBoolean), FrameworkPropertyMetadataOptions.None));
        public GetSetBoolean NeverShow
        {
            get => (GetSetBoolean)GetValue(NeverShowProperty);
            set => SetValue(NeverShowProperty, value);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(object), typeof(Dialog), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        Dialog() : base()
        {
            SetCurrentValue(ActionsProperty, new ObservableCollection<DialogButton>());
            InitializeComponent();
        }

        public static int Show(string title, object text, Uri image, params DialogButton[] buttons)
        {
            var dialog = new Dialog();
            dialog.SetCurrentValue(TitleProperty, title);

            if (text is string)
                text = new TextBlock() { Text = text.ToString(), TextWrapping = TextWrapping.Wrap };

            dialog.SetCurrentValue(TextProperty, text);
            dialog.SetCurrentValue(ImageProperty, image);
            buttons?.ForEach(i => dialog.Actions.Add(i));

            dialog.ShowDialog();
            return dialog.Result;
        }

        async public static Task<int> Show(string title, object text, Uri image, DialogAction action, params DialogButton[] buttons)
        {
            var result = Show(title, text, image, buttons);
            if (action != null)
                await action(result);

            return result;
        }

        ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                closeCommand = closeCommand ?? new RelayCommand<DialogButton>(i =>
                {
                    Result = i.Id;
                    Close();
                    i.Action?.Invoke();
                }, 
                i => i is DialogButton);
                return closeCommand;
            }
        }
    }
}