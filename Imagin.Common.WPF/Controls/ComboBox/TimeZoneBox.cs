using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class TimeZoneBox : ComboBox
	{
        Handle handle = false;

		ObservableCollection<TimeZoneInfo> items = new ObservableCollection<TimeZoneInfo>();

		public TimeZoneBox() : base()
		{
            foreach (var i in TimeZoneInfo.GetSystemTimeZones())
                items.Add(i);

            SetCurrentValue(ItemsSourceProperty, items);
            handle = true;
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            //This always gets thrown?
            //handle.If(i => true, i => throw new NotSupportedException());
        }
    }
}