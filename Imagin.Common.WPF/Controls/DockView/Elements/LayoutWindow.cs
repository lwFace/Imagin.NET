using Imagin.Common.Math;
using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class LayoutWindow : Base
    {
        LayoutElement child;
        public LayoutElement Child
        {
            get => child;
            set => this.Change(ref child, value);
        }

        Point2D position;
        public Point2D Position
        {
            get => position;
            set => this.Change(ref position, value);
        }

        DoubleSize size;
        public DoubleSize Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        public LayoutWindow() : base() { }
    }
}