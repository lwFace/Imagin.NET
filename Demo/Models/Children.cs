using Imagin.Common;
using System.Windows;

namespace Demo
{
    public class Child1 : NamedObject
    {
        bool boolean = false;
        public bool Boolean
        {
            get => boolean;
            set => this.Change(ref boolean, value);
        }

        [Featured(true)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        double _double = 0d;
        public double Double
        {
            get => _double;
            set => this.Change(ref _double, value);
        }

        Child2 child = new Child2();
        public Child2 Child
        {
            get => child;
            set => this.Change(ref child, value);
        }

        public Child1() : base() { }
    }

    public class Child2 : NamedObject
    {
        [Featured(true)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        decimal _decimal = 0m;
        public decimal Decimal
        {
            get => _decimal;
            set => this.Change(ref _decimal, value);
        }

        Visibility _enum = Visibility.Visible;
        public Visibility Enum
        {
            get => _enum;
            set => this.Change(ref _enum, value);
        }

        long _long = 0L;
        public long Long
        {
            get => _long;
            set => this.Change(ref _long, value);
        }

        Child3 child = new Child3();
        public Child3 Child
        {
            get => child;
            set => this.Change(ref child, value);
        }

        public Child2() : base()
        {
        }
    }

    public class Child3 : NamedObject
    {
        [Featured(true)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        byte _byte = 0;
        public byte Byte
        {
            get => _byte;
            set => this.Change(ref _byte, value);
        }

        short _short = 0;
        public short Short
        {
            get => _short;
            set => this.Change(ref _short, value);
        }

        Size size = new Size(0, 0);
        public Size Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        public Child3() : base() { }
    }
}