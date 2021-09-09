using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Media;
using Imagin.Common.Media.Models;
using Imagin.Common.Models;
using System.Windows.Input;

namespace Paint
{
    public class ColorPanel : Panel
    {
        Components component = Components.A;
        public Components Component
        {
            get => component;
            set => this.Change(ref component, value);
        }

        ColorConverter models;
        public ColorConverter Models
        {
            get => models;
            set => this.Change(ref models, value);
        }

        Imagin.Common.Media.Models.Model selectedModel;
        public Imagin.Common.Media.Models.Model SelectedModel
        {
            get => selectedModel;
            set => this.Change(ref selectedModel, value);
        }
         
        double value = 1;
        public double Value
        {
            get => value;
            set => this.Change(ref this.value, value);
        }

        public override string Title => "Color";

        public ColorPanel() : base(Resources.Uri(nameof(Paint), "/Images/Palette.png")) { }

        ICommand changeComponentCommand;
        public ICommand ChangeComponentCommand
        {
            get
            {
                changeComponentCommand = changeComponentCommand ?? new RelayCommand<Components>(i => Component = i, i => true);
                return changeComponentCommand;
            }
        }

        ICommand changeModelCommand;
        public ICommand ChangeModelCommand
        {
            get
            {
                changeModelCommand = changeModelCommand ?? new RelayCommand<Imagin.Common.Media.Models.Model>(i => { }, i => true);
                return changeModelCommand;
            }
        }
    }
}