using Imagin.Common.Linq;
using Imagin.Common.Time;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Calendar), Type = typeof(System.Windows.Controls.Calendar))]
    [TemplatePart(Name = nameof(PART_DropDown), Type = typeof(Popup))]
    [TemplatePart(Name = nameof(PART_TimeUpDown), Type = typeof(ComboBox))]
    public class DateTimeUpDown : MultiUpDown<DateTime, DateTimePart>
    {
        #region Properties

        Handle handleKind = false;

        new Handle handleValue = false;

        /// ......................................................................................................................

        Popup PART_DropDown { get; set; } = null;

        System.Windows.Controls.Calendar PART_Calendar { get; set; } = null;

        TimeUpDown PART_TimeUpDown { get; set; } = null;

        /// ......................................................................................................................

        public override DateTime AbsoluteMaximum => Convert(DateTime.MaxValue, Kind);

        public override DateTime AbsoluteMinimum => Convert(DateTime.MinValue, Kind);

        public override DateTime DefaultValue
        {
            get
            {
                var now = DateTime.Now;
                switch (Kind)
                {
                    case DateTimeKind.Utc:
                        return DateTime.UtcNow;
                    default:
                        return DateTime.Now;
                }
            }
        }

        /// ......................................................................................................................

        /// <summary>
        /// The value to increment by (e.g., if <see cref="SelectedPart" /> = <see cref="DateTimePart.Month" /> and <see cref="Increment" /> = 3, increment by 3 months).
        /// </summary>
        protected double Increment { get; set; } = 1d;
        
        public static DependencyProperty CalendarModeProperty = DependencyProperty.Register(nameof(CalendarMode), typeof(CalendarMode), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(CalendarMode.Month, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public CalendarMode CalendarMode
        {
            get => (CalendarMode)GetValue(CalendarModeProperty);
            set => SetValue(CalendarModeProperty, value);
        }

        public static DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(DateTimeKind), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(DateTimeKind.Local, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnKindChanged));
        public DateTimeKind Kind
        {
            get => (DateTimeKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }
        static void OnKindChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DateTimeUpDown>().OnKindChanged(new OldNew<DateTimeKind>(e));

        public static DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));
        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }
        static void OnIsDropDownOpenChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DateTimeUpDown>().OnIsDropDownOpenChanged(new OldNew<bool>(e));

        public static DependencyProperty DropDownAnimationProperty = DependencyProperty.Register(nameof(DropDownAnimation), typeof(PopupAnimation), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(PopupAnimation.Fade, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public PopupAnimation DropDownAnimation
        {
            get => (PopupAnimation)GetValue(DropDownAnimationProperty);
            set => SetValue(DropDownAnimationProperty, value);
        }

        public static DependencyProperty DropDownPlacementProperty = DependencyProperty.Register(nameof(DropDownPlacement), typeof(PlacementMode), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(PlacementMode.Bottom, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public PlacementMode DropDownPlacement
        {
            get => (PlacementMode)GetValue(DropDownPlacementProperty);
            set => SetValue(DropDownPlacementProperty, value);
        }

        public static DependencyProperty DropDownStretchProperty = DependencyProperty.Register(nameof(DropDownStretch), typeof(Stretch), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(Stretch.Fill, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the drop down stretch. If <see cref="Stretch.None"/>, drop down width assumes width of it's content; if anything else, drop down width assumes width of parent, <see cref="DateTimeUpDown"/>.
        /// </summary>
        public Stretch DropDownStretch
        {
            get => (Stretch)GetValue(DropDownStretchProperty);
            set => SetValue(DropDownStretchProperty, value);
        }

        public static DependencyProperty DropDownStyleProperty = DependencyProperty.Register(nameof(DropDownStyle), typeof(Style), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets style of drop down; style must target <see cref="Border"/> control.
        /// </summary>
        public Style DropDownStyle
        {
            get => (Style)GetValue(DropDownStyleProperty);
            set => SetValue(DropDownStyleProperty, value);
        }

        public static DependencyProperty SelectedPartProperty = DependencyProperty.Register(nameof(SelectedPart), typeof(DateTimePart), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(DateTimePart.Day, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the selected <see cref="DateTimePart"/> of the <see cref="DateTime"/> value.
        /// </summary>
        public DateTimePart SelectedPart
        {
            get => (DateTimePart)GetValue(SelectedPartProperty);
            set => SetValue(SelectedPartProperty, value);
        }

        public static DependencyProperty StaysOpenProperty = DependencyProperty.Register(nameof(StaysOpen), typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not the drop down stays open when clicking neutral area outside of it.
        /// </summary>
        public bool StaysOpen
        {
            get => (bool)GetValue(StaysOpenProperty);
            set => SetValue(StaysOpenProperty, value);
        }

        public static DependencyProperty StaysOpenOnSelectionProperty = DependencyProperty.Register(nameof(StaysOpenOnSelection), typeof(bool), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not the drop down stays open after making a selection.
        /// </summary>
        public bool StaysOpenOnSelection
        {
            get => (bool)GetValue(StaysOpenOnSelectionProperty);
            set => SetValue(StaysOpenOnSelectionProperty, value);
        }

        public static DependencyProperty TimeOfDayProperty = DependencyProperty.Register(nameof(TimeOfDay), typeof(TimeSpan), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(default(TimeSpan), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TimeSpan TimeOfDay
        {
            get => (TimeSpan)GetValue(TimeOfDayProperty);
            private set => SetValue(TimeOfDayProperty, value);
        }
        
        #endregion

        #region DateTimeUpDown

        public DateTimeUpDown() : base() => DefaultStyleKey = typeof(DateTimeUpDown);

        #endregion

        #region Methods

        DateTime Convert(DateTime dateTime, DateTimeKind kind)
        {
            if (dateTime.Kind == kind)
                return dateTime;

            if (dateTime.Kind == DateTimeKind.Unspecified || kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dateTime, kind);

            if (kind == DateTimeKind.Local)
            {
                return dateTime.ToLocalTime();
            }
            else return dateTime.ToUniversalTime();
        }

        void Increase(double increment)
        {
            var result = default(DateTime);
            if (Value != default(DateTime))
            {
                try
                {
                    switch (SelectedPart)
                    {
                        case DateTimePart.Day:
                            result = Value.AddDays(increment);
                            break;
                        case DateTimePart.Hour:
                            result = Value.AddHours(increment);
                            break;
                        case DateTimePart.Meridian:
                            switch (Value.Meridiem())
                            {
                                case Meridiem.AM:
                                    result = Value.AddHours(12d);
                                    break;
                                case Meridiem.PM:
                                    result = Value.AddHours(-12d);
                                    break;
                                default:
                                    result = Value;
                                    break;
                            }
                            break;
                        case DateTimePart.Millisecond:
                            result = Value.AddMilliseconds(increment);
                            break;
                        case DateTimePart.Minute:
                            result = Value.AddMinutes(increment);
                            break;
                        case DateTimePart.Month:
                            result = Value.AddMonths(increment.Round().Int32());
                            break;
                        case DateTimePart.Second:
                            result = Value.AddSeconds(increment);
                            break;
                        case DateTimePart.Year:
                            result = Value.AddYears(increment.Round().Int32());
                            break;
                    }
                }
                catch
                {
                    //An error may occur if the result is "unrepresentable"
                    result = Value;
                }
            }
            else result = Minimum;

            SetCurrentValue(ValueProperty.Property, result);
        }

        void OnTimeChanged(object sender, TextChangedEventArgs e)
        {
            var value = PART_TimeUpDown.Value;

            if (value != null)
                Value = Value.Date.AddHours(value.Hours).AddMinutes(value.Minutes).AddSeconds(value.Seconds).AddMilliseconds(value.Milliseconds);
        }

        /// ......................................................................................................................

        protected override bool CanDecrease() => Value > Minimum;

        protected override bool CanIncrease() => Value < Maximum;

        protected override DateTime GetValue(string input) => input.DateTime();

        protected override object OnMaximumCoerced(object input)
        {
            var result = input.As<DateTime>().Coerce(AbsoluteMaximum, Value);
            result = Convert(result, Kind);
            return result;
        }

        protected override object OnMinimumCoerced(object input)
        {
            var result = input.As<DateTime>().Coerce(Value, AbsoluteMinimum);
            result = Convert(result, Kind);
            return result;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            SetCurrentValue(TimeOfDayProperty, Value.TimeOfDay);
        }

        protected override void OnValueChanged(OldNew<DateTime> input)
        {
            base.OnValueChanged(input);
            if (!handleValue)
            {
                handleKind = true;
                SetCurrentValue(KindProperty, input.New.Kind);
                handleKind = false;
            }

            SetCurrentValue(TimeOfDayProperty, input.New.TimeOfDay);
        }

        protected override object OnValueCoerced(object input)
        {
            var result = input.As<DateTime>().Coerce(Maximum, Minimum);

            if (Kind != DateTimeKind.Unspecified)
                result = Convert(result, Kind);

            return result;
        }

        protected override string ToString(DateTime input) => input.ToString(CultureInfo.CurrentCulture);

        /// ......................................................................................................................

        public override void Decrease() => Increase(-Increment);

        public override void Increase() => Increase(Increment);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_TimeUpDown = Template.FindName(nameof(PART_TimeUpDown), this) as TimeUpDown;
            if (PART_TimeUpDown != null)
                PART_TimeUpDown.TextChanged += OnTimeChanged;

            PART_Calendar = Template.FindName(nameof(PART_Calendar), this) as System.Windows.Controls.Calendar;
            if (PART_Calendar != null)
                PART_Calendar.SelectedDatesChanged += OnSelectedDatesChanged;

            PART_DropDown = Template.FindName(nameof(PART_DropDown), this) as Popup;
            if (PART_DropDown != null)
                PART_DropDown.Closed += OnDropDownClosed;
        }

        /// ......................................................................................................................

        protected virtual void OnDropDownClosed(object sender, EventArgs e)
        {
            if (IsDropDownOpen)
                IsDropDownOpen = false;
        }

        protected virtual void OnIsDropDownOpenChanged(OldNew<bool> input)
        {
            if (PART_DropDown != null)
                PART_DropDown.IsOpen = input.New;
        }

        protected virtual void OnKindChanged(OldNew<DateTimeKind> input)
        {
            if (!handleKind)
            {
                handleValue = true;
                SetCurrentValue(ValueProperty.Property, Convert(Value, input.New));
                handleValue = false;
            }

            SetCurrentValue(MaximumProperty.Property, Convert(Maximum, input.New));
            SetCurrentValue(MinimumProperty.Property, Convert(Minimum, input.New));
        }

        protected virtual void OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.First()?.As<DateTime>();

            if (item != null)
                Value = item.Value.Date.AddHours(Value.TimeOfDay.Hours).AddMinutes(Value.TimeOfDay.Minutes).AddSeconds(Value.TimeOfDay.Seconds).AddMilliseconds(Value.TimeOfDay.Milliseconds);

            if (!StaysOpenOnSelection)
                SetCurrentValue(IsDropDownOpenProperty, false);
        }

        #endregion
    }
}