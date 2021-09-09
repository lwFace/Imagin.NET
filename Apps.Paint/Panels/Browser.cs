using Imagin.Common;
using Imagin.Common.Models;

namespace Paint
{
    public class BrowserPanel : Panel
    {
        public override string Title => "Browser";

        public BrowserPanel() : base(Resources.Uri(nameof(Paint), "/Images/Folder.png"))
        {
            IsVisible = false;
        }
    }
}