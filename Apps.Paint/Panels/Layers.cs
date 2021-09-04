using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Paint.Adjust;
using System;
using System.Windows.Input;

namespace Paint
{
    public class LayersPanel : Panel
    {
        public event EventHandler<EventArgs<Layer>> LayerSelected;

        LayerStyle copiedStyle;

        /// --------------------------------------------------------------

        public Document Document => Get.Current<MainViewModel>().ActiveDocument;

        /// --------------------------------------------------------------

        LayerCollection layers;
        public LayerCollection Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        Layer selectedLayer;
        public Layer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                this.Change(ref selectedLayer, value);
                OnLayerSelected(value);
            }
        }

        /// --------------------------------------------------------------

        public override string Title => "Layers";

        public LayersPanel() : base(Resources.Uri(nameof(Paint), "/Images/Layers.png"))
        {
            Get.Current<MainViewModel>().ActiveContentChanged += OnActiveContentChanged;
            Get.Current<MainViewModel>().DocumentClosed += OnDocumentClosed;
        }

        /// --------------------------------------------------------------

        void OnActiveContentChanged(object sender, EventArgs<Content> e)
        {
            if (e.Value is Document)
                Layers = e.Value.To<Document>().Layers;
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            Layers = default(LayerCollection);
        }

        void OnLayerSelected(Layer value)
        {
            LayerSelected?.Invoke(this, new EventArgs<Layer>(value));
        }

        /// --------------------------------------------------------------

        public void Add(Layer layer, bool aboveOrBelow)
        {
            var index = 0;
            GroupLayer parent = null;
            LayerCollection layers = null;

            if (SelectedLayer != null)
            {
                if (SelectedLayer is GroupLayer)
                {
                    index = 0;
                    layers = ((GroupLayer)SelectedLayer).Layers;
                    parent = (GroupLayer)SelectedLayer;
                }
                else
                {
                    layers
                    = SelectedLayer.Parent != null
                    ? SelectedLayer.Parent.Layers
                    : Layers;

                    index = layers.IndexOf(SelectedLayer);
                    parent = SelectedLayer.Parent;
                }
            }
            else layers = Layers;

            index
            = aboveOrBelow
            ? index
            : index + 1;

            if (layer == null)
                layer = new PixelLayer("Untitled", new System.Drawing.Size(Get.Current<MainViewModel>().ActiveDocument.Width, Get.Current<MainViewModel>().ActiveDocument.Height));

            layer.Parent = parent;
            layers.Insert(index, layer);

            layer.IsSelected = true;
            Get.Current<MainViewModel>().ActiveDocument.History.Add(new NewLayerAction(layer, layers, 0));
        }

        void Group()
        {
            var selectedLayer = SelectedLayer;

            LayerCollection layers
            = selectedLayer.Parent == null
            ? Layers
            : selectedLayer.Parent.Layers;

            var layerIndex = layers.IndexOf(selectedLayer);
            layers.RemoveAt(layerIndex);

            var group = new GroupLayer(layers.Document, "Untitled");
            group.Parent = selectedLayer.Parent;

            layers.Insert(layerIndex, group);

            selectedLayer.Parent = group;
            group.Layers.Add(selectedLayer);
        }

        /// --------------------------------------------------------------

        ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                addCommand = addCommand ?? new RelayCommand(() => Add(null, true), () => Document != null && Layers != null);
                return addCommand;
            }
        }

        ICommand addAdjustmentCommand;
        public ICommand AddAdjustmentCommand
        {
            get
            {
                addAdjustmentCommand = addAdjustmentCommand ?? new RelayCommand<AdjustmentEffect>(i => { } /*(SelectedLayer as EffectLayer).Adjustments.Add(i.Copy())*/, i => Document != null && Layers != null && SelectedLayer is AdjustmentLayer);
                return addAdjustmentCommand;
            }
        }
        
        /// --------------------------------------------------------------

        ICommand flattenCommand;
        public ICommand FlattenCommand
        {
            get
            {
                flattenCommand = flattenCommand ?? new RelayCommand(() => Document.Flatten(), () => Document != null);
                return flattenCommand;
            }
        }

        /// --------------------------------------------------------------

        ICommand groupCommand;
        public ICommand GroupCommand
        {
            get
            {
                groupCommand = groupCommand ?? new RelayCommand(() => Group(), () => SelectedLayer != null);
                return groupCommand;
            }
        }

        /// --------------------------------------------------------------

        ICommand insertAboveCommand;
        public ICommand InsertAboveCommand
        {
            get
            {
                insertAboveCommand = insertAboveCommand ?? new RelayCommand<object>(i => Add(i as Layer, true), i => Document != null);
                return insertAboveCommand;
            }
        }

        ICommand insertBelowCommand;
        public ICommand InsertBelowCommand
        {
            get
            {
                insertBelowCommand = insertBelowCommand ?? new RelayCommand<object>(i => Add(i as Layer, false), i => Document != null);
                return insertBelowCommand;
            }
        }

        /// --------------------------------------------------------------

        ICommand rasterizeCommand;
        public ICommand RasterizeCommand
        {
            get
            {
                rasterizeCommand = rasterizeCommand ?? new RelayCommand(() => (SelectedLayer as RasterizableLayer).Rasterize(Layers, new System.Drawing.Size(Document.Width, Document.Height)), () => SelectedLayer is RasterizableLayer);
                return rasterizeCommand;
            }
        }

        ICommand rasterizeStyleCommand;
        public ICommand RasterizeStyleCommand
        {
            get
            {
                rasterizeStyleCommand = rasterizeStyleCommand ?? new RelayCommand(() => SelectedLayer.Style.Rasterize(), () => SelectedLayer != null && SelectedLayer.Style != null);
                return rasterizeStyleCommand;
            }
        }

        /// --------------------------------------------------------------

        ICommand clearStyleCommand;
        public ICommand ClearStyleCommand
        {
            get
            {
                clearStyleCommand = clearStyleCommand ?? new RelayCommand<object>(p => (p as Layer).Style.Clear(), p => p is Layer);
                return clearStyleCommand;
            }
        }

        ICommand copyStyleCommand;
        public ICommand CopyStyleCommand
        {
            get
            {
                copyStyleCommand = copyStyleCommand ?? new RelayCommand<object>(p => copiedStyle = (p as Layer).Style, p => p is Layer);
                return copyStyleCommand;
            }
        }

        ICommand pasteStyleCommand;
        public ICommand PasteStyleCommand
        {
            get
            {
                pasteStyleCommand = pasteStyleCommand ?? new RelayCommand<object>(p =>
                {
                    var layer = p as Layer;

                    var oldStyle = layer.Style;
                    var newStyle = copiedStyle;

                    layer.Style = newStyle;

                    Get.Current<MainViewModel>().ActiveDocument.History.Add(new PasteLayerStyleAction(layer, oldStyle, newStyle));
                }, p => p is Layer && copiedStyle is LayerStyle);
                return pasteStyleCommand;
            }
        }

        /// --------------------------------------------------------------

        ICommand mergeDownCommand;
        public ICommand MergeDownCommand
        {
            get
            {
                mergeDownCommand = mergeDownCommand ?? new RelayCommand(() =>
                {
                    var layers
                    = SelectedLayer.Parent != null
                    ? SelectedLayer.Parent.Layers
                    : Layers;

                    var index = layers.IndexOf(SelectedLayer);
                    if (index + 1 < layers.Count)
                    {
                        if (layers[index + 1] is VisualLayer)
                        {
                            var a = (VisualLayer)layers[index + 1];
                            var b = (VisualLayer)SelectedLayer;

                            var newLayer = a.Merge(b, new System.Drawing.Size(Document.Width, Document.Height));
                            newLayer.Parent = SelectedLayer.Parent;

                            layers.Insert(index, newLayer);
                            layers.Remove(a);
                            layers.Remove(b);
                        }
                    }
                }, () => SelectedLayer is VisualLayer);
                return mergeDownCommand;
            }
        }

        ICommand mergeUpCommand;
        public ICommand MergeUpCommand
        {
            get
            {
                mergeUpCommand = mergeUpCommand ?? new RelayCommand(() =>
                {
                    var layers
                    = SelectedLayer.Parent != null
                    ? SelectedLayer.Parent.Layers
                    : Layers;

                    var index = layers.IndexOf(SelectedLayer);
                    if (index - 1 > -1)
                    {
                        if (layers[index - 1] is VisualLayer)
                        {
                            var a = (VisualLayer)layers[index - 1];
                            var b = (VisualLayer)SelectedLayer;

                            var newLayer = b.Merge(a, new System.Drawing.Size(Document.Width, Document.Height));
                            newLayer.Parent = SelectedLayer.Parent;

                            layers.Insert(index, newLayer);
                            layers.Remove(a);
                            layers.Remove(b);
                        }
                    }
                }, () => SelectedLayer is VisualLayer);
                return mergeUpCommand;
            }
        }

        ICommand mergeVisibleCommand;
        public ICommand MergeVisibleCommand
        {
            get
            {
                mergeVisibleCommand = mergeVisibleCommand ?? new RelayCommand(() => Document.MergeVisible(), () => Document != null);
                return mergeVisibleCommand;
            }
        }

        /// --------------------------------------------------------------

        ICommand moveDownCommand;
        public ICommand MoveDownCommand
        {
            get
            {
                moveDownCommand = moveDownCommand ?? new RelayCommand(() =>
                {
                    var selectedLayer = SelectedLayer;

                    LayerCollection layers
                    = selectedLayer.Parent != null
                    ? selectedLayer.Parent.Layers
                    : Layers;

                    var index = layers.IndexOf(selectedLayer);
                    layers.RemoveAt(index);
                    layers.Insert((index + 1).Maximum(layers.Count), selectedLayer);

                    Get.Current<MainViewModel>().ActiveDocument.History.Add(new MoveLayerAction(layers, Array<Layer>.New(selectedLayer)));
                }, () => SelectedLayer != null);
                return moveDownCommand;
            }
        }

        ICommand moveUpCommand;
        public ICommand MoveUpCommand
        {
            get
            {
                moveUpCommand = moveUpCommand ?? new RelayCommand(() =>
                {
                    var selectedLayer = SelectedLayer;

                    LayerCollection layers
                    = selectedLayer.Parent != null
                    ? selectedLayer.Parent.Layers
                    : Layers;

                    var index = layers.IndexOf(selectedLayer);
                    layers.RemoveAt(index);
                    layers.Insert((index - 1).Minimum(0), selectedLayer);

                    Get.Current<MainViewModel>().ActiveDocument.History.Add(new MoveLayerAction(layers, Array<Layer>.New(selectedLayer)));
                }, () => SelectedLayer != null);
                return moveUpCommand;
            }
        }

        ICommand moveOutsideCommand;
        public ICommand MoveOutsideCommand
        {
            get
            {
                moveOutsideCommand = moveOutsideCommand ?? new RelayCommand(() =>
                {
                    var selectedLayer = SelectedLayer;
                    LayerCollection layers = selectedLayer.Parent.Layers;
                    layers.Remove(selectedLayer);

                    layers
                        = selectedLayer.Parent.Parent != null
                        ? selectedLayer.Parent.Parent.Layers
                        : Layers;

                    var index = layers.IndexOf(selectedLayer.Parent);
                    layers.Insert(index, selectedLayer);
                }, () => SelectedLayer?.Parent != null);
                return moveOutsideCommand;
            }
        }

        /// --------------------------------------------------------------

        ICommand duplicateCommand;
        public ICommand DuplicateCommand
        {
            get
            {
                duplicateCommand = duplicateCommand ?? new RelayCommand(() =>
                {
                    var selectedLayer = SelectedLayer;

                    var clone = selectedLayer.Clone();
                    clone.Parent = selectedLayer.Parent;

                    LayerCollection layers
                    = selectedLayer.Parent != null
                    ? selectedLayer.Parent.Layers
                    : Layers;

                    var index = layers.IndexOf(selectedLayer);
                    layers.Insert(index, clone);

                    Get.Current<MainViewModel>().ActiveDocument.History.Add(new DuplicateLayerAction(clone, layers, index));
                }, () => Get.Current<MainViewModel>().ActiveDocument != null && SelectedLayer != null);
                return duplicateCommand;
            }
        }

        ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand = deleteCommand ?? new RelayCommand(() =>
                {
                    if (Layers.Count == 1)
                    {
                        Dialog.Show("Delete", "The document must specify at least one layer.", DialogImage.Error, DialogButtons.Ok);
                        return;
                    }

                    var selectedLayer = SelectedLayer;

                    LayerCollection layers
                    = selectedLayer.Parent != null
                    ? selectedLayer.Parent.Layers
                    : Layers;

                    var index = layers.IndexOf(selectedLayer);
                    layers.RemoveAt(index);

                    Get.Current<MainViewModel>().ActiveDocument.History.Add(new DeleteLayerAction(selectedLayer, layers, index));
                }, () => Get.Current<MainViewModel>().ActiveDocument != null && SelectedLayer != null);
                return deleteCommand;
            }
        }
    }
}