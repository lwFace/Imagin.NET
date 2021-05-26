using Imagin.Common;
using Imagin.Common.Data;
using System;
using System.Net;
using System.Windows;

namespace Demo
{
    public class ComplexObject : NamedObject
    {
        bool boolean = false;
        [Category("Misc Types")]
        [System.ComponentModel.Description("Description for Boolean property.")]
        public bool Boolean
        {
            get => boolean;
            set => this.Change(ref boolean, value);
        }

        byte _byte = (byte)0;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Byte property.")]
        public byte Byte
        {
            get => _byte;
            set => this.Change(ref _byte, value);
        }

        Child1 child = new Child1();
        [Category("Special Types")]
        [System.ComponentModel.Description("Description for Child property.")]
        public Child1 Child
        {
            get => child;
            set => this.Change(ref child, value);
        }

        DateTime dateTime = DateTime.Now;
        [Category("Misc Types")]
        [System.ComponentModel.Description("Description for DateTime property.")]
        public DateTime DateTime
        {
            get => dateTime;
            set => this.Change(ref dateTime, value);
        }

        decimal _decimal = 0m;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Decimal property.")]
        public decimal Decimal
        {
            get => _decimal;
            set => this.Change(ref _decimal, value);
        }

        double _double = 0d;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Double property.")]
        public double Double
        {
            get => _double;
            set => this.Change(ref _double, value);
        }

        Visibility _enum = Visibility.Visible;
        [Category("Misc Types")]
        [System.ComponentModel.Description("Description for Enum property.")]
        public Visibility Enum
        {
            get => _enum;
            set => this.Change(ref _enum, value);
        }

        Guid guid = Guid.NewGuid();
        [Category("Misc Types")]
        [System.ComponentModel.Description("Description for Guid property.")]
        public Guid Guid
        {
            get => guid;
            set => this.Change(ref guid, value);
        }

        int _int = 0;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Int property.")]
        public int Int
        {
            get => _int;
            set => this.Change(ref _int, value);
        }

        long _long = 0L;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Long property.")]
        public long Long
        {
            get => _long;
            set => this.Change(ref _long, value);
        }

        long longFileSize = 0L;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for LongFileSize property.")]
        [LongFormat(LongFormat.FileSize)]
        public long LongFileSize
        {
            get => longFileSize;
            set => this.Change(ref longFileSize, value);
        }

        NetworkCredential networkCredential = new NetworkCredential("UserName", "Password");
        [Category("String Types")]
        [System.ComponentModel.Description("Description for NetworkCredential property.")]
        public NetworkCredential NetworkCredential
        {
            get => networkCredential;
            set => this.Change(ref networkCredential, value);
        }

        /*
        Point? nullablePoint = null;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for NullablePoint property.")]
        public Point? NullablePoint
        {
            get => nullablePoint;
            set => this.Change(ref nullablePoint, value);
        }
        */

        Point point = new Point(0, 0);
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Point property.")]
        public Point Point
        {
            get => point;
            set => this.Change(ref point, value);
        }

        short _short = (short)0;
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Short property.")]
        public short Short
        {
            get => _short;
            set => this.Change(ref _short, value);
        }

        Size size = new Size(0, 0);
        [Category("Numeric Types")]
        [System.ComponentModel.Description("Description for Size property.")]
        public Size Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        [Category("String Types")]
        [System.ComponentModel.Description("Description for NormalString property.")]
        public string NormalStringWithNoSetter
        {
            get => normalString;
        }

        string normalString = "Default string";
        [Category("String Types")]
        [System.ComponentModel.Description("Description for NormalString property.")]
        public string NormalString
        {
            get => normalString;
            set => this.Change(ref normalString, value);
        }

        string filePathString = string.Empty;
        [Category("String Types")]
        [System.ComponentModel.Description("Description for FilePathString property.")]
        [StringFormat(StringFormat.FilePath)]
        public string FilePathString
        {
            get => filePathString;
            set => this.Change(ref filePathString, value);
        }

        string folderPathString = string.Empty;
        [Category("String Types")]
        [System.ComponentModel.Description("Description for FolderPathString property.")]
        [StringFormat(StringFormat.FolderPath)]
        public string FolderPathString
        {
            get => folderPathString;
            set => this.Change(ref folderPathString, value);
        }

        string multilineString = string.Empty;
        [Category("String Types")]
        [System.ComponentModel.Description("Description for MultilineString property.")]
        [StringFormat(StringFormat.Multiline)]
        public string MultilineString
        {
            get => multilineString;
            set => this.Change(ref multilineString, value);
        }

        string passwordString = string.Empty;
        [Category("String Types")]
        [System.ComponentModel.Description("Description for PasswordString property.")]
        [StringFormat(StringFormat.Password)]
        public string PasswordString
        {
            get => passwordString;
            set => this.Change(ref passwordString, value);
        }

        string tokensString = "Red;Green;Blue;Yellow;Orange;Black;Purple;";
        [Category("String Types")]
        [System.ComponentModel.Description("Description for TokensString property.")]
        [StringFormat(StringFormat.Tokens)]
        public string TokensString
        {
            get => tokensString;
            set => this.Change(ref tokensString, value);
        }

        Uri uri = new Uri("http://www.google.com");
        [Category("Misc Types")]
        [System.ComponentModel.Description("Description for Uri property.")]
        public Uri Uri
        {
            get => uri;
            set => this.Change(ref uri, value);
        }

        Version version = new Version();
        [Category("Misc Types")]
        [System.ComponentModel.Description("Description for Version property.")]
        public Version Version
        {
            get => version;
            set => this.Change(ref version, value);
        }

        [Category("String Types")]
        [System.ComponentModel.Description("Description for Name property.")]
        [Featured(true)]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        public ComplexObject() : this(string.Empty) { }

        public ComplexObject(string Name) : base(Name) { }
    }
}
