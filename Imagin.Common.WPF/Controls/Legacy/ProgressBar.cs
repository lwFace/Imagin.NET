using Imagin.Common.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Imagin.Common.Controls
{
    public class ProgressBar : System.Windows.Controls.ProgressBar
    {
        public enum Shapes
        {
            Circle,
            Line
        }

        #region Properties

        readonly object lockme = new object();

        Storyboard indeterminateStoryboard;

        Grid PART_Indicator;

        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(ProgressBar), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets/sets the diameter of the ellipses used in the indeterminate animation.
        /// </summary>
        public double EllipseDiameter
        {
            get
            {
                return (double)GetValue(EllipseDiameterProperty);
            }
            set
            {
                SetValue(EllipseDiameterProperty, value);
            }
        }

        public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(double), typeof(ProgressBar), new PropertyMetadata(default(double)));
        /// <summary>
        /// Gets/sets the offset of the ellipses used in the indeterminate animation.
        /// </summary>
        public double EllipseOffset
        {
            get
            {
                return (double)GetValue(EllipseOffsetProperty);
            }
            set
            {
                SetValue(EllipseOffsetProperty, value);
            }
        }

        public static readonly DependencyProperty IndeterminateShapeProperty = DependencyProperty.Register(nameof(IndeterminateShape), typeof(Shapes), typeof(ProgressBar), new PropertyMetadata(Shapes.Line));
        public Shapes IndeterminateShape
        {
            get => (Shapes)GetValue(IndeterminateShapeProperty);
            set => SetValue(IndeterminateShapeProperty, value);
        }

        public new Brush Foreground
        {
            get => base.Foreground;
            set
            {
                base.Foreground = value;
                OnForegroundChanged(value);
            }
        }

        #endregion

        #region ProgressBar

        static ProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
            IsIndeterminateProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(OnIsIndeterminateChanged));
        }

        public ProgressBar()
        {
            IsVisibleChanged += VisibleChangedHandler;
        }

        #endregion

        #region Methods

        #region Events

        void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= LoadedHandler;
            SizeChangedHandler(null, null);
            SizeChanged += SizeChangedHandler;
        }

        void VisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            // reset Storyboard if Visibility is set to Visible #1300
            if (this.IsIndeterminate)
            {
                ToggleIndeterminate(this, (bool)e.OldValue, (bool)e.NewValue);
            }
        }

        #endregion

        #region Overrides

        LinearGradientBrush GetForeground()
        {
            var result = new LinearGradientBrush();

            double offset = 0;
            for (int i = 0; i < 5; i++, offset += 0.25)
            {
                var color = ((SolidColorBrush)Foreground).Color;

                byte alpha = 0;
                switch (i)
                {
                    case 1:
                    case 3:
                        alpha = ((255.0 / 5.0) * 2.0).Byte();
                        break;
                    case 2:
                        alpha = ((255.0 / 5.0) * 3.0).Byte();
                        break;
                }

                color = Colors.White.A(alpha);
                result.GradientStops.Add(new GradientStop(color, offset));
            }

            return result;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            lock (lockme)
            {
                indeterminateStoryboard = TryFindResource("IndeterminateStoryboard") as Storyboard;
            }

            Loaded -= LoadedHandler;
            Loaded += LoadedHandler;

            PART_Indicator = Template.FindName(nameof(PART_Indicator), this) as Grid;
            PART_Indicator.Background = GetForeground();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Update the Ellipse properties to their default values
            // only if they haven't been user-set.
            if (EllipseDiameter.Equals(0))
            {
                SetEllipseDiameter(this.ActualSize());
            }
            if (EllipseOffset.Equals(0))
            {
                SetEllipseOffset(this.ActualSize());
            }
        }

        protected virtual void OnForegroundChanged(Brush foreground)
        {
            if (PART_Indicator != null)
                PART_Indicator.Background = GetForeground();
        }

        #endregion

        #region Private

        void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            var size = this.ActualSize();
            var bar = this;
            if (this.Visibility == Visibility.Visible && this.IsIndeterminate)
            {
                bar.ResetStoryboard(size, true);
            }
        }

        double ActualSize()
        {
            return this.Orientation == Orientation.Horizontal ? this.ActualWidth : this.ActualHeight;
        }

        void ResetStoryboard(double width, bool removeOldStoryboard)
        {
            if (!this.IsIndeterminate)
            {
                return;
            }
            lock (this.lockme)
            {
                //perform calculations
                var containerAnimStart = CalcContainerAnimStart(width);
                var containerAnimEnd = CalcContainerAnimEnd(width);
                var ellipseAnimWell = CalcEllipseAnimWell(width);
                var ellipseAnimEnd = CalcEllipseAnimEnd(width);
                //reset the main double animation
                try
                {
                    var indeterminate = GetIndeterminate();

                    if (indeterminate != null && this.indeterminateStoryboard != null)
                    {
                        var newStoryboard = this.indeterminateStoryboard.Clone();
                        var doubleAnim = newStoryboard.Children.First(t => t.Name == "MainDoubleAnim");
                        doubleAnim.SetValue(DoubleAnimation.FromProperty, containerAnimStart);
                        doubleAnim.SetValue(DoubleAnimation.ToProperty, containerAnimEnd);

                        var namesOfElements = new[] { "E1", "E2", "E3", "E4", "E5" };
                        foreach (var elemName in namesOfElements)
                        {
                            var doubleAnimParent = (DoubleAnimationUsingKeyFrames)newStoryboard.Children.First(t => t.Name == elemName + "Anim");
                            DoubleKeyFrame first, second, third;
                            if (elemName == "E1")
                            {
                                first = doubleAnimParent.KeyFrames[1];
                                second = doubleAnimParent.KeyFrames[2];
                                third = doubleAnimParent.KeyFrames[3];
                            }
                            else
                            {
                                first = doubleAnimParent.KeyFrames[2];
                                second = doubleAnimParent.KeyFrames[3];
                                third = doubleAnimParent.KeyFrames[4];
                            }

                            first.Value = ellipseAnimWell;
                            second.Value = ellipseAnimWell;
                            third.Value = ellipseAnimEnd;
                            first.InvalidateProperty(DoubleKeyFrame.ValueProperty);
                            second.InvalidateProperty(DoubleKeyFrame.ValueProperty);
                            third.InvalidateProperty(DoubleKeyFrame.ValueProperty);

                            doubleAnimParent.InvalidateProperty(Storyboard.TargetPropertyProperty);
                            doubleAnimParent.InvalidateProperty(Storyboard.TargetNameProperty);
                        }

                        var containingGrid = (FrameworkElement)GetTemplateChild("ContainingGrid");

                        if (removeOldStoryboard && indeterminate.Storyboard != null)
                        {
                            // remove the previous storyboard from the Grid #1855
                            indeterminate.Storyboard.Stop(containingGrid);
                            indeterminate.Storyboard.Remove(containingGrid);
                        }

                        indeterminate.Storyboard = newStoryboard;

                        if (indeterminate.Storyboard != null)
                        {
                            indeterminate.Storyboard.Begin(containingGrid, true);
                        }
                    }
                }
                catch (Exception)
                {
                    //we just ignore 
                }
            }
        }

        VisualState GetIndeterminate()
        {
            var templateGrid = GetTemplateChild("ContainingGrid") as FrameworkElement;
            if (templateGrid == null)
            {
                this.ApplyTemplate();
                templateGrid = GetTemplateChild("ContainingGrid") as FrameworkElement;
                if (templateGrid == null) return null;
            }
            var groups = VisualStateManager.GetVisualStateGroups(templateGrid);
            return groups != null
                ? groups.Cast<VisualStateGroup>()
                        .SelectMany(@group => @group.States.Cast<VisualState>())
                        .FirstOrDefault(state => state.Name == "Indeterminate")
                : null;
        }

        void SetEllipseDiameter(double width)
        {
            if (width <= 180)
            {
                EllipseDiameter = 4;
                return;
            }
            if (width <= 280)
            {
                EllipseDiameter = 5;
                return;
            }

            EllipseDiameter = 6;
        }

        void SetEllipseOffset(double width)
        {
            if (width <= 180)
            {
                EllipseOffset = 4;
                return;
            }
            if (width <= 280)
            {
                EllipseOffset = 7;
                return;
            }

            EllipseOffset = 9;
        }

        double CalcContainerAnimStart(double width)
        {
            if (width <= 180)
            {
                return -34;
            }
            if (width <= 280)
            {
                return -50.5;
            }

            return -63;
        }

        double CalcContainerAnimEnd(double width)
        {
            var firstPart = 0.4352 * width;
            if (width <= 180)
            {
                return firstPart - 25.731;
            }
            if (width <= 280)
            {
                return firstPart + 27.84;
            }

            return firstPart + 58.862;
        }

        double CalcEllipseAnimWell(double width)
        {
            return width * 1.0 / 3.0;
        }

        double CalcEllipseAnimEnd(double width)
        {
            return width * 2.0 / 3.0;
        }

        #endregion

        #region Static

        static void OnIsIndeterminateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var bar = (ProgressBar)dependencyObject;
            if (!bar.IsLoaded || !bar.IsVisible)
            {
                return;
            }
            ToggleIndeterminate(bar, (bool)e.OldValue, (bool)e.NewValue);
        }

        static void ToggleIndeterminate(ProgressBar bar, bool oldValue, bool newValue)
        {
            if (newValue == oldValue)
            {
                return;
            }
            var indeterminateState = bar.GetIndeterminate();
            var containingObject = bar.GetTemplateChild("ContainingGrid") as FrameworkElement;
            if (indeterminateState != null && containingObject != null)
            {
                var resetAction = new Action(() =>
                {
                    if (oldValue && indeterminateState.Storyboard != null)
                    {
                        // remove the previous storyboard from the Grid #1855
                        indeterminateState.Storyboard.Stop(containingObject);
                        indeterminateState.Storyboard.Remove(containingObject);
                    }
                    if (newValue)
                    {
                        bar.InvalidateMeasure();
                        bar.InvalidateArrange();
                        bar.ResetStoryboard(bar.ActualSize(), false);
                    }
                });
                resetAction.Invoke();
            }
        }

        #endregion

        #endregion
    }
}