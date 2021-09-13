using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// A <see cref="ContentControl"/> that applies a "ripple effect" to content. If <see cref="RippleMouseEvent"/> = <see cref="MouseEvent.Default"/>, the animation begins automatically with <see cref="RepeatBehavior.Forever"/>.
    /// </summary>
    [TemplatePart(Name = nameof(PART_Canvas), Type = typeof(Canvas))]
    [TemplatePart(Name = nameof(PART_Ellipse), Type = typeof(Ellipse))]
    public class RippleDecorator : ContentControl
    {
        #region Properties

        Canvas PART_Canvas;

        Ellipse PART_Ellipse;

        Handle handle = false;

        public static DependencyProperty FromStrokeThicknessProperty = DependencyProperty.Register(nameof(FromStrokeThickness), typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(15.0, FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public double FromStrokeThickness
        {
            get => (double)GetValue(FromStrokeThicknessProperty);
            set => SetValue(FromStrokeThicknessProperty, value);
        }

        public static DependencyProperty IsRippleEnabledProperty = DependencyProperty.Register(nameof(IsRippleEnabled), typeof(bool), typeof(RippleDecorator), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool IsRippleEnabled
        {
            get => (bool)GetValue(IsRippleEnabledProperty);
            set => SetValue(IsRippleEnabledProperty, value);
        }

        public static DependencyProperty MaximumOpacityProperty = DependencyProperty.Register(nameof(MaximumOpacity), typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(0.8, FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public double MaximumOpacity
        {
            get => (double)GetValue(MaximumOpacityProperty);
            set => SetValue(MaximumOpacityProperty, value);
        }

        public static DependencyProperty MaximumRadiusProperty = DependencyProperty.Register(nameof(MaximumRadius), typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public double MaximumRadius
        {
            get => (double)GetValue(MaximumRadiusProperty);
            set => SetValue(MaximumRadiusProperty, value);
        }

        public static DependencyProperty RippleMouseEventProperty = DependencyProperty.Register(nameof(RippleMouseEvent), typeof(MouseEvent), typeof(RippleDecorator), new FrameworkPropertyMetadata(MouseEvent.MouseDown, FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        /// <summary>
        /// Only <see cref="MouseEvent.MouseDown"/> is currently supported!
        /// </summary>
        public MouseEvent RippleMouseEvent
        {
            get => (MouseEvent)GetValue(RippleMouseEventProperty);
            set => SetValue(RippleMouseEventProperty, value);
        }

        public static DependencyProperty RippleAnimationProperty = DependencyProperty.Register(nameof(RippleAnimation), typeof(Storyboard), typeof(RippleDecorator), new FrameworkPropertyMetadata(default(Storyboard), FrameworkPropertyMetadataOptions.None));
        public Storyboard RippleAnimation
        {
            get => (Storyboard)GetValue(RippleAnimationProperty);
            private set => SetValue(RippleAnimationProperty, value);
        }

        public static DependencyProperty RippleAccelerationProperty = DependencyProperty.Register(nameof(RippleAcceleration), typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(0.6, FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public double RippleAcceleration
        {
            get => (double)GetValue(RippleAccelerationProperty);
            set => SetValue(RippleAccelerationProperty, value);
        }

        public static DependencyProperty RippleDecelerationProperty = DependencyProperty.Register(nameof(RippleDeceleration), typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(0.4, FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public double RippleDeceleration
        {
            get => (double)GetValue(RippleDecelerationProperty);
            set => SetValue(RippleDecelerationProperty, value);
        }

        public static DependencyProperty RippleDelayProperty = DependencyProperty.Register(nameof(RippleDelay), typeof(Duration), typeof(RippleDecorator), new FrameworkPropertyMetadata(default(Duration), FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public Duration RippleDelay
        {
            get => (Duration)GetValue(RippleDelayProperty);
            set => SetValue(RippleDelayProperty, value);
        }

        public static DependencyProperty RippleDurationProperty = DependencyProperty.Register(nameof(RippleDuration), typeof(Duration), typeof(RippleDecorator), new FrameworkPropertyMetadata(default(Duration), FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public Duration RippleDuration
        {
            get => (Duration)GetValue(RippleDurationProperty);
            set => SetValue(RippleDurationProperty, value);
        }

        public static DependencyProperty ToStrokeThicknessProperty = DependencyProperty.Register(nameof(ToStrokeThickness), typeof(double), typeof(RippleDecorator), new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.None, OnRippleChanged));
        public double ToStrokeThickness
        {
            get => (double)GetValue(ToStrokeThicknessProperty);
            set => SetValue(ToStrokeThicknessProperty, value);
        }

        static void OnRippleChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<RippleDecorator>().OnRippleChanged();

        #endregion

        #region RippleDecorator

        public RippleDecorator()
        {
            DefaultStyleKey = typeof(RippleDecorator);
            handle.Invoke(() =>
            {
                SetCurrentValue(RippleDelayProperty, new Duration(TimeSpan.FromSeconds(0.0)));
                SetCurrentValue(RippleDurationProperty, new Duration(TimeSpan.FromSeconds(1.0)));
            });
            SetCurrentValue(RippleAnimationProperty, GetAnimation());
        }

        #endregion

        #region Methods

        void Animate() => PART_Ellipse.BeginStoryboard(RippleAnimation);

        Storyboard GetAnimation()
        {
            var result = new Storyboard();
            result.Duration = RippleDuration;

            var WidthAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = RippleDuration,
                AccelerationRatio = RippleAcceleration,
                DecelerationRatio = RippleDeceleration
            };
            var HeightAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = RippleDuration,
                AccelerationRatio = RippleAcceleration,
                DecelerationRatio = RippleDeceleration
            };
            var StrokeThicknessAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = RippleDuration,
                AccelerationRatio = RippleAcceleration,
                DecelerationRatio = RippleDeceleration
            };
            var OpacityAnimation = new DoubleAnimationUsingKeyFrames()
            {
                Duration = RippleDuration,
                AccelerationRatio = RippleAcceleration,
                DecelerationRatio = RippleDeceleration
            };

            Storyboard.SetTargetName(HeightAnimation, nameof(PART_Ellipse));
            Storyboard.SetTargetName(WidthAnimation, nameof(PART_Ellipse));
            Storyboard.SetTargetName(OpacityAnimation, nameof(PART_Ellipse));
            Storyboard.SetTargetName(StrokeThicknessAnimation, nameof(PART_Ellipse));

            Storyboard.SetTargetProperty(HeightAnimation, new PropertyPath(nameof(Height)));
            Storyboard.SetTargetProperty(WidthAnimation, new PropertyPath(nameof(Width)));
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(nameof(Opacity)));
            Storyboard.SetTargetProperty(StrokeThicknessAnimation, new PropertyPath(nameof(Ellipse.StrokeThickness)));

            HeightAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(RippleDelay.TimeSpan)));
            HeightAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(MaximumRadius, KeyTime.FromTimeSpan(RippleDuration.TimeSpan)));
            HeightAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(RippleDuration.TimeSpan)));

            WidthAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(RippleDelay.TimeSpan)));
            WidthAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(MaximumRadius, KeyTime.FromTimeSpan(RippleDuration.TimeSpan)));
            WidthAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(0, KeyTime.FromTimeSpan(RippleDuration.TimeSpan)));

            var offset = (RippleDuration.TimeSpan.Ticks / 4) * 3;

            StrokeThicknessAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(FromStrokeThickness, KeyTime.FromTimeSpan(RippleDelay.TimeSpan)));
            StrokeThicknessAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(ToStrokeThickness, KeyTime.FromTimeSpan(RippleDuration.TimeSpan)));

            OpacityAnimation.KeyFrames.Add(new DiscreteDoubleKeyFrame(MaximumOpacity, KeyTime.FromTimeSpan(RippleDelay.TimeSpan)));
            OpacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(MaximumOpacity / 2.0, KeyTime.FromTimeSpan(new TimeSpan(offset))));
            OpacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(RippleDuration.TimeSpan)));

            result.Children.Add(HeightAnimation);
            result.Children.Add(WidthAnimation);
            result.Children.Add(OpacityAnimation);
            result.Children.Add(StrokeThicknessAnimation);

            if (RippleMouseEvent == MouseEvent.None)
            {
                result.RepeatBehavior = RepeatBehavior.Forever;
                Animate();
            }
            return result;
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetTop(PART_Ellipse, (PART_Canvas.ActualHeight / 2.0) - (e.NewSize.Height / 2.0));
            Canvas.SetLeft(PART_Ellipse, (PART_Canvas.ActualWidth / 2.0) - (e.NewSize.Width / 2.0));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (RippleAnimation != null && IsRippleEnabled && RippleMouseEvent == MouseEvent.MouseDown)
                Animate();
        }

        public override void OnApplyTemplate()
        {
            PART_Canvas = Template.FindName(nameof(PART_Canvas), this).As<Canvas>();
            PART_Ellipse = Template.FindName(nameof(PART_Ellipse), this).As<Ellipse>();
            PART_Ellipse.SizeChanged += OnSizeChanged;
        }

        protected virtual void OnRippleChanged() => handle.If(i => !i, i => SetCurrentValue(RippleAnimationProperty, GetAnimation()));

        #endregion
    }
}