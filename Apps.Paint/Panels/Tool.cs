using Imagin.Common;
using Imagin.Common.Models;

namespace Paint
{
    public class ToolPanel : Panel
    {
        Tool tool;
        public Tool Tool
        {
            get => tool;
            set => this.Change(ref tool, value);
        }

        public override string Title => "Tool";

        public ToolPanel() : base(Resources.Uri(nameof(Paint), "/Images/Wrench.png"))
        {
            TitleVisibility = false;
        }
    }
}