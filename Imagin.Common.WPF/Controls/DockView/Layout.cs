using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    [XmlRoot("Layout")]
    public class Layout : Base
    {
        List<LayoutWindow> floating = new List<LayoutWindow>();
        [XmlArray]
        [XmlArrayItem(ElementName = "Window")]
        public List<LayoutWindow> Floating
        {
            get => floating;
            set => this.Change(ref floating, value);
        }

        LayoutElement root;
        public LayoutElement Root
        {
            get => root;
            set => this.Change(ref root, value);
        }

        public Layout() : base() { }

        public T First<T>(LayoutGroup parent = null) where T : LayoutElement
        {
            parent = parent ?? root as LayoutGroup;
            if (parent is T)
                return (T)(LayoutElement)parent;

            if (parent != null)
            {
                foreach (var i in parent.Elements)
                {
                    if (i is T)
                        return (T)i;

                    if (i is LayoutGroup j)
                    {
                        var result = First<T>(j);
                        if (result != null)
                            return result;
                    }
                }
            }
            return default;
        }
    }
}