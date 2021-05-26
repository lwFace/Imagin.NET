using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Storage;

namespace Explorer
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            Get.Current<MainViewModel>().View = this;
            InitializeComponent();
        }

        void OnFileOpened(object sender, Imagin.Common.Input.EventArgs<string> e)
        {
            File.Long.Open(e.Value);
        }
    }
}