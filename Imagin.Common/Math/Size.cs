using Imagin.Common.Linq;
using System;
using System.Runtime.CompilerServices;

namespace Imagin.Common.Math
{
    public enum SizeField
    {
        Height,
        Width
    }

    [Serializable]
    public class Size<T> : Base, IChange, ICloneable, IEquatable<Size<T>>
    {
        public event ChangedEventHandler Changed;

        //.....................................................................................

        T height = default;
        public T Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        T width = default;
        public T Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        //.....................................................................................

        public Size() : this(default) { }

        public Size(T input) : this(input, input) { }

        public Size(T height, T width) : base()
        {
            Height = height;
            Width = width;
        }

        //.....................................................................................

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            Changed?.Invoke(this);
        }

        public override string ToString() => $"{nameof(Height)} = {height}, {nameof(Width)} = {width}";

        //.....................................................................................

        public Size<T> Clone() => new Size<T>(width, height);
        object ICloneable.Clone() => Clone();

        //.....................................................................................

        public static bool operator ==(Size<T> left, Size<T> right) => left.EqualsOverload(right);

        public static bool operator !=(Size<T> left, Size<T> right) => !(left == right);

        public bool Equals(Size<T> i) => this.Equals<Size<T>>(i) && height.Equals(i.height) && width.Equals(i.width);

        public override bool Equals(object i) => Equals((Size<T>)i);

        public override int GetHashCode() => new { height, width }.GetHashCode();
    }

    [Serializable]
    public class Int32Size : Size<int>
    {
        public Int32Size() : base() { }

        public Int32Size(int input) : base(input) { }

        public Int32Size(int height, int width) : base(height, width) { }

        //.....................................................................................

        public Int32Size Resize(SizeField field, int value)
        {
            int oldHeight = Height, oldWidth = Width, newHeight = 0, newWidth = 0;
            switch (field)
            {
                case SizeField.Height:
                    newHeight = value;
                    newWidth = (newHeight.Double() / (oldHeight.Double() / oldWidth.Double())).Int32();
                    break;

                case SizeField.Width:
                    newWidth = value;
                    newHeight = (newWidth.Double() * (oldHeight.Double() / oldWidth.Double())).Int32();
                    break;
            }
            return new Int32Size(newHeight, newWidth);
        }
    }

    [Serializable]
    public class DoubleSize : Size<double>
    {
        public DoubleSize() : base() { }

        public DoubleSize(double input) : base(input) { }

        public DoubleSize(double height, double width) : base(height, width) { }

        //.....................................................................................

        public DoubleSize Resize(SizeField field, double value)
        {
            double oldHeight = Height, oldWidth = Width, newHeight = 0, newWidth = 0;
            switch (field)
            {
                case SizeField.Height:
                    newHeight = value;
                    newWidth = newHeight / (oldHeight / oldWidth);
                    break;

                case SizeField.Width:
                    newWidth = value;
                    newHeight = newWidth * (oldHeight / oldWidth);
                    break;
            }
            return new DoubleSize(newHeight, newWidth);
        }
    }
}