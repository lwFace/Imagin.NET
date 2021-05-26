using Imagin.Common;
using Imagin.Common.Controls;

namespace Desktop
{
    public partial class OptionsWindow : BaseWindow
    {
        public OptionsWindow()
        {
            InitializeComponent();
            DataContext = Get.Current<MainViewModel>();
        }
    }
}