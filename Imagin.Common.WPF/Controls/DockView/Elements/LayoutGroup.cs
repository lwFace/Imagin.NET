using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class LayoutGroup : LayoutElement
    {
        ObservableCollection<LayoutElement> elements = new ObservableCollection<LayoutElement>();
        [XmlArray]
        [XmlArrayItem("Element")]
        public ObservableCollection<LayoutElement> Elements
        {
            get => elements;
            set => this.Change(ref elements, value);
        }

        Orientation orientation = Orientation.Horizontal;
        [XmlAttribute]
        public Orientation Orientation
        {
            get => orientation;
            set => this.Change(ref orientation, value);
        }

        public LayoutGroup() : base() { }

        public LayoutGroup(Orientation orientation) : base()
        {
            Orientation = orientation;
        }
    }
}