using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Text;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Notes
{
    [XmlRoot(ElementName = nameof(List))]
    public partial class List : TextDocument
    {
        static readonly XmlSerializer Serializer = new XmlSerializer(typeof(List), new XmlAttributeOverrides(), Array<Type>.New(typeof(Attributes), typeof(Bullets), typeof(DateTime), typeof(Line), typeof(Lines), typeof(SortDirection), typeof(SortNames)), new XmlRootAttribute(nameof(List)), nameof(Notes));

        /// ........................................................................

        enum Category
        {
            Column,
            Image,
            View
        }

        /// ........................................................................

        [Serializable]
        public enum SortNames
        {
            Added,
            Checked,
            DateTime,
            Decimal,
            Text
        }

        /// ........................................................................

        bool Initialized = false;

        /// ........................................................................

        bool valid = false;
        [Hidden]
        [XmlIgnore]
        public bool Valid
        {
            get => valid;
            private set => this.Change(ref valid, value);
        }

        Attributes attributes = Attributes.Bullet;
        [Category("Attributes")]
        [EnumFormat(EnumFormat.Flags)]
        [XmlAttribute]
        public Attributes Attributes
        {
            get => attributes;
            set => this.Change(ref attributes, value);
        }

        Bullets bullets = Bullets.Square;
        [Category("Attributes")]
        [XmlAttribute]
        public Bullets Bullets
        {
            get => bullets;
            set => this.Change(ref bullets, value);
        }

        Columns column = Columns.None;
        [Category(Category.Column)]
        [DisplayName("Column")]
        [XmlAttribute]
        public Columns Value
        {
            get => column;
            set => (this).Change(ref this.column, value);
        }

        [Hidden]
        [XmlIgnore]
        public int Count => Lines.Count;

        string dateTimeFormat = "dddd, MMM d, yyyy";
        [Category(nameof(Category.Column) + " format")]
        [DisplayName("DateTime")]
        [XmlAttribute]
        public string DateTimeFormat
        {
            get => dateTimeFormat;
            set => this.Change(ref dateTimeFormat, value);
        }

        string decimalFormat = "${N2}";
        [Category(nameof(Category.Column) + " format")]
        [DisplayName("Decimal")]
        [XmlAttribute]
        public string DecimalFormat
        {
            get => decimalFormat;
            set => this.Change(ref decimalFormat, value);
        }
        
        [Hidden]
        [XmlIgnore]
        public override bool IsModified
        {
            get => base.IsModified;
            set => base.IsModified = value;
        }

        Lines lines;
        [Hidden]
        public Lines Lines
        {
            get => lines;
            set => this.Change(ref lines, value);
        }

        SortDirection sortDirection = SortDirection.Descending;
        [Category("Sort")]
        [DisplayName("Direction")]
        [XmlAttribute]
        public SortDirection SortDirection
        {
            get => sortDirection;
            set => this.Change(ref sortDirection, value);
        }

        SortNames sortName = SortNames.Added;
        [Category("Sort")]
        [DisplayName("Name")]
        [XmlAttribute]
        public SortNames SortName
        {
            get => sortName;
            set => this.Change(ref sortName, value);
        }

        [Hidden]
        [XmlIgnore]
        public override string Title => $"{Name} ({(attributes.HasFlag(Attributes.Check) ? $"{lines.Where(i => i.Checked).Count()}/" : string.Empty)}{lines.Count}){(IsModified ? "*" : string.Empty)}";

        /// ........................................................................

        //For XML serialization
        public List() : base(string.Empty, string.Empty) { }

        public List(string name, string text) : base(name, text)
        {
            Lines = new Lines(this);
        }

        /// ........................................................................

        public static List Open(string filePath, System.Text.Encoding encoding)
        {
            List result = null;
            Try.Invoke(() =>
            {
                var a = Read(filePath, encoding);
                var b = new System.IO.StringReader(a);
                result = Serializer.Deserialize(b) as List;
            });
            return result;
        }

        public override Result Open()
        {
            try
            {
                //The new way
                var list = (List)Serializer.Deserialize(new System.IO.StringReader(Text));

                foreach (var i in list.GetType().GetProperties())
                {
                    switch (i.Name)
                    {
                        case nameof(Lines):
                        case nameof(Name):
                        case nameof(Text):
                            continue;
                    }

                    if (i.GetMethod?.IsPublic == true && i.SetMethod?.IsPublic == true)
                        this.SetPropertyValue(i.Name, i.GetValue(list));
                }

                foreach (var i in list.Lines)
                {
                    i.List = this;
                    i.Reset(TimeSpan.FromSeconds(60), true);

                    Lines.Add(i);
                }
                Valid = true;
            }
            catch (Exception e)
            {
                try
                {
                    //The old way
                    var stringReader = new System.IO.StringReader(Text);
                    string line = string.Empty;
                    do
                    {
                        line = stringReader.ReadLine();
                        if (line != null)
                            Lines.Add(Convert(line.Trim()));
                    }
                    while (line != null);
                    Valid = false;
                }
                catch
                {
                    return new Error(e);
                }
            }
            Initialized = true;
            return new Success();
        }

        /// ........................................................................

        protected virtual string Convert(Line line) => line.Text;

        protected virtual Line Convert(string line) => new Line(this, false, line);

        protected override void Save(string path)
        {
            if (!Valid)
            {
                base.Save(path);
                return;
            }
            Try.Invoke(() => Serializer.Serialize(new System.IO.StreamWriter(path), this));
        }

        /// ........................................................................

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Attributes):
                case nameof(Bullets):
                case nameof(DateTimeFormat):
                case nameof(DecimalFormat):
                case nameof(SortDirection):
                case nameof(SortName):
                case nameof(Value):
                    Initialized.If(i => i, i => IsModified = true);
                    break;
            }
        }

        public override string ToString() => nameof(List);

        /// ........................................................................

        public void Refresh()
        {
            var stringBuilder = new System.Text.StringBuilder();
            foreach (var i in Lines)
                stringBuilder.AppendLine(Convert(i));

            Text = stringBuilder.ToString();
        }

        public void Validate() => Valid = true;

        /// ........................................................................

        ICommand addCommand;
        [Hidden]
        public ICommand AddCommand
        {
            get
            {
                addCommand = addCommand ?? new RelayCommand(() =>
                {
                    foreach (var i in Lines)
                    {
                        if (i.Editing)
                            i.Editing = false;
                    }

                    Lines.Insert(0, new Line(this, true, string.Empty));
                    Refresh();
                });
                return addCommand;
            }
        }

        ICommand clearCommand;
        [Hidden]
        [XmlIgnore]
        public ICommand ClearCommand
        {
            get
            {
                clearCommand = clearCommand ?? new RelayCommand(() =>
                {
                    var result = Dialog.Show("Clear", $"Are you sure you want to clear the {nameof(List)}?", DialogImage.Warning, DialogButtons.YesNo);
                    if (result == 0)
                    {
                        Lines.Clear();
                        Refresh();
                    }
                });
                return clearCommand;
            }
        }
    }
}