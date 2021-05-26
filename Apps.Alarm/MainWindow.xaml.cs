using Imagin.Common;
using Imagin.Common.Controls;
using System.ComponentModel;

namespace Alarm
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            Get.Current<MainViewModel>().View = this;
            InitializeComponent();
        }

        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !Get.Current<MainViewModel>().Solve();
        }
    }
}