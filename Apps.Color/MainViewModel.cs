using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System.Windows.Input;
using System.Windows.Media;

namespace Color
{
    public class MainViewModel : MainViewModel<MainWindow>
    {
        DocumentCollection documents = new DocumentCollection();
        public DocumentCollection Documents
        {
            get => documents;
            set => this.Change(ref documents, value);
        }

        /// ........................................................................

        public MainViewModel() : base() { }

        /// ........................................................................

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                addCommand = addCommand ?? new RelayCommand(() => Documents.Add(new ColorDocument(Colors.White)));
                return addCommand;
            }
        }
    }
}