using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class PolygonLayer : RegionShapeLayer
    {
        double angle = 0;
        public double Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        int indent = 2;
        public int Indent
        {
            get => indent;
            set => this.Change(ref indent, value);
        }

        uint sides = 5;
        public uint Sides
        {
            get => sides;
            set => this.Change(ref sides, value);
        }

        bool star = false;
        public bool Star
        {
            get => star;
            set => this.Change(ref star, value);
        }

        public PolygonLayer(string name, int height, int width, double angle, uint sides, bool star, int indent, SolidColorBrush fill, SolidColorBrush stroke, double strokeThickness) : base(LayerType.Polygon, name, height, width, fill, stroke, strokeThickness)
        {
            this.angle = angle;
            this.sides = sides;
            this.star = star;
            this.indent = indent;
        }

        public sealed override Layer Clone()
        {
            return new PolygonLayer(Name, Height, Width, Angle, Sides, Star, Indent, Fill, Stroke, StrokeThickness)
            {
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Opacity = Opacity,
                Style = Style.Clone(),
                X = X,
                Y = Y,
            };
        }

        void Draw(WriteableBitmap input, double centerX, double centerY, double height, double width, Color color)
        {
            var center = new Point(centerX, centerY);
            var size = new Size(width.Coerce(double.MaxValue), height.Coerce(double.MaxValue));

            var points
                = Star
                ? CustomShape.GetStar(Angle.FromDegreeToRadian(), Sides.Int32(), Indent, new Rect(center.X - (Width / 2), center.Y - (Height / 2), size.Width, size.Height))
                : CustomShape.GetPolygon(Angle.FromDegreeToRadian(), center, size, Sides.Int32());

            input.FillPolygon(CustomShape.From(points), color);
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Angle):
                case nameof(Indent):
                case nameof(Sides):
                case nameof(Star):
                    Render();
                    break;
            }
        }

        public override IEnumerable<System.Windows.Point> GetPoints()
        {
            var region = Region;

            var centerX = Region.X + (Region.Width / 2.0);
            var centerY = Region.Y + (Region.Height / 2.0);

            var center = new Point(centerX, centerY);
            var size = new Size(Region.Width.Coerce(int.MaxValue), Region.Height.Coerce(int.MaxValue));

            var oldPoints
                = Star
                ? CustomShape.GetStar(Angle.FromDegreeToRadian(), Sides.Int32(), Indent, new Rect(center.X, center.Y, size.Width, size.Height))
                : CustomShape.GetPolygon(Angle.FromDegreeToRadian(), center, size, Sides.Int32());

            for (var i = 0; i < oldPoints.Length; i++)
                yield return new System.Windows.Point(oldPoints[i].X, oldPoints[i].Y);
        }

        public override void Render(System.Drawing.Graphics g)
        {
            var region = Region;

            var centerX = Region.X + (Region.Width / 2.0);
            var centerY = Region.Y + (Region.Height / 2.0);

            var center = new Point(centerX, centerY);
            var size = new Size(Region.Width.Coerce(int.MaxValue), Region.Height.Coerce(int.MaxValue));

            var oldPoints
                = Star
                ? CustomShape.GetStar(Angle.FromDegreeToRadian(), Sides.Int32(), Indent, new Rect(center.X, center.Y, size.Width, size.Height))
                : CustomShape.GetPolygon(Angle.FromDegreeToRadian(), center, size, Sides.Int32());

            g.DrawPolygon(StrokePen, oldPoints);
            g.FillPolygon(FillBrush, oldPoints);
        }

        public override void Render(WriteableBitmap input)
        {
            var centerX = Region.X + (Region.Width / 2.0);
            var centerY = Region.Y + (Region.Height / 2.0);

            Draw(input, centerX, centerY, Region.Height, Region.Width, Stroke.Color);
            Draw(input, centerX, centerY, Region.Height - (StrokeThickness * 2.0), Region.Width - (StrokeThickness * 2.0), Fill.Color);
        }

        public override string ToString() => "Polygon";
    }
}