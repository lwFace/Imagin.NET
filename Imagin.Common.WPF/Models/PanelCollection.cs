using Imagin.Common.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Models
{
    public sealed class PanelCollection : Collection<Panel>
    {
        public Panel this[string name] => this.FirstOrDefault(i => i.Name == name);

        public PanelCollection() : base() { }
    }
}