using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media.Animation;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(Column1), Type = typeof(ColumnDefinition))]
    [TemplatePart(Name = nameof(Column3), Type = typeof(ColumnDefinition))]
    [TemplatePart(Name = nameof(PART_Grid), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(PART_Panel1), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(PART_Panel2), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(Row1), Type = typeof(RowDefinition))]
    [TemplatePart(Name = nameof(Row3), Type = typeof(RowDefinition))]
    public class SplitView : UserControl
    {
        #region Properties

        ColumnDefinition Column1
        {
            get; set;
        }

        ColumnDefinition Column3
        {
            get; set;
        }

        Grid PART_Grid
        {
            get; set;
        }

        Grid PART_Panel1
        {
            get; set;
        }

        Grid PART_Panel2
        {
            get; set;
        }

        RowDefinition Row1
        {
            get; set;
        }

        RowDefinition Row3
        {
            get; set;
        }

        public event EventHandler<EventArgs> Aligned;

        public event EventHandler<EventArgs> Collapsed;

        public static readonly DependencyProperty ButtonSpacingProperty = DependencyProperty.Register(nameof(ButtonSpacing), typeof(Thickness), typeof(SplitView), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ButtonSpacing
        {
            get => (Thickness)GetValue(ButtonSpacingProperty);
            set => SetValue(ButtonSpacingProperty, value);
        }

        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(nameof(Buttons), typeof(FrameworkElementCollection), typeof(SplitView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public FrameworkElementCollection Buttons
        {
            get => (FrameworkElementCollection)GetValue(ButtonsProperty);
            set => SetValue(ButtonsProperty, value);
        }
        
        public static readonly DependencyProperty CanAlignProperty = DependencyProperty.Register(nameof(CanAlign), typeof(bool), typeof(SplitView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanAlign
        {
            get => (bool)GetValue(CanAlignProperty);
            set => SetValue(CanAlignProperty, value);
        }

        public static readonly DependencyProperty CanCollapseProperty = DependencyProperty.Register(nameof(CanCollapse), typeof(bool), typeof(SplitView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanCollapse
        {
            get => (bool)GetValue(CanCollapseProperty);
            set => SetValue(CanCollapseProperty, value);
        }

        public static readonly DependencyProperty CanSetOrientationProperty = DependencyProperty.Register(nameof(CanSetOrientation), typeof(bool), typeof(SplitView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanSetOrientation
        {
            get => (bool)GetValue(CanSetOrientationProperty);
            set => SetValue(CanSetOrientationProperty, value);
        }

        public static readonly DependencyProperty CanSwapProperty = DependencyProperty.Register(nameof(CanSwap), typeof(bool), typeof(SplitView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanSwap
        {
            get => (bool)GetValue(CanSwapProperty);
            set => SetValue(CanSwapProperty, value);
        }

        public static readonly DependencyProperty Panel1Property = DependencyProperty.Register(nameof(Panel1), typeof(object), typeof(SplitView), null);
        public object Panel1
        {
            get => GetValue(Panel1Property);
            set => SetValue(Panel1Property, value);
        }

        public static readonly DependencyProperty Panel2Property = DependencyProperty.Register(nameof(Panel2), typeof(object), typeof(SplitView), null);
        public object Panel2
        {
            get => GetValue(Panel2Property);
            set => SetValue(Panel2Property, value);
        }

        public static DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(SplitView), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.None, OnOrientationChanged));
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        static void OnOrientationChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<SplitView>().OnOrientationChanged(new OldNew<Orientation>(e).New);

        public static DependencyProperty IsOrientationCanonicalProperty = DependencyProperty.Register(nameof(IsOrientationCanonical), typeof(bool), typeof(SplitView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsOrientationCanonical
        {
            get => (bool)GetValue(IsOrientationCanonicalProperty);
            set => SetValue(IsOrientationCanonicalProperty, value);
        }

        public static DependencyProperty ShowPanel1Property = DependencyProperty.Register(nameof(ShowPanel1), typeof(bool), typeof(SplitView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowPanel1
        {
            get => (bool)GetValue(ShowPanel1Property);
            set => SetValue(ShowPanel1Property, value);
        }

        public static DependencyProperty ShowPanel2Property = DependencyProperty.Register(nameof(ShowPanel2), typeof(bool), typeof(SplitView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowPanel2
        {
            get => (bool)GetValue(ShowPanel2Property);
            set => SetValue(ShowPanel2Property, value);
        }

        #endregion

        #region SplitView

        public SplitView()
        {
            DefaultStyleKey = typeof(SplitView);
            Buttons = new FrameworkElementCollection();
        }

        #endregion

        #region Methods

        protected virtual void OnAligned()
        {
            var a = new GridLength(1d, GridUnitType.Star);
            var b = new GridLength(1d, GridUnitType.Star);

            if (Orientation == Orientation.Horizontal)
            {
                Row1.Height = a;
                Row3.Height = b;
            }
            else
            {
                Column1.Width = a;
                Column3.Width = b;
            }

            Aligned?.Invoke(this, new EventArgs());
        }

        protected virtual void OnCollapsed(bool LeftOrTop)
        {
            var Animation = new GridLengthAnimation()
            {
                From = LeftOrTop ? Column1.Width : Row1.Height,
                To = new GridLength(0.0),
                Duration = TimeSpan.FromSeconds(0.5),
                AccelerationRatio = 0.4,
                DecelerationRatio = 0.4
            };

            if (LeftOrTop)
            {
                Column1.BeginAnimation(ColumnDefinition.WidthProperty, Animation);
            }
            else Row1.BeginAnimation(RowDefinition.HeightProperty, Animation);

            Collapsed?.Invoke(this, new EventArgs());
        }

        protected virtual async void OnOrientationChanged(Orientation orientation)
        {
            PART_Grid.Opacity = 0;
            await PART_Grid.FadeIn();
        }

        Handle swapping = false;

        async void Swap()
        {
            if (!swapping)
            {
                swapping = true;
                PART_Grid.Opacity = 0;
                IsOrientationCanonical = !IsOrientationCanonical;
                await PART_Grid.FadeIn();
                swapping = false;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Grid = Template.FindName(nameof(PART_Grid), this) as Grid;

            PART_Panel1 = Template.FindName(nameof(PART_Panel1), this) as Grid;
            PART_Panel2 = Template.FindName(nameof(PART_Panel2), this) as Grid;

            Row1 = Template.FindName(nameof(Row1), this) as RowDefinition;
            Row3 = Template.FindName(nameof(Row3), this) as RowDefinition;

            Column1 = Template.FindName(nameof(Column1), this) as ColumnDefinition;
            Column3 = Template.FindName(nameof(Column3), this) as ColumnDefinition;
        }

        ICommand alignCommand;
        public ICommand AlignCommand => alignCommand = alignCommand ?? new RelayCommand(() => OnAligned(), () => CanAlign);

        ICommand collapseCommand;
        public ICommand CollapseCommand => collapseCommand = collapseCommand ?? new RelayCommand<object>(p => OnCollapsed(p.ToString().Boolean().Value), p => CanCollapse && p != null);

        ICommand setOrientationCommand;
        public ICommand SetOrientationCommand => setOrientationCommand = setOrientationCommand ?? new RelayCommand<Orientation>(i => OnOrientationChanged(i), i => CanSetOrientation);

        ICommand swapCommand;
        public ICommand SwapCommand => swapCommand = swapCommand ?? new RelayCommand(() => Swap(), () => CanSwap);

        #endregion
    }
}