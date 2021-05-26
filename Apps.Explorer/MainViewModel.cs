using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using System.Collections.Generic;
using System.Windows.Input;

namespace Explorer
{
    public class MainViewModel : MainViewModel<MainWindow, ExplorerDocument>
    {
        public override string Title
        {
            get
            {
                if (ActiveDocument != null)
                    return Machine.FriendlyName(ActiveDocument.Path);

                return nameof(Explorer);
            }
        }

        /// ........................................................................

        public MainViewModel() : base() { }

        /// ........................................................................

        protected override void OnActiveDocumentChanged(ExplorerDocument value)
        {
            base.OnActiveDocumentChanged(value);
            this.Changed(() => Title);
        }

        public override IEnumerable<Panel> Load()
        {
            yield return new OptionsPanel();
            yield return new PropertiesPanel();
            yield return new RenamePanel();
        }

        /// ........................................................................

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        ICommand addConsoleCommand;
        public ICommand AddConsoleCommand => addConsoleCommand = addConsoleCommand ?? new RelayCommand(() => Documents.Add(new ConsoleDocument()));

        ICommand addExplorerCommand;
        public ICommand AddExplorerCommand => addExplorerCommand = addExplorerCommand ?? new RelayCommand(() => Documents.Add(new ExplorerDocument()));
    }
}