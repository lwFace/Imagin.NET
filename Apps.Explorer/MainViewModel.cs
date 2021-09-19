using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Globalization.Engine;
using Imagin.Common.Globalization.Extensions;
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
                if (ActiveDocument == null)
                    return $"{LocExtension.GetLocalizedValue(typeof(string), nameof(Explorer), LocalizeDictionary.Instance.SpecificCulture, null)}";

                return Machine.FriendlyName(ActiveDocument.Path);
            }
        }

        public string NewPath
        {
            set
            {
                if (Documents.Count > 0)
                {
                    ActiveDocument.Path = value;
                    return;
                }
                Documents.Add(new ExplorerDocument(value));
            }
        }

        /// ........................................................................

        public MainViewModel() : base() 
        {
            Get.Current<Options>().PropertyChanged += OnOptionsChanged;
        }

        private void OnOptionsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Options.Language):
                    this.Changed(() => Title);
                    break;
            }
        }

        /// ........................................................................

        protected override void OnActiveDocumentChanged(ExplorerDocument i)
        {
            base.OnActiveDocumentChanged(i);
            this.Changed(() => Title);
        }

        public override IEnumerable<Panel> Load()
        {
            yield return new FavoritesPanel();
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