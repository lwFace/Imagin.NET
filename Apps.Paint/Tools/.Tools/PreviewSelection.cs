using Imagin.Common;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public abstract class PreviewSelectionTool : Tool
    {
        public override Cursor Cursor => Cursors.Cross;

        PointCollection points = new PointCollection();
        public PointCollection Points
        {
            get => points;
            set => this.Change(ref points, value);
        }

        PointCollection preview = new PointCollection();
        public PointCollection Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }
    }
}