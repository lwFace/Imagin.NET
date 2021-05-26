using Imagin.Common.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Imagin.Common.Controls
{
    public abstract class NumericUpDown<T> : UpDown<T>
    {
        protected const string Comma = ",";

        protected const string Dash = "-";

        protected const string Period = ".";

        public abstract T DefaultIncrement { get; }

        public virtual Regex Expression => new Regex("^[0-9]?$");

        public static readonly DependencyProperty<T, NumericUpDown<T>> IncrementProperty = new DependencyProperty<T, NumericUpDown<T>>(nameof(Increment), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIncrementChanged));
        public T Increment
        {
            get => IncrementProperty.Get(this);
            set => IncrementProperty.Set(this, value);
        }
        static void OnIncrementChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<NumericUpDown<T>>().OnIncrementChanged(new OldNew<T>(e));

        public static readonly DependencyProperty<string, NumericUpDown<T>> StringFormatProperty = new DependencyProperty<string, NumericUpDown<T>>(nameof(StringFormat), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStringFormatChanged));
        public string StringFormat
        {
            get => StringFormatProperty.Get(this);
            set => StringFormatProperty.Set(this, value);
        }
        static void OnStringFormatChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<NumericUpDown<T>>().OnStringFormatChanged(new OldNew<string>(e));

        public NumericUpDown() : base()
        {
            SetCurrentValue(IncrementProperty.Property, DefaultIncrement);
        }

        protected virtual void OnIncrementChanged(OldNew<T> input) { }

        protected virtual void OnStringFormatChanged(OldNew<string> input)
        {
            OnValueChanged(new OldNew<T>(default, Value));
        }
    }
}