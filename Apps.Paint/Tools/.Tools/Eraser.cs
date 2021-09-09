using Imagin.Common;
using Imagin.Common.Math;
using System;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class EraserTool : BrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Eraser.png").OriginalString;

        protected override void Draw(Vector2<int> point, Color color)
        {
            Mode = null;
            base.Draw(point, Color.FromArgb(0, 0, 0, 0));
        }

        public override string ToString() => "Eraser";
    }
}