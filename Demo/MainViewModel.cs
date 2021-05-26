using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace Demo
{
    public class MainViewModel : MainViewModel<MainWindow, Document>
    {
        public MainViewModel() : base() { }

        public override IEnumerable<Panel> Load()
        {
            yield return new ControlsPanel();
            yield return new InheritsPanel();
            yield return new OptionsPanel();
            yield return new PropertiesPanel();
        }

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());
    }
}