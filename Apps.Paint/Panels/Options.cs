using Imagin.Common;
using Imagin.Common.Models;

namespace Paint
{
    public class OptionsPanel : Panel
    {
        public override string Title => "Options";

        public OptionsPanel() : base(Resources.Uri(nameof(Paint), "Images/Options.png")) { }
    }
}