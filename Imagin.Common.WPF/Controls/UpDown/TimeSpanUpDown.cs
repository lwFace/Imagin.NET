using Imagin.Common.Linq;
using Imagin.Common.Time;
using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class TimeSpanUpDown : MultiUpDown<TimeSpan, TimePart>
    {
        bool SelectionChangeHandled;

        public override TimeSpan AbsoluteMaximum => TimeSpan.MaxValue;

        public override TimeSpan AbsoluteMinimum => TimeSpan.MinValue;

        public override TimeSpan DefaultValue => TimeSpan.Zero;

        public static DependencyProperty IncrementProperty = DependencyProperty.Register(nameof(Increment), typeof(double), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double Increment
        {
            get => (double)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        public static DependencyProperty SelectedPartProperty = DependencyProperty.Register(nameof(SelectedPart), typeof(TimePart), typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimePart.Hour, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the selected <see cref="TimePart"/> of the <see cref="TimeSpan"/>.
        /// </summary>
        public TimePart SelectedPart
        {
            get => (TimePart)GetValue(SelectedPartProperty);
            set => SetValue(SelectedPartProperty, value);
        }

        public TimeSpanUpDown() : base() { }

        void Increase(double increment)
        {
            var result = TimeSpan.Zero;

            if (Value != default(TimeSpan))
            {
                try
                {
                    switch (SelectedPart)
                    {
                        case TimePart.Hour:
                            result = Value + TimeSpan.FromHours(increment);
                            break;
                        case TimePart.Minute:
                            result = Value + TimeSpan.FromMinutes(increment);
                            break;
                        case TimePart.Second:
                            result = Value + TimeSpan.FromSeconds(increment);
                            break;
                    }
                }
                catch
                {
                    result = Value;
                }
            }
            else result = Minimum;

            SetCurrentValue(ValueProperty.Property, result);
        }

        protected override bool CanDecrease() => Value > Minimum;

        protected override bool CanIncrease() => Value < Maximum;

        protected override TimeSpan GetValue(string input) => input.TimeSpan();

        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (!SelectionChangeHandled)
            {
                var targetIndex = CaretIndex;

                var end = 0;
                var start = 0;
                var found = false;

                var part = TimePart.Hour;
                var length = Text.Length;

                var index = 0;
                foreach (var @char in Text)
                {
                    if (index == targetIndex)
                    {
                        found = true;
                        end = length;
                    }

                    if (@char == ':')
                    {
                        if (found)
                        {
                            end = index;
                            break;
                        }

                        switch (part)
                        {
                            case TimePart.Hour:
                                part = TimePart.Minute;
                                break;
                            case TimePart.Minute:
                                part = TimePart.Second;
                                break;
                        }

                        start = index + 1;
                    }

                    index++;
                }

                start = start.Coerce(length);
                length = (end - start).Coerce(length);

                if (SelectionStart!= start && SelectionLength != length)
                {
                    SelectionChangeHandled = true;
                    Select(start, length);
                    SelectionChangeHandled = false;
                }

                SetCurrentValue(SelectedPartProperty, part);
            }
        }

        protected override object OnMaximumCoerced(object input) => input.As<TimeSpan>().Coerce(AbsoluteMaximum, Value);

        protected override object OnMinimumCoerced(object input) => input.As<TimeSpan>().Coerce(Value, AbsoluteMinimum);

        protected override object OnValueCoerced(object input) => input.As<TimeSpan>().Coerce(Maximum, Minimum);

        protected override string ToString(TimeSpan input) => input.ToString();

        public override void Decrease() => Increase(-Increment);

        public override void Increase() => Increase(Increment);
    }
}