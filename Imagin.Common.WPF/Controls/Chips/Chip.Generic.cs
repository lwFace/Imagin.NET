using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public abstract class Chip<T> : ChipBase where T : Brush
    {
        public event EventHandler<EventArgs<T>> ValueChanged;

        public static readonly DependencyProperty<T, Chip<T>> ValueProperty = new DependencyProperty<T, Chip<T>>(nameof(Value), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        public T Value
        {
            get => ValueProperty.Get(this);
            set => ValueProperty.Set(this, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Chip<T>>().OnValueChanged(new OldNew<T>(e));

        public Chip() : base() { }

        protected virtual void OnValueChanged(OldNew<T> input) => ValueChanged?.Invoke(this, new EventArgs<T>(input.New));
    }
}