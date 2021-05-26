using Imagin.Common.Input;
using Imagin.Common.Math;
using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class SelectionCanvas : System.Windows.Controls.UserControl
    {
        #region Properties

        public event EventHandler<EventArgs<DoubleRegion>> Selected;

        bool isDragging;

        Point startPosition;
        
        public static DependencyProperty MouseButtonProperty = DependencyProperty.Register(nameof(MouseButton), typeof(MouseButton), typeof(SelectionCanvas), new FrameworkPropertyMetadata(MouseButton.Left, FrameworkPropertyMetadataOptions.None));
        public MouseButton MouseButton
        {
            get => (MouseButton)GetValue(MouseButtonProperty);
            set => SetValue(MouseButtonProperty, value);
        }

        public static DependencyProperty ResetOnDrawnProperty = DependencyProperty.Register(nameof(ResetOnDrawn), typeof(bool), typeof(SelectionCanvas), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ResetOnDrawn
        {
            get => (bool)GetValue(ResetOnDrawnProperty);
            set => SetValue(ResetOnDrawnProperty, value);
        }

        public static DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(DoubleRegion), typeof(SelectionCanvas), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DoubleRegion Selection
        {
            get => (DoubleRegion)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        #endregion

        #region SelectionCanvas

        public SelectionCanvas()
        {
            InitializeComponent();
            Selection = new DoubleRegion();
        }

        #endregion

        #region Methods

        Rect GetRect(Point CurrentPosition)
        {
            var result = new Rect();

            double
                x = (startPosition.X < CurrentPosition.X ? startPosition.X : CurrentPosition.X),
                y = (startPosition.Y < CurrentPosition.Y ? startPosition.Y : CurrentPosition.Y);

            result.Size = new Size(System.Math.Abs(CurrentPosition.X - startPosition.X), System.Math.Abs(CurrentPosition.Y - startPosition.Y));
            result.X = x;
            result.Y = y;

            return result;
        }

        void OnDrawDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton)
            {
                PART_Grid.CaptureMouse();
                isDragging = true;
                startPosition = e.GetPosition(PART_Grid);

                Selection.X = startPosition.X;
                Selection.Y = startPosition.Y;
                Selection.Height = 0;
                Selection.Width = 0;
            }
        }

        void OnDrawMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var result = GetRect(e.GetPosition(PART_Grid));
                Selection.X = result.X;
                Selection.Y = result.Y;
                Selection.Height = result.Height;
                Selection.Width = result.Width;
            }
        }

        void OnDrawUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;

            if (PART_Grid.IsMouseCaptured)
                PART_Grid.ReleaseMouseCapture();

            startPosition = default;

            OnSelected(Selection);
            if (ResetOnDrawn)
            {
                Selection.X = 0;
                Selection.Y = 0;
                Selection.Height = 0;
                Selection.Width = 0;
            }
        }

        protected virtual void OnSelected(DoubleRegion Selection)
        {
            Selected?.Invoke(this, new EventArgs<DoubleRegion>(Selection));
        }

        #endregion
    }
}