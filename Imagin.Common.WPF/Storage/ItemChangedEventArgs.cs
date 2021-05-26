using Imagin.Common.Input;
using System;

namespace Imagin.Common.Storage
{
    public class ItemChangedEventArgs : EventArgs<Item>
    {
        public new ItemProperty Parameter => (ItemProperty)base.Parameter;

        public ItemChangedEventArgs(Item item, ItemProperty itemProperty) : base(item, itemProperty) { }
    }

    public class ItemCreatedEventArgs : EventArgs<Item>
    {
        public ItemCreatedEventArgs(Item item) : base(item) { }
    }
    
    public class ItemDeletedEventArgs : EventArgs<string>
    {
        public string Path => Value;

        public ItemDeletedEventArgs(string path) : base(path) { }
    }

    public class ItemRenamedEventArgs : EventArgs
    {
        public readonly string OldPath;

        public readonly string NewPath;

        public ItemRenamedEventArgs(string oldPath, string newPath) : base()
        {
            OldPath = oldPath;
            NewPath = newPath;
        }
    }
}