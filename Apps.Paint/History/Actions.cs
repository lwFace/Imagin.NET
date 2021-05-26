using System;
using System.Collections.Generic;

namespace Paint
{
    [Serializable]
    public class DeleteLayerAction : RegionalLayerAction
    {
        public DeleteLayerAction(Layer layer, LayerCollection layers, int index) : base(layer, layers, index)
        {
            Name = "Delete layer";
        }

        public override BaseAction Clone()
        {
            return default(BaseAction);
        }

        public override void Execute()
        {
            Layers.Remove(Layer);
        }

        public override void Reverse()
        {
            Layers.Insert(Index, Layer);
        }
    }

    [Serializable]
    public class DuplicateLayerAction : RegionalLayerAction
    {
        public DuplicateLayerAction(Layer layer, LayerCollection layers, int index) : base(layer, layers, index)
        {
            Name = "Duplicate layer";
        }

        public override BaseAction Clone()
        {
            return default(BaseAction);
        }

        public override void Execute()
        {
            Layers.Insert(Index, Layer);
        }

        public override void Reverse()
        {
            Layers.Remove(Layer);
        }
    }

    [Serializable]
    public class MergeLayerAction : BaseAction
    {
        public MergeLayerAction() : base()
        {
            Name = "Merge layers";
        }

        public override BaseAction Clone()
        {
            return default(BaseAction);
        }

        public override void Execute()
        {
        }

        public override void Reverse()
        {
        }
    }

    [Serializable]
    public class MoveLayerAction : BaseAction
    {
        IEnumerable<Layer> Layers
        {
            get; set;
        }

        LayerCollection Source
        {
            get; set;
        }

        public MoveLayerAction(LayerCollection source, IEnumerable<Layer> layers) : base()
        {
            Name = "Move layer";
            Source = source;
            Layers = layers;
        }

        public override BaseAction Clone()
        {
            return default(BaseAction);
        }

        public override void Execute()
        {

        }

        public override void Reverse()
        {
        }
    }

    [Serializable]
    public class NewLayerAction : RegionalLayerAction
    {
        public NewLayerAction(Layer layer, LayerCollection layers, int index) : base(layer, layers, index)
        {
            Name = "New layer";
        }

        public override BaseAction Clone()
        {
            return default(BaseAction);
        }

        public override void Execute()
        {
            Layers.Insert(Index, Layer);
        }

        public override void Reverse()
        {
            Layers.Remove(Layer);
        }
    }

    [Serializable]
    public class PasteLayerStyleAction : LayerAction
    {
        LayerStyle NewStyle
        {
            get; set;
        }

        LayerStyle OldStyle
        {
            get; set;
        }

        public PasteLayerStyleAction(Layer layer, LayerStyle oldStyle, LayerStyle newStyle) : base(layer)
        {
            Name = "Paste layer style";
            OldStyle = oldStyle;
            NewStyle = newStyle;
        }

        public override BaseAction Clone()
        {
            return default(BaseAction);
        }

        public override void Execute()
        {
            Layer.Style = NewStyle;
        }

        public override void Reverse()
        {
            Layer.Style = OldStyle;
        }
    }

    [Serializable]
    public class RasterizeLayerAction : BaseAction
    {
        public RasterizeLayerAction() : base()
        {
            Name = "Rasterize layer";
        }

        public override BaseAction Clone()
        {
            return default(BaseAction);
        }

        public override void Execute()
        {
        }

        public override void Reverse()
        {
        }
    }
}