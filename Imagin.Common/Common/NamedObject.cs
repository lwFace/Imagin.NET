using Imagin.Common.Input;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="INamable"/>.
    /// </summary>
    [Serializable]
    public class NamedObject : Base, INamable
    {
        [field:NonSerialized]
        public event EventHandler<EventArgs<string>> NameChanged;

        string _name = string.Empty;
        public virtual string Name
        {
            get => _name;
            set
            {
                this.Change(ref _name, OnPreviewNameChanged(_name, value));
                OnNameChanged(_name);
            }
        }

        public NamedObject() : base() { }

        public NamedObject(string name) : base() => Name = name;

        protected virtual void OnNameChanged(string Value) => NameChanged?.Invoke(this, new EventArgs<string>(Value));

        protected virtual string OnPreviewNameChanged(string OldValue, string NewValue) => NewValue;

        public override string ToString() => Name;
    }
}
