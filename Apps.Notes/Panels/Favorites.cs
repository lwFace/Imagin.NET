using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System.Windows.Input;

namespace Notes
{
    public class FavoritesPanel : Panel
    {
        public override string Title => "Favorites";

        public FavoritesPanel() : base(Resources.Uri(nameof(Notes), "Images/Star.png")) { }

        ICommand goCommand;
        public ICommand GoCommand => goCommand = goCommand ?? new RelayCommand<string>(i => Get.Current<Options>().Folder = i, i => Imagin.Common.Storage.Folder.Long.Exists(i));
    }
}