using System;
using System.Xml.Serialization;

namespace Imagin.Common.Models
{
    [Serializable]
    public abstract class Content : ViewModel
    {
        [field: NonSerialized]
        bool canFloat = true;
        [Hidden]
        [XmlIgnore]
        public virtual bool CanFloat
        {
            get => canFloat;
            set => this.Change(ref canFloat, value);
        }

        [Hidden]
        [XmlIgnore]
        public virtual string Title { get; }

        [Hidden]
        [XmlIgnore]
        public virtual object ToolTip { get; }// = null;
    }
}