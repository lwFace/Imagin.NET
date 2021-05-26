using System.IO;

namespace Imagin.Common.Storage
{
    /// <summary>
    /// Represents a <see cref="Folder"/> or <see cref="Drive"/>.
    /// </summary>
    public abstract class Container : Item
    {
        ItemCollection items = new ItemCollection();
        [Hidden]
        public ItemCollection Items
        {
            get => items;
            private set => this.Change(ref items, value, nameof(Items));
        }

        public override FileSystemInfo Read() => new DirectoryInfo(path);

        protected Container(ItemType type, Origin origin, string path) : base(type, origin, path) { }
    }
}