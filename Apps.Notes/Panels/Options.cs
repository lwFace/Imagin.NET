using Imagin.Common;
using Imagin.Common.Models;

namespace Notes
{
    public class OptionsPanel : Panel
    {
        public override string Title => "Options";

        public OptionsPanel() : base(Resources.Uri(nameof(Notes), "Images/Options.png")) { }
    }
}