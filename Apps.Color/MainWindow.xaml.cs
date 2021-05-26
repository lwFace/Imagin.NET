using Imagin.Common;
using Imagin.Common.Controls;

namespace Color
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            Get.Current<MainViewModel>().View = this;
            InitializeComponent();
        }
    }
}