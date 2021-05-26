using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public abstract class ParseBox<T> : TextBox
    {
        Handle handleText = false;

        Handle handleValue = false;

        public static readonly DependencyProperty<T, ParseBox<T>> ValueProperty = new DependencyProperty<T, ParseBox<T>>(nameof(Value), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnValueCoerced));
        public T Value
        {
            get => ValueProperty.Get(this);
            set => ValueProperty.Set(this, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ParseBox<T>>().OnValueChanged(new OldNew<T>(e));
        static object OnValueCoerced(DependencyObject i, object value) => i.As<ParseBox<T>>().OnValueCoerced(value);

        public ParseBox() : base() { }

        protected void SetText(string text)
        {
            var i = CaretIndex;
            SetCurrentValue(TextProperty, text);
            CaretIndex = i;
        }

        protected abstract T GetValue(string value);

        protected abstract string ToString(T Value);

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!handleText)
            {
                handleValue = true;
                SetCurrentValue(ValueProperty.Property, GetValue(Text));

                var i = ToString(Value);
                if (Text != i)
                {
                    handleText = true;
                    SetCurrentValue(TextProperty, i);
                    handleText = false;
                }

                handleValue = false;
            }
        }

        protected virtual void OnValueChanged(OldNew<T> input)
        {
            if (!handleValue)
            {
                handleText = true;
                SetText(ToString(input.New));
                handleText = false;
            }
        }

        protected virtual object OnValueCoerced(object input) => input;
    }
}