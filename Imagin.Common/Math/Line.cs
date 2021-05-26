using Imagin.Common.Linq;
using System;
using System.Runtime.CompilerServices;

namespace Imagin.Common.Math
{
    [Serializable]
    public class Line<T> : Base, IChange, ICloneable, IEquatable<Line<T>>
    {
        public event ChangedEventHandler Changed;

        //.....................................................................................

        public virtual Vector2<T> Distance { get; }

        //.....................................................................................

        T x1 = default;
        public T X1
        {
            get => x1;
            set => this.Change(ref x1, value);
        }

        T y1 = default;
        public T Y1
        {
            get => y1;
            set => this.Change(ref y1, value);
        }

        T x2 = default;
        public T X2
        {
            get => x2;
            set => this.Change(ref x2, value);
        }

        T y2 = default;
        public T Y2
        {
            get => y2;
            set => this.Change(ref y2, value);
        }

        //.....................................................................................

        public Line() : base() { }

        public Line(T x1, T y1, T x2, T y2) : this()
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        //.....................................................................................

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            Changed?.Invoke(this);
        }

        public override string ToString() => $"{nameof(X1)} = {x1}, {nameof(Y1)} = {y1}, {nameof(X2)} = {x2}, {nameof(Y2)} = {y2}";

        //.....................................................................................

        public Line<T> Clone() => new Line<T>(x1, y1, x2, y2);
        object ICloneable.Clone() => Clone();

        //.....................................................................................

        public static bool operator ==(Line<T> left, Line<T> right) => left.EqualsOverload(right);

        public static bool operator !=(Line<T> left, Line<T> right) => !(left == right);

        public bool Equals(Line<T> i) => this.Equals<Line<T>>(i) && X1.Equals(i.X1) && X2.Equals(i.X2) && Y1.Equals(i.Y1) && Y2.Equals(i.Y2);

        public override bool Equals(object i) => Equals((Line<T>)i);

        public override int GetHashCode() => new { X1, Y1, X2, Y2 }.GetHashCode();
    }

    [Serializable]
    public class DoubleLine : Line<double>
    {
        public override Vector2<double> Distance => new Vector2<double>(X1 > X2 ? X1 - X2 : X1 < X2 ? X2 - X1 : 0, Y1 > Y2 ? Y1 - Y2 : Y1 < Y2 ? Y2 - Y1 : 0);

        public DoubleLine(double x1, double y1, double x2, double y2) : base(x1, y1, x2, y2) { }
    }

    [Serializable]
    public class Int32Line : Line<int>
    {
        public override Vector2<int> Distance => new Vector2<int>(X1 > X2 ? X1 - X2 : X1 < X2 ? X2 - X1 : 0, Y1 > Y2 ? Y1 - Y2 : Y1 < Y2 ? Y2 - Y1 : 0);

        public Int32Line(int x1, int y1, int x2, int y2) : base(x1, y1, x2, y2) { }
    }
}