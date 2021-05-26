using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public abstract class UpDown<T> : UpDown, IUpDown<T>
    {
        #region Properties

        protected Handle handleText { get; set; } = false;

        protected Handle handleValue { get; set; } = false;

        /// <summary>
        /// The absolute maximum value possible.
        /// </summary>
        public abstract T AbsoluteMaximum { get; }

        /// <summary>
        /// The absolute minimum value possible.
        /// </summary>
        public abstract T AbsoluteMinimum { get; }

        /// <summary>
        /// The default value.
        /// </summary>
        public abstract T DefaultValue { get; }

        public static readonly DependencyProperty<T, UpDown<T>> MaximumProperty = new DependencyProperty<T, UpDown<T>>(nameof(Maximum), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.None, OnMaximumChanged, OnMaximumCoerced));
        public T Maximum
        {
            get => MaximumProperty.Get(this);
            set => MaximumProperty.Set(this, value);
        }
        static void OnMaximumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UpDown<T>>().OnMaximumChanged(new OldNew<T>(e));
        static object OnMaximumCoerced(DependencyObject i, object Value) => i.As<UpDown<T>>().OnMaximumCoerced(Value);

        public static readonly DependencyProperty<T, UpDown<T>> MinimumProperty = new DependencyProperty<T, UpDown<T>>(nameof(Minimum), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.None, OnMinimumChanged, OnMinimumCoerced));
        public T Minimum
        {
            get => MinimumProperty.Get(this);
            set => MinimumProperty.Set(this, value);
        }
        static void OnMinimumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UpDown<T>>().OnMinimumChanged(new OldNew<T>(e));
        static object OnMinimumCoerced(DependencyObject i, object Value) => i.As<UpDown<T>>().OnMinimumCoerced(Value);

        public static readonly DependencyProperty<T, UpDown<T>> ValueProperty = new DependencyProperty<T, UpDown<T>>(nameof(Value), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnValueCoerced));
        public T Value
        {
            get => ValueProperty.Get(this);
            set => ValueProperty.Set(this, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UpDown<T>>().OnValueChanged(new OldNew<T>(e));
        static object OnValueCoerced(DependencyObject i, object Value) => i.As<UpDown<T>>().OnValueCoerced(Value);

        #endregion

        #region UpDown

        public UpDown() : base()
        {
            SetCurrentValue(MaximumProperty.Property, AbsoluteMaximum);
            SetCurrentValue(MinimumProperty.Property, AbsoluteMinimum);
            SetCurrentValue(ValueProperty.Property, DefaultValue);
        }

        #endregion

        #region Methods

        #region Abstract

        protected abstract T GetValue(string Value);

        protected abstract object OnMaximumCoerced(object Value);

        protected abstract object OnMinimumCoerced(object Value);

        protected abstract object OnValueCoerced(object Value);

        protected abstract string ToString(T Value);

        #endregion

        #region Overrides

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!handleText)
            {
                handleValue = true;
                Value = GetValue(Text);

                //If current value does not match up with text, make it so
                var i = ToString(Value);
                if (Text != i)
                {
                    handleText = true;
                    Text = i;
                    handleText = false;
                }

                handleValue = false;
            }
        }

        public sealed override void ValueToMaximum() => SetCurrentValue(ValueProperty.Property, Maximum);

        public sealed override void ValueToMinimum() => SetCurrentValue(ValueProperty.Property, Minimum);

        #endregion

        #region Virtual

        protected virtual void OnMaximumChanged(OldNew<T> input) { }

        protected virtual void OnMinimumChanged(OldNew<T> input) { }

        protected virtual void OnValueChanged(OldNew<T> input)
        {
            if (!handleValue)
            {
                handleText = true;
                SetText(ToString(input.New));
                handleText = false;
            }
        }

        #endregion

        #endregion
    }
}