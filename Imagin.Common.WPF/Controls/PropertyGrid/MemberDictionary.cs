using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace Imagin.Common.Controls
{
    public delegate T ConvertDelegate<T>(IDictionary<string, object> source);

    public class Entry : Base, INotifyPropertyChanged
    {
        public delegate void EntryChangedEventHandler(object sender, string key, ChangedValue value);

        public event EntryChangedEventHandler Changed;

        string key = string.Empty;
        public string Key
        {
            get => key;
            set => key = value;
        }

        object value = null;
        public object Value
        {
            get => value;
            set
            {
                var oldValue = this.value;
                var newValue = value;
                if (this.Change(ref this.value, newValue))
                    OnValueChanged(key, oldValue, newValue);
            }
        }

        Entry() { }

        public Entry(string key, object value) : this()
        {
            Key = key;
            Value = value;
        }

        protected virtual void OnValueChanged(string key, object oldValue, object newValue)
        {
            Changed?.Invoke(this, key, new ChangedValue(oldValue, newValue));
        }
    }

    public class MemberDictionary<T> : Base, IEnumerable<KeyValuePair<string, object>>
    {
        public delegate void ChangedEventHandler(object sender, T value);

        public event ChangedEventHandler Changed;

        public IDictionary<string, object> Members { get; private set; } = new ExpandoObject() as IDictionary<string, object>;

        public object this[string Key]
        {
            get => Members.TryGetValue(Key, out object Value) ? Value.As<Entry>().Value : null;
            set
            {
                if (Members.TryGetValue(Key, out object Value))
                    Value.As<Entry>().Value = value;
            }
        }

        protected readonly ConvertDelegate<T> Convert;

        public MemberDictionary(ConvertDelegate<T> convert)
        {
            Convert = convert;
            foreach (var i in typeof(T).GetFields())
            {
                if (i.CanWrite())
                    Add(i.Name, i.FieldType.Create<object>());
            }
            foreach (var i in typeof(T).GetProperties())
            {
                if (i.CanWrite)
                    Add(i.Name, i.PropertyType.Create<object>());
            }
        }

        protected void Add(string key, object value)
        {
            if (Members.ContainsKey(key))
                return;

            var entry = new Entry(key, value);
            entry.Changed += OnChanged;
            Members.Add(key, entry);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (KeyValuePair<string, object> i in Members)
                yield return i;
        }

        void OnChanged(object sender, string key, ChangedValue value) => OnChanged();

        protected virtual void OnChanged()
        {
            T result = default;
            Try.Invoke(() => 
            {
                var members = new Dictionary<string, object>();
                foreach (var i in this)
                    members.Add(i.Key, i.Value.To<Entry>().Value);

                result = Convert(members);
            });
            Changed?.Invoke(this, result);
        }
    }
}
