using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Notes
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            Get.Current<MainViewModel>().View = this;
            InitializeComponent();
        }

        void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var file = sender.To<FrameworkElement>().DataContext.To<Imagin.Common.Storage.File>();
            var fileInfo = new FileInfo(file.Path);

            var point = (sender as Visual).PointToScreen(e.GetPosition(sender as IInputElement));
            ShellContextMenu.Show(new System.Drawing.Point(point.X.Int32(), point.Y.Int32()), fileInfo);
        }

        void OnRefreshed(object sender, RoutedEventArgs e)
        {
            Get.Current<MainViewModel>().Panel<NotesPanel>().StorageView.Refresh();
        }
    }
}