using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Time;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public partial class TimeSpanView : System.Windows.Controls.UserControl
    {
        public ObservableCollection<int> Hours { get; private set; } = new ObservableCollection<int>();

        public ObservableCollection<int> Minutes { get; private set; } = new ObservableCollection<int>();

        public ObservableCollection<Meridiem> Meridiems { get; private set; } = new ObservableCollection<Meridiem>();

        public static DependencyProperty HourProperty = DependencyProperty.Register(nameof(Hour), typeof(int), typeof(TimeSpanView), new PropertyMetadata(12));
        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set => SetValue(HourProperty, value);
        }

        public static DependencyProperty HourStyleProperty = DependencyProperty.Register(nameof(HourStyle), typeof(Style), typeof(TimeSpanView), new PropertyMetadata(null));
        public Style HourStyle
        {
            get => (Style)GetValue(HourStyleProperty);
            set => SetValue(HourStyleProperty, value);
        }

        public static DependencyProperty MinuteProperty = DependencyProperty.Register(nameof(Minute), typeof(int), typeof(TimeSpanView), new PropertyMetadata(0));
        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, value);
        }

        public static DependencyProperty MinuteStyleProperty = DependencyProperty.Register(nameof(MinuteStyle), typeof(Style), typeof(TimeSpanView), new PropertyMetadata(null));
        public Style MinuteStyle
        {
            get => (Style)GetValue(MinuteStyleProperty);
            set => SetValue(MinuteStyleProperty, value);
        }

        public static DependencyProperty MeridiemProperty = DependencyProperty.Register(nameof(Meridiem), typeof(Meridiem), typeof(TimeSpanView), new PropertyMetadata(Meridiem.PM));
        public Meridiem Meridiem
        {
            get => (Meridiem)GetValue(MeridiemProperty);
            set => SetValue(MeridiemProperty, value);
        }

        public static DependencyProperty MeridiemStyleProperty = DependencyProperty.Register(nameof(MeridiemStyle), typeof(Style), typeof(TimeSpanView), new PropertyMetadata(null));
        public Style MeridiemStyle
        {
            get => (Style)GetValue(MeridiemStyleProperty);
            set => SetValue(MeridiemStyleProperty, value);
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(TimeSpan), typeof(TimeSpanView), new PropertyMetadata(TimeSpan.Zero));
        public TimeSpan Value
        {
            get => (TimeSpan)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public TimeSpanView()
        {
            InitializeComponent();

            for (var i = 1; i <= 12; i++)
                Hours.Add(i);

            for (var i = 0; i < 60; i += 5)
                Minutes.Add(i);

            foreach (var i in Enum.GetValues(typeof(Meridiem)).Cast<Meridiem>())
                Meridiems.Add(i);

            this.Bind(HourProperty, nameof(Value), this, BindingMode.TwoWay, new DefaultConverter<TimeSpan, int>
            (
                i => i.Hours > 12 ? i.Hours - 12 : i.Hours == 0 ? 12 : i.Hours,
                i => Update(i, Minute, Meridiem)
            ));
            this.Bind(MinuteProperty, nameof(Value), this, BindingMode.TwoWay, new DefaultConverter<TimeSpan, int>
            (
                i => i.Minutes,
                i => Update(Hour, i, Meridiem)
            ));
            this.Bind(MeridiemProperty, nameof(Value), this, BindingMode.TwoWay, new DefaultConverter<TimeSpan, Meridiem>
            (
                i => i.Hours >= 12 ? Meridiem.PM : Meridiem.AM,
                i => Update(Hour, Minute, i)
            ));
        }

        TimeSpan Update(int hour, int minute, Meridiem meridiem)
        {
            var result = TimeSpan.Zero;
            if (hour == 12)
            {
                if (meridiem == Meridiem.PM)
                    result = result.Add(TimeSpan.FromHours(hour));
            }
            else result = result.Add(TimeSpan.FromHours(meridiem == Meridiem.PM ? hour + 12 : hour));

            result = result.Add(TimeSpan.FromMinutes(minute));
            return result;
        }
    }
}