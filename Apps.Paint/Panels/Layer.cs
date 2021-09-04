using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Paint.Adjust;
using System.Collections;
using System.Linq;
using System.Windows.Input;

namespace Paint
{
    public class LayerPanel : Panel
    {
        IList layers;
        public IList Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        public override string Title => "Layer";

        public LayerPanel() : base(Resources.Uri(nameof(Paint), "/Images/Layer.png"))
        {
            Get.Current<MainViewModel>().ActiveContentChanged += OnActiveContentChanged;
            Get.Current<MainViewModel>().DocumentClosed += OnDocumentClosed;
        }

        void OnActiveContentChanged(object sender, EventArgs<Content> e)
        {
            if (e.Value is Document)
                Layers = e.Value.To<Document>().Layers.Where(i => i.IsSelected).ToList();
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            Layers = null;
        }

        ICommand deleteAdjustmentCommand;
        public ICommand DeleteAdjustmentCommand
        {
            get
            {
                deleteAdjustmentCommand = deleteAdjustmentCommand ?? new RelayCommand<AdjustmentEffect>(i =>
                {
                    var layer = layers[0] as AdjustmentLayer;
                    /*
                    for (var j = 0; j < layer.Adjustments.Count; j++)
                    {
                        if (layer.Adjustments[j].Index == i.Index)
                        {
                            layer.Adjustments.RemoveAt(j);
                            break;
                        }
                    }
                    */
                }, 
                i => i is AdjustmentEffect);
                return deleteAdjustmentCommand;
            }
        }
    }
}