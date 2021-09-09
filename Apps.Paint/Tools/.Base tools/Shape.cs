using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Paint
{
    [Serializable]
    public abstract class ShapeTool : Tool
    {
        public static IEnumerable<Point> CalculatePoints(IList<Point> points)
        {
            for (var i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                {
                    yield return points[i];
                    break;
                }

                var a = points[i].Int32().Double();
                var b = points[i + 1].Int32().Double();

                if (a == b)
                    continue;

                var c = b - a;
                var d = a;

                var ax = c.X.Absolute();
                var ay = c.Y.Absolute();

                var ix = (c.X / ax);
                var iy = (c.Y / ay);

                var cx = ax;
                var cy = ay;

                yield return d;

                if (ax == ay)
                {
                    while (d != b)
                    {
                        if (cx > 0)
                        {
                            d = new Point(d.X + ix, d.Y);
                            yield return d;
                            cx--;
                        }
                        if (cy > 0)
                        {
                            d = new Point(d.X, d.Y + iy);
                            yield return d;
                            cy--;
                        }
                    }
                }
                else if (ax > ay)
                {
                    //For every n ax, do ay
                    while (cy > 0)
                    {
                        cx = ax / ay;
                        while (cx > 0)
                        {
                            d = new Point(d.X + ix, d.Y);
                            yield return d;
                            cx--;
                        }
                        d = new Point(d.X, d.Y + iy);
                        yield return d;
                        cy--;
                    }
                }
                else if (ax < ay)
                {
                    //For every n ay, do ax
                    while (cx > 0)
                    {
                        cy = ay / ax;
                        while (cy > 0)
                        {
                            d = new Point(d.X, d.Y + iy);
                            yield return d;
                            cy--;
                        }
                        d = new Point(d.X + ix, d.Y);
                        yield return d;
                        cx--;
                    }
                }
            }
        }

        public static Int32Region CalculateRegion(Point a, Point b)
        {
            double x = 0, y = 0, height = 0, width = 0;

            x = a.X < b.X ? a.X : b.X;
            y = a.Y < b.Y ? a.Y : b.Y;

            width = (a.X > b.X ? a.X - b.X : b.X - a.X).Absolute();
            height = (a.Y > b.Y ? a.Y - b.Y : b.Y - a.Y).Absolute();

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                var greater = height > width ? height : width;
                var smaller = height < width ? height : width;

                if (b.X > a.X)
                {
                    //Top right
                    if (b.Y < a.Y)
                    {

                        if (b.Y + greater > a.Y)
                        {
                            height = width = smaller;
                            y = a.Y - smaller;
                        }
                        else height = width = greater;
                    }
                    //Bottom right
                    else height = width = greater;
                }
                else
                {
                    //Top left
                    if (b.Y < a.Y)
                    {
                        height = width = smaller;
                        x = a.X - smaller;
                        y = a.Y - smaller;
                    }
                    //Bottom left
                    else
                    {
                        if (b.X + greater > a.X)
                        {
                            height = width = smaller;
                            x = a.X - smaller;
                        }
                        else height = width = greater;
                    }
                }
            }

            return new Int32Region(x.Int32(), y.Int32(), width.Int32(), height.Int32());
        }

        protected override bool AssertLayer() => AssertPixelLayer();
    }

    [Serializable]
    public abstract class ShapeTool<ShapeLayer> : ShapeTool where ShapeLayer : Paint.ShapeLayer
    {
        public override Cursor Cursor => Cursors.Cross;

        [field: NonSerialized]
        protected ShapeLayer CurrentLayer;

        ShapeToolModes mode = ShapeToolModes.Shape;
        public ShapeToolModes Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        protected abstract ShapeLayer NewLayer();

        protected override bool AssertLayer()
        {
            if (Mode == ShapeToolModes.Pixels)
                return base.AssertLayer();

            if (Mode == ShapeToolModes.Shape)
                return true;

            throw new InvalidOperationException();
        }
    }
}