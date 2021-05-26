using System.Collections.ObjectModel;

namespace Demo
{
    public class HierarchialCollection : ObservableCollection<HierarchialObject>
    {
        public HierarchialCollection() : base() { }
    }
}