using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Notes
{
    public partial class List
    {
        public class Line : Notify
        {
            [XmlIgnore]
            public List List { get; set; }

            DateTime added = DateTime.Now;
            [XmlAttribute]
            public DateTime Added
            {
                get => added;
                set => this.Change(ref added, value);
            }

            Decimal @decimal = 0;
            [XmlAttribute]
            public Decimal Decimal
            {
                get => @decimal;
                set => this.Change(ref @decimal, value);
            }

            bool @checked = false;
            [XmlAttribute]
            public bool Checked
            {
                get => @checked;
                set => this.Change(ref @checked, value);
            }

            DateTime dateTime = DateTime.Now.AddDays(1);
            [XmlAttribute]
            public DateTime DateTime
            {
                get => dateTime;
                set => this.Change(ref dateTime, value);
            }

            bool editing;
            [XmlIgnore]
            public bool Editing
            {
                get => editing;
                set => this.Change(ref editing, value);
            }

            string image = string.Empty;
            [XmlAttribute]
            public string Image
            {
                get => image;
                set => this.Change(ref image, value);
            }

            string text;
            [XmlText]
            public string Text
            {
                get => text;
                set => this.Change(ref text, value);
            }

            //For XML serialization
            public Line() : base() { }

            public Line(List list, bool editing, string text)
            {
                List = list;
                Editing = editing;
                Text = text;

                Interval = TimeSpan.FromSeconds(60);
                Enabled = true;
            }

            bool notifying = false;

            public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                base.OnPropertyChanged(propertyName);
                switch (propertyName)
                {
                    case nameof(Checked):
                    case nameof(DateTime):
                    case nameof(Decimal):
                    case nameof(Image):
                    case nameof(Text):
                        if (!notifying)
                            List.If(i => i?.Initialized == true, i => i.IsModified = true);
                        break;
                }
            }

            ICommand deleteCommand;
            [XmlIgnore]
            public ICommand DeleteCommand
            {
                get
                {
                    deleteCommand = deleteCommand ?? new RelayCommand(() =>
                    {
                        List.Lines.Remove(this);
                        List.Refresh();
                    });
                    return deleteCommand;
                }
            }

            protected override void OnElapsed(ElapsedEventArgs e)
            {
                base.OnElapsed(e);
                if (!editing)
                {
                    notifying = true;
                    this.Changed(() => DateTime);
                    notifying = false;
                }
            }
        }
    }
}