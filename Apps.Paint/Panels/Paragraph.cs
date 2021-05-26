using Imagin.Common;
using Imagin.Common.Models;

namespace Paint
{
    public class ParagraphPanel : Panel
    {
        public override string Title => "Paragraph";

        public ParagraphPanel() : base(Resources.Uri(nameof(Paint), "/Images/AlignCenter.png"))
        {
            IsVisible = false;
        }
    }
}