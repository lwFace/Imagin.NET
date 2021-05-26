using Imagin.Common.Configuration;
using Imagin.Common.Models;
using System;

namespace Notes
{
    public partial class App : SingleApplication
    {
        public override DataProperties Data => new DataProperties<Options>(DataFolders.Documents, "Options", "data");

        public override string Name => nameof(Notes);

        public override Func<IMainViewModel> MainViewModel => () => new MainViewModel();

        [STAThread]
        public static void Main(params string[] Arguments)
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(nameof(Notes)))
            {
                var App = new App();
                App.InitializeComponent();
                App.Run();

                SingleInstance<App>.Cleanup();
            }
        }
    }
}