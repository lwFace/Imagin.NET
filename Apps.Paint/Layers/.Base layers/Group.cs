using Imagin.Common;
using System;

namespace Paint
{
    [Serializable]
    public class GroupLayer : Layer
    {
        bool isExpanded = true;
        [DisplayName("Open")]
        public bool IsExpanded
        {
            get => isExpanded;
            set => this.Change(ref isExpanded, value);
        }

        LayerCollection layers;
        public LayerCollection Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        public GroupLayer(Document document, string name) : base(LayerType.Group, name)
        {
            Layers = new LayerCollection(document);
        }

        public override Layer Clone() => new GroupLayer(Document, Name);

        public override string ToString() => "Group";
    }
}