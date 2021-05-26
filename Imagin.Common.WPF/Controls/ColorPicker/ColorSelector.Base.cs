using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media;
using Imagin.Common.Media.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    [TemplatePart(
      Name = nameof(PART_Canvas),
      Type = typeof(Canvas))]
    [TemplatePart(
      Name = nameof(PART_CheckerBoard),
      Type = typeof(CheckerBoard))]
    public class BaseColorSelector : ContentControl
    {
        Canvas PART_Canvas;

        CheckerBoard PART_CheckerBoard;

        /// .........................................................................................

        public static DependencyProperty AlphaProperty = DependencyProperty.Register(nameof(Alpha), typeof(Alpha), typeof(BaseColorSelector), new FrameworkPropertyMetadata(Alpha.With));
        public Alpha Alpha
        {
            get => (Alpha)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        public static DependencyProperty AlphaValueProperty = DependencyProperty.Register(nameof(AlphaValue), typeof(byte), typeof(BaseColorSelector), new FrameworkPropertyMetadata(byte.MaxValue));
        public byte AlphaValue
        {
            get => (byte)GetValue(AlphaValueProperty);
            set => SetValue(AlphaValueProperty, value);
        }

        public static DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(VisualModel), typeof(BaseColorSelector), new FrameworkPropertyMetadata(null));
        public VisualModel Model
        {
            get => (VisualModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(BaseColorSelector), new FrameworkPropertyMetadata(0.0, OnValueChanged));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        protected static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as BaseColorSelector).OnValueChanged(new OldNew<double>(e));

        /// .........................................................................................

        public BaseColorSelector() : base() { }

        protected virtual void Mark() { }

        /// .........................................................................................

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is IInputElement i)
                {
                    OnMouseChanged(Normalize(e.GetPosition(i)));
                    i.CaptureMouse();
                }
            }
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is IInputElement i)
                    OnMouseChanged(Normalize(e.GetPosition(i)));
            }
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                ((IInputElement)sender).ReleaseMouseCapture();
        }

        /// .........................................................................................

        /// <summary>
        /// Gets a <see cref="Point"/> with range [0, 1].
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected Vector2<One> Normalize(Point input)
        {
            input = input.Coerce(new Point(ActualWidth, ActualHeight), new Point(0, 0));
            input = new Point(input.X, ActualHeight - input.Y);
            input = new Point(input.X / ActualWidth, input.Y / ActualHeight);
            return new Vector2<One>(input.X, input.Y);
        }

        /// .........................................................................................

        protected virtual void OnMouseChanged(Vector2<One> point) { }

        protected virtual void OnValueChanged(OldNew<double> input) { }

        /// .........................................................................................

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Mark();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Canvas = Template.FindName(nameof(PART_Canvas), this) as Canvas;

            PART_CheckerBoard = Template.FindName(nameof(PART_CheckerBoard), this) as CheckerBoard;
            PART_CheckerBoard.MouseDown += OnMouseDown;
            PART_CheckerBoard.MouseMove += OnMouseMove;
            PART_CheckerBoard.MouseUp += OnMouseUp;
        }
    }
}