using Imagin.Common;
using Imagin.Common.Models;

namespace Demo
{
    public class PropertiesPanel : Panel
    {
        public override string Title => "Properties";

        public PropertiesPanel() : base(Resources.Uri(nameof(Demo), "/Images/Properties.png")) { }
    }
}