using Imagin.Common;
using Imagin.Common.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Paint.Adjust
{
    public class AdjustmentEffectCollection : ObservableCollection<AdjustmentEffect>, IPropertyChanged
    {
        new public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<EventArgs<AdjustmentEffect>> Added;

        public event EventHandler<EventArgs> Changed;

        public event EventHandler<EventArgs> Cleared;

        public event EventHandler<EventArgs<AdjustmentEffect>> Removed;

        public bool Any => Count > 0;

        new public void Add(AdjustmentEffect i)
        {
            base.Add(i);
            Changed?.Invoke(this, EventArgs.Empty);
            Added?.Invoke(this, new EventArgs<AdjustmentEffect>(i));
            IPropertyChangedExtensions.Changed(this, () => Any);
            IPropertyChangedExtensions.Changed(this, () => Count);
        }

        new public void Insert(int index, AdjustmentEffect i)
        {
            base.Insert(index, i);
            Changed?.Invoke(this, EventArgs.Empty);
            Added?.Invoke(this, new EventArgs<AdjustmentEffect>(i));
            IPropertyChangedExtensions.Changed(this, () => Any);
            IPropertyChangedExtensions.Changed(this, () => Count);
        }

        new public void Remove(AdjustmentEffect i)
        {
            base.Add(i);
            Changed?.Invoke(this, EventArgs.Empty);
            Removed?.Invoke(this, new EventArgs<AdjustmentEffect>(i));
            IPropertyChangedExtensions.Changed(this, () => Any);
            IPropertyChangedExtensions.Changed(this, () => Count);
        }

        new public void RemoveAt(int index)
        {
            var i = this[index];
            base.RemoveAt(index);
            Changed?.Invoke(this, EventArgs.Empty);
            Removed?.Invoke(this, new EventArgs<AdjustmentEffect>(i));
            IPropertyChangedExtensions.Changed(this, () => Any);
            IPropertyChangedExtensions.Changed(this, () => Count);
        }

        new public void Clear()
        {
            base.Clear();
            Changed?.Invoke(this, EventArgs.Empty);
            Cleared?.Invoke(this, EventArgs.Empty);
            IPropertyChangedExtensions.Changed(this, () => Any);
            IPropertyChangedExtensions.Changed(this, () => Count);
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public override string ToString() => "Adjustments";
    }
}