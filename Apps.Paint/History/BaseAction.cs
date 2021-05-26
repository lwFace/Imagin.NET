using Imagin.Common;
using System;

namespace Paint
{
    [Serializable]
    public abstract class BaseAction : NamedObject, Imagin.Common.ICloneable
    {
        public virtual bool IsRepeatable
        {
            get => false;
        }

        protected BaseAction() : base()
        {
        }

        public abstract void Execute();

        public abstract void Reverse();

        object Imagin.Common.ICloneable.Clone()
        {
            return Clone();
        }
        public abstract BaseAction Clone();
    }

    [Serializable]
    public abstract class RepeatableAction : BaseAction
    {
        public sealed override bool IsRepeatable
        {
            get => true;
        }

        protected RepeatableAction() : base()
        {
        }
    }

    [Serializable]
    public abstract class LayerAction : BaseAction
    {
        /// <summary>
        /// The layer in question.
        /// </summary>
        protected Layer Layer
        {
            get; set;
        }

        protected LayerAction(Layer layer) : base()
        {
            Layer = layer;
        }
    }

    [Serializable]
    public abstract class RegionalLayerAction : LayerAction
    {
        /// <summary>
        /// The index of the layer relative to the collection.
        /// </summary>
        protected int Index
        {
            get; set;
        }

        /// <summary>
        /// The collection of layers in which the layer is stored.
        /// </summary>
        protected LayerCollection Layers
        {
            get; set;
        }

        protected RegionalLayerAction(Layer layer, LayerCollection layers, int index) : base(layer)
        {
            Layers = layers;
            Index = index;
        }
    }
}