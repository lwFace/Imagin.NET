using Imagin.Common;
using System.Collections.ObjectModel;

namespace Demo
{
    public class HierarchialObject : NamedObject
    {
        ObservableCollection<HierarchialObject> items = null;
        public ObservableCollection<HierarchialObject> Items
        {
            get => items;
            set => this.Change(ref items, value);
        }

        public HierarchialObject(string Name) : base(Name)
        {
            Items = new ObservableCollection<HierarchialObject>();
        }
    }
}