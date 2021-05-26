using Imagin.Common;
using Imagin.Common.Models;

namespace Demo
{
    public class OptionsPanel : Panel
    {
        public override bool CanHide => false;

        public override string Title => "Options";

        public OptionsPanel() : base(Resources.Uri(nameof(Demo), "/Images/Gear.png")) { }
    }
}