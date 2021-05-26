using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Timers;

namespace Desktop.Tiles
{
    [Serializable]
    public class ClockTile : Tile
    {
        public readonly Notify Notify;

        public string DateTime => System.DateTime.Now.ToString(Get.Current<Options>().ClockDateTimeFormat);

        public ClockTile() : base()
        {
            Notify = new Notify(1.0.Seconds());
            Notify.Elapsed += OnNotified;
            Notify.Enabled = true;
        }

        void OnNotified(object sender, ElapsedEventArgs e) => IPropertyChangedExtensions.Changed(this, () => DateTime);
    }
}