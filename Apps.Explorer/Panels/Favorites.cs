using Imagin.Common;
using Imagin.Common.Models;

namespace Explorer
{
    public class FavoritesPanel : Panel
    {
        public override string Title => "Favorites";

        public FavoritesPanel() : base(Resources.Uri(nameof(Explorer), "Images/Star.png")) { }
    }
}