using System;
using System.Collections.ObjectModel;

namespace Imagin.Common.Storage
{
    [Serializable]
    public class Favorites : ObservableCollection<Favorite> { }
}