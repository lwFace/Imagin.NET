using Imagin.Common;
using System;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class PathLayer : Layer
    {
        PointCollection points = new PointCollection();
        public PointCollection Points
        {
            get => points;
            set => this.Change(ref points, value);
        }

        public PathLayer(string name = null) : base(LayerType.Path, name) { }

        public override Layer Clone()
        {
            return new PathLayer(Name)
            {
                Points = Points
            };
        }
    }
}