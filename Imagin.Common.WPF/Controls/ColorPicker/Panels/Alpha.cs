using Imagin.Common.Globalization;
using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class AlphaPanel : Panel
    {
        public override string Title => Localizer.Prefix + "Alpha";

        public override bool TitleVisibility => false;

        public AlphaPanel() : base() { }
    }
}