using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Imagin.Common.Media
{
    [Serializable]
    public class Palette : ObservableCollection<StringColor>, IPropertyChanged
    {
        [field: NonSerialized]
        new public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        string name;
        public string Name
        {
            get => name;
            set => this.Change(ref name, value);
        }

        public Palette() : base() { }

        public Palette(string name) : base() => Name = name;

        public override string ToString()
        {
            return name;
        }
    }
}