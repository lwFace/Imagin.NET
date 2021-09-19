using Imagin.Common.Globalization;
using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class PropertiesPanel : Panel
    {
        public override string Title => Localizer.Prefix + "Properties";

        public PropertiesPanel() : base(Resources.Uri(AssemblyData.Name, "Images/Properties.png")) { }
    }
}