using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Down), Type = typeof(Button))]
    [TemplatePart(Name = nameof(PART_Up), Type = typeof(Button))]
    public abstract class UpDown : TextBox, IPropertyChanged
    {
        #region Classes

        protected class UpDownTimer : DispatcherTimer
        {
            public double Milliseconds { get; set; } = 0;

            public UpDownDirection Direction { get; set; } = default(UpDownDirection);

            internal UpDownTimer() : base() { }
        }

        #endregion

        #region Enums

        protected enum UpDownDirection
        {
            None,
            Up,
            Down
        }

        #endregion

        #region Properties

        protected ContentControl PART_Down { get; private set; } = null;

        protected ContentControl PART_Up { get; private set; } = null;

        protected UpDownTimer Timer { get; private set; } = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public static DependencyProperty CanUpDownProperty = DependencyProperty.Register(nameof(CanUpDown), typeof(bool), typeof(UpDown), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanUpDown
        {
            get => (bool)GetValue(CanUpDownProperty);
            set => SetValue(CanUpDownProperty, value);
        }
        
        public static DependencyProperty DirectionalChangeProperty = DependencyProperty.Register(nameof(DirectionalChange), typeof(DirectionalNavigation), typeof(UpDown), new FrameworkPropertyMetadata(DirectionalNavigation.Circular, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DirectionalNavigation DirectionalChange
        {
            get => (DirectionalNavigation)GetValue(DirectionalChangeProperty);
            set => SetValue(DirectionalChangeProperty, value);
        }

        public static DependencyProperty DownButtonTemplateProperty = DependencyProperty.Register(nameof(DownButtonTemplate), typeof(DataTemplate), typeof(UpDown), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate DownButtonTemplate
        {
            get => (DataTemplate)GetValue(DownButtonTemplateProperty);
            set => SetValue(DownButtonTemplateProperty, value);
        }

        public static DependencyProperty MajorChangeProperty = DependencyProperty.Register(nameof(MajorChange), typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMajorChangeChanged));
        public double MajorChange
        {
            get => (double)GetValue(MajorChangeProperty);
            set => SetValue(MajorChangeProperty, value);
        }
        static void OnMajorChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.As<UpDown>().OnMajorChangeChanged(new OldNew<double>(e));

        public static DependencyProperty MajorChangeDelayProperty = DependencyProperty.Register(nameof(MajorChangeDelay), typeof(double), typeof(UpDown), new FrameworkPropertyMetadata(500.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double MajorChangeDelay
        {
            get => (double)GetValue(MajorChangeDelayProperty);
            set => SetValue(MajorChangeDelayProperty, value);
        }

        public static DependencyProperty UpButtonTemplateProperty = DependencyProperty.Register(nameof(UpButtonTemplate), typeof(DataTemplate), typeof(UpDown), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate UpButtonTemplate
        {
            get => (DataTemplate)GetValue(UpButtonTemplateProperty);
            set => SetValue(UpButtonTemplateProperty, value);
        }

        #endregion

        #region UpDown

        public UpDown() : base()
        {
            DefaultStyleKey = typeof(UpDown);
            ResetTimer();
        }

        #endregion

        #region Methods

        #region Abstract

        /// <summary>
        /// Gets whether or not decreasing is enabled.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanDecrease();

        /// <summary>
        /// Gets whether or not increasing is enabled.
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanIncrease();

        /// <summary>
        /// Decreases value by some value.
        /// </summary>
        public abstract void Decrease();

        public abstract void ValueToMaximum();

        public abstract void ValueToMinimum();

        /// <summary>
        /// Increases value by some value.
        /// </summary>
        public abstract void Increase();

        #endregion

        #region Commands

        ICommand downCommand;
        public ICommand DownCommand
        {
            get
            {
                downCommand = downCommand ?? new RelayCommand(() => Change(UpDownDirection.Down), () => CanUpDown && (DirectionalChange == DirectionalNavigation.Circular || CanDecrease()));
                return downCommand;
            }
        }

        ICommand upCommand;
        public ICommand UpCommand
        {
            get
            {
                upCommand = upCommand ?? new RelayCommand(() => Change(UpDownDirection.Up), () => CanUpDown && (DirectionalChange == DirectionalNavigation.Circular || CanIncrease()));
                return upCommand;
            }
        }

        #endregion

        #region Private

        void Handle(object sender, MouseButtonEventArgs e) => e.Handled = true;

        #endregion

        #region Protected

        /// <summary>
        /// Increase or decrease based on given direction.
        /// </summary>
        /// <param name="Direction"></param>
        protected void Change(UpDownDirection Direction)
        {
            if (Direction == UpDownDirection.Up)
            {
                if (CanIncrease())
                {
                    Increase();
                }
                else if (DirectionalChange == DirectionalNavigation.Circular)
                    ValueToMinimum(); //Value = Minimum;
            }
            else if (Direction == UpDownDirection.Down)
            {
                if (CanDecrease())
                {
                    Decrease();
                }
                else if (DirectionalChange == DirectionalNavigation.Circular)
                    ValueToMaximum(); //Value = Maximum;
            }
        }

        /// <summary>
        /// Reset the timer used for making major changes.
        /// </summary>
        protected void ResetTimer()
        {
            Timer = Timer ?? new UpDownTimer();
            Timer.Stop();
            Timer.Interval = TimeSpan.FromMilliseconds(MajorChange);
            Timer.Tick -= OnMajorChange;
            Timer.Tick += OnMajorChange;
        }

        /// <summary>
        /// Set text; string format should be applied prior to calling.
        /// </summary>
        protected void SetText(string text)
        {
            var i = CaretIndex;
            i = i <= 0 ? text.Length - 1 : i;
            SetCurrentValue(TextProperty, text);
            CaretIndex = i;
        }

        #endregion

        #region Overrides

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if (ModifierKeys.Control.Pressed())
            {
                if (e.Delta > 0)
                {
                    Change(UpDownDirection.Up);
                }
                else Change(UpDownDirection.Down);
                e.Handled = true;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Down = Template.FindName(nameof(PART_Down), this) as ContentControl;

            PART_Down.PreviewMouseDown += OnButtonMouseDown;
            PART_Down.PreviewMouseUp += OnButtonMouseUp;

            PART_Down.MouseDoubleClick += Handle;
            PART_Down.MouseDown += Handle;
            PART_Down.MouseUp += Handle;

            PART_Up = Template.FindName(nameof(PART_Up), this) as ContentControl;

            PART_Up.PreviewMouseDown += OnButtonMouseDown;
            PART_Up.PreviewMouseUp += OnButtonMouseUp;

            PART_Up.MouseDoubleClick += Handle;
            PART_Up.MouseDown += Handle;
            PART_Up.MouseUp += Handle;
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Occurs when the mouse presses the increase or decrease button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            var name = sender.As<ContentControl>().Name;
            Timer.Direction = name == nameof(PART_Up) ? UpDownDirection.Up : name == nameof(PART_Down) ? UpDownDirection.Down : UpDownDirection.None;
            Timer.Start();
        }

        /// <summary>
        /// Occurs when the mouse releases the increase or decrease button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnButtonMouseUp(object sender, MouseButtonEventArgs e)
        {
            Timer.Stop();
            Timer.Direction = UpDownDirection.None;
            Timer.Milliseconds = 0d;
        }

        /// <summary>
        /// Occurs during a major change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMajorChange(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                OnButtonMouseUp(this, null);
                return;
            }

            Timer.Milliseconds += Timer.Interval.TotalMilliseconds;

            if (Timer.Milliseconds < MajorChangeDelay)
                return;

            Change(Timer.Direction);
        }

        /// <summary>
        /// Occurs when <see cref="MajorChange"/> changes.
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnMajorChangeChanged(OldNew<double> input)
        {
            ResetTimer();
        }

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        /// <param name="Name"></param>
        public virtual void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        #endregion

        #endregion
    }
}