using Imagin.Common;
using System;
using System.Runtime.CompilerServices;

namespace Desktop.Tiles
{
    [Serializable]
    public class NoteTile : Tile
    {
        string text;
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        public NoteTile() : base() { }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Text):
                    OnChanged();
                    break;
            }
        }
    }
}