using Imagin.Common;
using Imagin.Common.Math;
using System;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class PencilTool : BrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Pencil.png").OriginalString;

        public PencilTool() : base()
        {
            Brushes.Clear();
            Brushes.Add(new FixedSquareBrush());
            Brush = Brushes[0];
        }

        protected override void Draw(Vector2<int> point, Color color)
        {
            Mode = null;
            base.Draw(point, color);
        }

        public override string ToString() => "Pencil";
    }
}