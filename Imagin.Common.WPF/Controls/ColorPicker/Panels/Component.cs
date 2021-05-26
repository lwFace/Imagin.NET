using Imagin.Common.Models;

namespace Imagin.Common.Controls
{
    public class ComponentPanel : Panel
    {
        public override string Title => "Component";

        public override bool TitleVisibility => false;

        public ComponentPanel() : base() { }
    }
}