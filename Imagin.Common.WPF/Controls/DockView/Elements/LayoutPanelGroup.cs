using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class LayoutPanelGroup : LayoutElement
    {
        List<LayoutPanel> panels = new List<LayoutPanel>();
        [XmlArray]
        public List<LayoutPanel> Panels
        {
            get => panels;
            set => this.Change(ref panels, value);
        }

        public LayoutPanelGroup() : base() { }

        public LayoutPanelGroup(IEnumerable<LayoutPanel> input) : base() => input?.ForEach(i => panels.Add(i));

        public LayoutPanelGroup(params LayoutPanel[] input) : this(input as IEnumerable<LayoutPanel>) { }

        public LayoutPanelGroup(params Panel[] input) : this(input?.Select(i => new LayoutPanel((i as Panel).Name))) { }

        public LayoutPanelGroup(IEnumerable<Panel> input) : this(input?.ToArray()) { }

        public LayoutPanelGroup(params string[] input) : this(input?.Select(i => new LayoutPanel(i.ToString()))) { }
    }
}