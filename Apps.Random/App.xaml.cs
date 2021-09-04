using Imagin.Common.Configuration;
using Imagin.Common.Models;
using System;

namespace Random
{
    public partial class App : SingleApplication
    {
        public override DataProperties Data => new DataProperties<Options>(DataFolders.Documents, "Options", "data");

        public override Func<IMainViewModel> MainViewModel => () => new MainViewModel();

        public override string Name => nameof(Random);

        [STAThread]
        public static void Main(params string[] Arguments)
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(nameof(Random)))
            {
                var App = new App();
                App.InitializeComponent();
                App.Run();

                SingleInstance<App>.Cleanup();
            }
        }
    }
}