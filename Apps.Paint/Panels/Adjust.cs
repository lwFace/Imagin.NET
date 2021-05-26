using Imagin.Common;
using Imagin.Common.AvalonDock;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Paint.Adjust;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Paint
{
    public class AdjustViewModel : Pane
    {
        public class ViewModel : BaseObject
        {
            AdjustmentEffectCollection adjustments;
            public AdjustmentEffectCollection Adjustments
            {
                get => adjustments;
                set => this.Change(ref adjustments, value);
            }

            public ViewModel(AdjustmentEffectCollection adjustments)
            {
                Adjustments = adjustments;
            }

            public override string ToString() => "Adjust";
        }

        ViewModel selectedAdjustments;
        public ViewModel SelectedAdjustments
        {
            get => selectedAdjustments;
            set => this.Change(ref selectedAdjustments, value);
        }

        VisualLayer selectedLayer;
        public VisualLayer SelectedLayer
        {
            get => selectedLayer;
            set => this.Change(ref selectedLayer, value);
        }

        public AdjustViewModel() : base("AdjustPane", "Adjust", Resources.Uri(nameof(Paint), "/Images/Adjust.png"))
        {

            MainViewModel.Current.LayerSelected += (s, e) => SelectedLayer = e.Value is VisualLayer ? (VisualLayer)e.Value : null;
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedLayer):

                    if (selectedLayer != null)
                        SelectedAdjustments = new ViewModel(selectedLayer.Adjustments);

                    break;
            }
        }

        ICommand deleteAdjustmentCommand;
        public ICommand DeleteAdjustmentCommand
        {
            get
            {
                deleteAdjustmentCommand = deleteAdjustmentCommand ?? new RelayCommand<AdjustmentEffect>(i =>
                {
                    var layer = SelectedLayer as VisualLayer;
                    layer.Adjustments.RemoveAt(layer.Adjustments.IndexOf(layer.Adjustments.First(j => j.Index == i.Index)));
                },
                i => SelectedLayer is VisualLayer);
                return deleteAdjustmentCommand;
            }
        }
    }
}