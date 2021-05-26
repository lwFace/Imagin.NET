using Imagin.Common;
using Imagin.Common.Models;

namespace Explorer
{
    public class OptionsPanel : Panel
    {
        public override string Title => "Options";

        public OptionsPanel() : base(Resources.Uri(nameof(Explorer), "/Images/Options.png")) { }
    }
}