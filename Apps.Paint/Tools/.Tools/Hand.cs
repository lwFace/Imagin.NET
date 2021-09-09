using Imagin.Common;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Paint
{
    [Serializable]
    public class HandTool : Tool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Hand.png").OriginalString;

        ScrollViewer ScrollViewer
        {
            get
            {
                return Get.Current<MainViewModel>().ActiveDocument.ScrollViewer;
            }
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                var dx = point.X - MouseDown.Value.X;
                var dy = point.Y - MouseDown.Value.Y;

                ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - dx);
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - dy);
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
        }

        public override string ToString() => "Hand";
    }
}