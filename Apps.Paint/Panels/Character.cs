using Imagin.Common;
using Imagin.Common.Models;

namespace Paint
{
    public class CharacterPanel : Panel
    {
        public override string Title => "Character";

        public CharacterPanel() : base(Resources.Uri(nameof(Paint), "/Images/Text.png"))
        {
            IsVisible = false;
        }
    }
}