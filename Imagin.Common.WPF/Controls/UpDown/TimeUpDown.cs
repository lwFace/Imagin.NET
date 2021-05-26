using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_DropDown), Type = typeof(Popup))]
    [TemplatePart(Name = nameof(PART_Options), Type = typeof(ListBox))]
    public class TimeUpDown : TimeSpanUpDown
    {
        Handle handleOption;

        Popup PART_DropDown;

        ListBox PART_Options;

        public event EventHandler<EventArgs<TimeSpan>> OptionSelected;

        public static DependencyProperty DropDownStyleProperty = DependencyProperty.Register(nameof(DropDownStyle), typeof(Style), typeof(TimeUpDown), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style DropDownStyle
        {
            get => (Style)GetValue(DropDownStyleProperty);
            set => SetValue(DropDownStyleProperty, value);
        }

        public static DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof(IsDropDownOpen), typeof(bool), typeof(TimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));
        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }
        static void OnIsDropDownOpenChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TimeUpDown>().OnIsDropDownOpenChanged(new OldNew<bool>(e));

        public static DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof(MaxDropDownHeight), typeof(double), typeof(TimeUpDown), new FrameworkPropertyMetadata(360d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double MaxDropDownHeight
        {
            get => (double)GetValue(MaxDropDownHeightProperty);
            set => SetValue(MaxDropDownHeightProperty, value);
        }
        
        public static DependencyProperty StaysOpenProperty = DependencyProperty.Register(nameof(StaysOpen), typeof(bool), typeof(TimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool StaysOpen
        {
            get => (bool)GetValue(StaysOpenProperty);
            set => SetValue(StaysOpenProperty, value);
        }

        public static DependencyProperty StaysOpenOnSelectionProperty = DependencyProperty.Register(nameof(StaysOpenOnSelection), typeof(bool), typeof(TimeUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool StaysOpenOnSelection
        {
            get => (bool)GetValue(StaysOpenOnSelectionProperty);
            set => SetValue(StaysOpenOnSelectionProperty, value);
        }

        public TimeUpDown() : base() => DefaultStyleKey = typeof(TimeUpDown);

        void OnOptionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!handleOption)
            {
                var option = e.AddedItems.First()?.As<DateTime>();

                if (option != null)
                {
                    var result = option.Value.TimeOfDay;
                    SetCurrentValue(ValueProperty.Property, result);

                    handleOption = true;
                    PART_Options.SelectedIndex = -1;
                    handleOption = false;

                    OptionSelected?.Invoke(this, new EventArgs<TimeSpan>(result));
                }

                if (!StaysOpenOnSelection)
                    SetCurrentValue(IsDropDownOpenProperty, false);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Options = Template.FindName(nameof(PART_Options), this) as ListBox;
            if (PART_Options != null)
            {
                var j = DateTime.MinValue.Date;
                for (var i = 0; i < 24; i++)
                {
                    PART_Options.Items.Add(j);
                    j = j.AddHours(1);
                }
                PART_Options.SelectionChanged += OnOptionSelected;
            }

            PART_DropDown = Template.FindName(nameof(PART_DropDown), this) as Popup;
            if (PART_DropDown != null)
                PART_DropDown.Closed += OnDropDownClosed;
        }

        protected virtual void OnDropDownClosed(object sender, EventArgs e)
        {
            if (IsDropDownOpen)
                IsDropDownOpen = false;
        }

        protected virtual void OnIsDropDownOpenChanged(OldNew<bool> input)
        {
            PART_DropDown.If(i => i != null, i => i.IsOpen = input.New);
        }
    }
}