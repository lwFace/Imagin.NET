using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Media;
using System.Windows.Input;

namespace Paint
{
    public partial class NewWindow : BaseWindow
    {
        public uint Result { get; private set; }

        DocumentPreset documentPreset;
        public DocumentPreset DocumentPreset
        {
            get => documentPreset;
            private set => this.Change(ref documentPreset, value);
        }

        DocumentPreset selectedPreset;
        public DocumentPreset SelectedPreset
        {
            get => selectedPreset;
            set => this.Change(ref selectedPreset, value);
        }

        GraphicalUnit unit = GraphicalUnit.Pixel;
        public GraphicalUnit Unit
        {
            get => unit;
            set => this.Change(ref unit, value);
        }

        public NewWindow() : base()
        {
            InitializeComponent();
            DocumentPreset = new DocumentPreset("Untitled");
        }

        void OnCancel(object sender, System.Windows.RoutedEventArgs e)
        {
            Result = 0;
            Close();
        }

        void OnCreate(object sender, System.Windows.RoutedEventArgs e)
        {
            Result = 1;
            Close();
        }

        void OnPresetChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (selectedPreset != null)
                DocumentPreset = selectedPreset;
        }

        ICommand deletePresetCommand;
        public ICommand DeletePresetCommand
        {
            get
            {
                deletePresetCommand = deletePresetCommand ?? new RelayCommand(() => Get.Current<Options>().DocumentPresets.Remove(DocumentPreset), () => DocumentPreset != null);
                return deletePresetCommand;
            }
        }

        ICommand savePresetCommand;
        public ICommand SavePresetCommand
        {
            get
            {
                savePresetCommand = savePresetCommand ?? new RelayCommand(() => Get.Current<Options>().DocumentPresets.Add(DocumentPreset.Clone()), () => DocumentPreset != null);
                return savePresetCommand;
            }
        }
    }
}