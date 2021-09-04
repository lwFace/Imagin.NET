using Imagin.Common.Configuration;
using Imagin.Common.Models;
using System;

namespace Rename
{
    public partial class App : SingleApplication
    {
        public override DataProperties Data => new DataProperties<Options>(DataFolders.Documents, "Options", "data");

        public override Func<IMainViewModel> MainViewModel => () => new MainViewModel();

        public override string Name => nameof(Rename);

        [STAThread]
        public static void Main(params string[] Arguments)
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(nameof(Rename)))
            {
                var App = new App();
                App.InitializeComponent();
                App.Run();

                SingleInstance<App>.Cleanup();
            }
        }
    }
}