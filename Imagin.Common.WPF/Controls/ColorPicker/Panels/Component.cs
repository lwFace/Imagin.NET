using Imagin.Common.Globalization;
using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class ComponentPanel : Panel
    {
        public override string Title => Localizer.Prefix + "Component";

        public override bool TitleVisibility => false;

        public ComponentPanel() : base() { }
    }
}