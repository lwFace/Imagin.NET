using Imagin.Common.Globalization;
using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class OptionsPanel : Panel
    {
        public override string Title => Localizer.Prefix + "Options";

        public OptionsPanel() : base(Resources.Uri(AssemblyData.Name, "Images/Options.png")) { }
    }
}