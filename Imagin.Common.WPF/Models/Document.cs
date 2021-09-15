using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Imagin.Common.Models
{
    [Serializable]
    public abstract class Document : Content
    {
        bool canClose = true;
        [Hidden]
        [XmlIgnore]
        public virtual bool CanClose
        {
            get => canClose;
            set => this.Change(ref canClose, value);
        }

        [field: NonSerialized]
        bool isModified = false;
        [Hidden]
        [XmlIgnore]
        public virtual bool IsModified
        {
            get => isModified;
            set => this.Change(ref isModified, value);
        }

        public Document() : base() { }

        public virtual void OnAdded() { }

        public virtual void OnRemoved() { }
        
        public abstract void Save();

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(IsModified):
                    this.Changed(() => Title);
                    break;
            }
        }
    }
}