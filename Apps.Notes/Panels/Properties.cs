using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Models;

namespace Notes
{
    public class PropertiesPanel : Panel
    {
        public override string Title => "Properties";

        public PropertiesPanel() : base(Resources.Uri(nameof(Notes), "Images/Properties.png")) { }
    }
}