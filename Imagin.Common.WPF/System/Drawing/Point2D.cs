using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Runtime.CompilerServices;

namespace System.Drawing
{
    [Serializable]
    public class Point2D : Base, ICloneable, IEquatable<Point2D>
    {
        int x = 0;
        public int X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        int y = 0;
        public int Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        public Point2D() : base() { }

        public Point2D(int x, int y) : this(new Point(x, y)) { }

        public Point2D(Point input) : base()
        {
            X = input.X;
            Y = input.Y;
        }

        public static bool operator ==(Point2D left, Point2D right) => left.EqualsOverload(right);

        public static bool operator !=(Point2D left, Point2D right) => !(left == right);

        public override bool Equals(object i) => Equals(i as Point2D);

        public bool Equals(Point2D right) => this.Equals<Point2D>(right) && x == right.x && y == right.y;

        public override int GetHashCode() => ((Point)this).GetHashCode();

#pragma warning disable 1591
        public static implicit operator Point2D(Point right) => new Point2D(right);

        public static implicit operator Point(Point2D right) => new Point(right.X, right.Y);
#pragma warning restore 1591

        public Point2D Clone() => new Point2D(x, y);
        object ICloneable.Clone() => Clone();

        public override string ToString() => $"X => {x}, Y => {y}";
    }
}