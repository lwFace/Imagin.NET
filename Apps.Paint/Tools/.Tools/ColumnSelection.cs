using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Paint
{
    [Serializable]
    public class ColumnSelectionTool : SelectionTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/ColumnSelection.png").OriginalString;

        int width = 30;
        public int Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        protected override IEnumerable<Point> GetPoints(double x, double y, double height, double width)
        {
            yield return new Point(x, 0);
            yield return new Point(x, height);
            yield return new Point(x + width, height);
            yield return new Point(x + width, 0);
        }

        protected override Int32Region CalculateRegion(Point point) => new Int32Region(point.X.Coerce(Document.Width - width, 0).Int32(), point.Y.Int32(), width, Document.Height);

        public override string ToString() => "Column selection";
    }
}