using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Timers;

namespace Desktop.Tiles
{
    [Serializable]
    public class CountDownTile : Tile
    {
        public readonly Notify Notify;

        DateTime date = DateTime.Now;
        public DateTime Date
        {
            get => date;
            set => this.Change(ref date, value);
        }

        public CountDownTile() : base()
        {
            Notify = new Notify(1.0.Seconds());
            Notify.Elapsed += OnNotified;
            Notify.Enabled = true;
        }

        void OnNotified(object sender, ElapsedEventArgs e) => IPropertyChangedExtensions.Changed(this, () => Date);
    }
}