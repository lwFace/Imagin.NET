using Imagin.Common;
using Imagin.Common.Controls;
using System.ComponentModel;

namespace Vault
{
    public partial class MainWindow : BaseWindow
    {
        bool exit = false;

        public MainWindow()
        {
            Get.Current<MainViewModel>().View = this;
            InitializeComponent();
        }

        void OnExit(object sender, System.Windows.RoutedEventArgs e)
        {
            exit = true;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (Get.Current<Options>().ShowInTaskBar)
            {
                if (!exit)
                {
                    e.Cancel = true;
                    Get.Current<MainViewModel>().Hide();
                }
            }
        }
    }
}