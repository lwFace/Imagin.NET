using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notes
{
    public partial class TextView : ResourceDictionary
    {
        public TextView() : base()
        {
            InitializeComponent();
        }

        /// ........................................................................

        void OnImagePreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is List.Line line)
            {
                var path = string.Empty;
                if (ExplorerWindow.Show(out path, "Select an image...", ExplorerWindow.Modes.OpenFile, Array<string>.New("jpg", "jpeg", "png"), line.Image.NullOrEmpty() ? Get.Current<MainViewModel>().Panel<NotesPanel>().LastImage : line.Image))
                    Get.Current<MainViewModel>().Panel<NotesPanel>().LastImage = line.Image = path;
            }
        }

        /// ........................................................................

        void OnTextBoxPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var textBox = (System.Windows.Controls.TextBox)sender;
                Get.Current<MainViewModel>().Panel<FindPanel>().OriginalSelection = new Region(Get.Current<MainViewModel>().ActiveDocument, textBox.SelectionStart, textBox.SelectionLength);
            }
        }

        /// ........................................................................

        void OnListLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender.To<System.Windows.Controls.TextBox>();

            var line = textBox.DataContext.As<List.Line>();
            if (Get.Current<Options>().ListDeleteEmptyLines)
            {
                if (textBox.Text.NullOrEmpty())
                {
                    line.DeleteCommand.Execute(null);
                    //The command above automatically refreshes the list so quit here!
                    return;
                }
            }

            line?.List.Refresh();
        }

        void OnListPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var line = (List.Line)((System.Windows.Controls.TextBox)sender).DataContext;
            if (e.Key == Key.Back)
            {
                if ((sender as System.Windows.Controls.TextBox).Text.NullOrEmpty())
                {
                    line.DeleteCommand.Execute(null);
                }
            }
        }
    }
}