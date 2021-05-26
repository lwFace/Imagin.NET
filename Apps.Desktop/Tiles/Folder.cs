using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Storage;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Desktop.Tiles
{
    [Serializable]
    public class FolderTile : Tile
    {
        ItemProperty? groupName = null;
        [XmlIgnore]
        public ItemProperty? GroupName
        {
            get => groupName;
            set => this.Change(ref groupName, value);
        }

        [field:NonSerialized]
        protected ItemCollection items = new ItemCollection();
        [XmlIgnore]
        public virtual ItemCollection Items
        {
            get => items;
            private set => this.Change(ref items, value);
        }

        string path = Folder.Long.Root;
        public string Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        SortDirection sortDirection = SortDirection.Ascending;
        [XmlIgnore]
        public SortDirection SortDirection
        {
            get => sortDirection;
            set => this.Change(ref sortDirection, value);
        }

        ItemProperty sortName = ItemProperty.Name;
        [XmlIgnore]
        public ItemProperty SortName
        {
            get => sortName;
            set => this.Change(ref sortName, value);
        }

        [XmlIgnore]
        public override string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        public FolderTile() : base() { }

        public FolderTile(string path) : base()
        {
            Path = path;
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Path):
                    OnChanged();
                    break;
            }
        }
    }
}