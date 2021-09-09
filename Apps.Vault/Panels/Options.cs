using Imagin.Common;
using Imagin.Common.Models;

namespace Vault
{
    public class OptionsPanel : Panel
    {
        public override string Title => "Options";

        public OptionsPanel() : base(Resources.Uri(nameof(Vault), "Images/Options.png")) { }
    }
}