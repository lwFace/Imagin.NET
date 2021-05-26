using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class PropertiesPanel : Panel
    {
        public override string Title => "Properties";

        public PropertiesPanel() : base(Resources.Uri(AssemblyData.Name, "Images/Properties.png")) { }
    }
}