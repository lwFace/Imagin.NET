using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System.Linq;
using System.Windows.Input;

namespace Vault
{
    public class LogPanel : Panel
    {
        Log log;
        public Log Log
        {
            get => log;
            set => this.Change(ref log, value);
        }

        public override string Title => "Log";

        public LogPanel() : base(Resources.Uri(nameof(Vault), "/Images/File.png"))
        {
            Get.Current<MainViewModel>().SelectedItemsChanged += OnSelectedItemsChanged;
        }

        void OnSelectedItemsChanged(object sender, EventArgs<System.Collections.IList> e)
        {
            if (e.Value?.Count > 0)
                Log = e.Value.First<CopyTask>()?.Log;
        }

        ICommand clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                clearCommand = clearCommand ?? new RelayCommand(() => Log.Clear(), () => Log?.Count > 0);
                return clearCommand;
            }
        }
    }
}