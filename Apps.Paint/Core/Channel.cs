using Imagin.Common;
using System;
using System.Collections.ObjectModel;

namespace Paint
{
    [Serializable]
    public abstract class Channel : NamedObject
    {
        bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        bool isVisible = true;
        public bool IsVisible
        {
            get => isVisible;
            set => this.Change(ref isVisible, value);
        }

        public abstract double Index { get; }

        public Channel(string name) : base(name) { }
    }

    public class RChannel : Channel
    {
        public override double Index => 0;

        public RChannel() : base("Red") { }
    }

    public class GChannel : Channel
    {
        public override double Index => 1;

        public GChannel() : base("Green") { }
    }

    public class BChannel : Channel
    {
        public override double Index => 2;

        public BChannel() : base("Blue") { }
    }

    public class ChannelCollection : ObservableCollection<Channel>
    {
        public Channel Red => this[0];

        public Channel Green => this[1];

        public Channel Blue => this[2];
    }
}