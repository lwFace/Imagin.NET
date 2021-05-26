using Imagin.Common;
using Imagin.Common.Controls;

namespace Paint
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow() : base()
        {
            Get.Current<MainViewModel>().View = this;
            InitializeComponent();
        }
    }
}