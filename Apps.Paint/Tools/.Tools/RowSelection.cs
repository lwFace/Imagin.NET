using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Paint
{
    [Serializable]
    public class RowSelectionTool : SelectionTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/RowSelection.png").OriginalString;

        int height = 30;
        public int Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        protected override IEnumerable<Point> GetPoints(double x, double y, double height, double width)
        {
            yield return new Point(0, y);
            yield return new Point(0, y + height);
            yield return new Point(width, y + height);
            yield return new Point(width, y);
        }

        protected override Int32Region CalculateRegion(Point point) => new Int32Region(point.X.Int32(), point.Y.Coerce(Document.Height - height, 0).Int32(), Document.Width, height);

        public override string ToString() => "Row selection";
    }
}