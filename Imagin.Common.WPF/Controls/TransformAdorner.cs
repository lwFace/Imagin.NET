using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class TransformAdorner : Adorner
    {
        #region Properties

        /// <summary>
        /// Resizing adorner uses Thumbs for visual elements. 
        /// The Thumbs have built-in mouse input handling.
        /// </summary>
        Thumb top, bottom, left, right, topLeft, topRight, bottomLeft, bottomRight;

        public double SnapToGrid { get; set; } = 16d;

        Style thumbStyle = null;
        public Style ThumbStyle
        {
            get => thumbStyle;
            set
            {
                thumbStyle = value;
            }
        }

        /// <summary>
        /// To store and manage the adorner’s visual children.
        /// </summary>
        VisualCollection visualChildren;

        /// <summary>
        /// Override the VisualChildrenCount and GetVisualChild 
        /// properties to interface with the adorner’s visual 
        /// collection.
        /// </summary>
        protected override int VisualChildrenCount => visualChildren.Count;

        #endregion

        #region TransformAdorner

        readonly FrameworkElement Element;

        public TransformAdorner(UIElement element, double snapToGrid, Style resizeThumbStyle) : base(element)
        {
            Element = element as FrameworkElement;
            SnapToGrid = snapToGrid;
            ThumbStyle = resizeThumbStyle;

            if (Element == null)
                throw new NotSupportedException();

            visualChildren = new VisualCollection(this);

            BuildAdornerCorner(ref top, Cursors.SizeNS);
            BuildAdornerCorner(ref left, Cursors.SizeWE);
            BuildAdornerCorner(ref right, Cursors.SizeWE);
            BuildAdornerCorner(ref bottom, Cursors.SizeNS);
            BuildAdornerCorner(ref topLeft, Cursors.SizeNWSE);
            BuildAdornerCorner(ref topRight, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomLeft, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomRight, Cursors.SizeNWSE);

            top.DragDelta 
                += new DragDeltaEventHandler(HandleTop);
            left.DragDelta 
                += new DragDeltaEventHandler(HandleLeft);
            right.DragDelta
                += new DragDeltaEventHandler(HandleRight);
            bottom.DragDelta 
                += new DragDeltaEventHandler(HandleBottom);
            topLeft.DragDelta 
                += new DragDeltaEventHandler(HandleTopLeft);
            topRight.DragDelta 
                += new DragDeltaEventHandler(HandleTopRight);
            bottomLeft.DragDelta 
                += new DragDeltaEventHandler(HandleBottomLeft);
            bottomRight.DragDelta 
                += new DragDeltaEventHandler(HandleBottomRight);

            /*
            rotateLine = new System.Windows.Shapes.Rectangle()
            {
                Fill = Brushes.Black,
                Height = 20,
                Width = 1
            };
            visualChildren.Add(rotateLine);
            rotateSphere = new Thumb()
            {
                Background = Brushes.Black,
                Height = 10,
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                Style = rotateThumbStyle,
                Width = 10
            };
            rotateSphere.DragDelta += new DragDeltaEventHandler(OnRotating);
            rotateSphere.DragStarted += new DragStartedEventHandler(OnRotateStarted);
            visualChildren.Add(rotateSphere);
            */
        }

        //System.Windows.Shapes.Rectangle rotateLine;

        //Thumb rotateSphere;

        #endregion

        #region Methods

        bool CanHandle(Thumb Thumb)
        {
            var result = true;

            if (AdornedElement == null || Thumb == null)
                result = false;

            if (result)
                EnforceSize(AdornedElement as FrameworkElement);

            return result;
        }

        /*
        Canvas canvas;

        Point centerPoint;

        double initialAngle;

        RotateTransform rotateTransform;

        System.Windows.Vector startVector;

        void OnRotating(object sender, DragDeltaEventArgs e)
        {
            if (CanHandle(sender as Thumb))
            {
                if (Element != null && canvas != null)
                {
                    Point currentPoint = Mouse.GetPosition(canvas);
                    System.Windows.Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

                    double angle = System.Windows.Vector.AngleBetween(startVector, deltaVector);

                    RotateTransform rotateTransform = Element.RenderTransform as RotateTransform;
                    rotateTransform.Angle = initialAngle + Math.Round(angle, 0);

                    Element.RenderTransformOrigin = new Point(0.5, 0.5);
                    Element.InvalidateMeasure();

                    ControlExtensions.SetRotation(Element, rotateTransform.Angle);
                }
            }
        }

        void OnRotateStarted(object sender, DragStartedEventArgs e)
        {
            if (CanHandle(sender as Thumb))
            {
                if (Element != null)
                {
                    canvas = VisualTreeHelper.GetParent(Element) as Canvas;
                    if (canvas != null)
                    {
                        centerPoint = Element.TranslatePoint(new Point(Element.Width * Element.RenderTransformOrigin.X, Element.Height * Element.RenderTransformOrigin.Y), canvas);

                        Point startPoint = Mouse.GetPosition(canvas);
                        startVector = Point.Subtract(startPoint, centerPoint);

                        rotateTransform = Element.RenderTransform as RotateTransform;
                        if (rotateTransform == null)
                        {
                            Element.RenderTransform = new RotateTransform(0);
                            initialAngle = 0;
                        }
                        else initialAngle = this.rotateTransform.Angle;
                    }
                }
            }
        }
        */

        /// <summary>
        /// Handler for resizing from the bottom-right.
        /// </summary>
        void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                Adorned.Width = System.Math.Max(Adorned.Width + args.HorizontalChange, Thumb.DesiredSize.Width).NearestFactor(SnapToGrid);
                Adorned.Height = System.Math.Max(args.VerticalChange + Adorned.Height, Thumb.DesiredSize.Height).NearestFactor(SnapToGrid);
            }
        }

        /// <summary>
        /// Handler for resizing from the top-right.
        /// </summary>
        void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                Adorned.Width = System.Math.Max(Adorned.Width + args.HorizontalChange, Thumb.DesiredSize.Width).NearestFactor(SnapToGrid);

                var height_old = Adorned.Height;
                var height_new = System.Math.Max(Adorned.Height - args.VerticalChange, Thumb.DesiredSize.Height).NearestFactor(SnapToGrid);
                var top_old = Canvas.GetTop(Adorned);

                Adorned.Height = height_new;
                Canvas.SetTop(Adorned, top_old - (height_new - height_old));
            }
        }

        /// <summary>
        /// Handler for resizing from the top-left.
        /// </summary>
        void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                var width_old = Adorned.Width;
                var width_new = System.Math.Max(Adorned.Width - args.HorizontalChange, Thumb.DesiredSize.Width).NearestFactor(SnapToGrid);
                var left_old = Canvas.GetLeft(Adorned);

                Adorned.Width = width_new;
                Canvas.SetLeft(Adorned, left_old - (width_new - width_old));

                var height_old = Adorned.Height;
                var height_new = System.Math.Max(Adorned.Height - args.VerticalChange, Thumb.DesiredSize.Height).NearestFactor(SnapToGrid);
                var top_old = Canvas.GetTop(Adorned);

                Adorned.Height = height_new;
                Canvas.SetTop(Adorned, top_old - (height_new - height_old));
            }
        }

        /// <summary>
        /// Handler for resizing from the bottom-left.
        /// </summary>
        void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                Adorned.Height = System.Math.Max(args.VerticalChange + Adorned.Height, Thumb.DesiredSize.Height).NearestFactor(SnapToGrid);

                var width_old = Adorned.Width;
                var width_new = System.Math.Max(Adorned.Width - args.HorizontalChange, Thumb.DesiredSize.Width).NearestFactor(SnapToGrid);
                var left_old = Canvas.GetLeft(Adorned);

                Adorned.Width = width_new;
                Canvas.SetLeft(Adorned, left_old - (width_new - width_old));
            }
        }

        /// <summary>
        /// Handler for resizing from the top.
        /// </summary>
        void HandleTop(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                var height_old = Adorned.Height;
                var height_new = System.Math.Max(Adorned.Height - args.VerticalChange, Thumb.DesiredSize.Height).NearestFactor(SnapToGrid);
                var top_old = Canvas.GetTop(Adorned);
                var top_new = top_old - (height_new - height_old);

                Adorned.Height = height_new;
                Canvas.SetTop(Adorned, top_new);
            }
        }

        /// <summary>
        /// Handler for resizing from the left.
        /// </summary>
        void HandleLeft(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                var width_old = Adorned.Width;
                var width_new = System.Math.Max(Adorned.Width - args.HorizontalChange, Thumb.DesiredSize.Width).NearestFactor(SnapToGrid);
                var left_old = Canvas.GetLeft(Adorned);
                var left_new = left_old - (width_new - width_old);

                Adorned.Width = width_new;
                Canvas.SetLeft(Adorned, left_new);
            }
        }

        /// <summary>
        /// Handler for resizing from the right.
        /// </summary>
        void HandleRight(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                var width = System.Math.Max(Adorned.Width + args.HorizontalChange, Thumb.DesiredSize.Width);
                Adorned.Width = width.NearestFactor(SnapToGrid);
            }
        }

        /// <summary>
        /// Handler for resizing from the bottom.
        /// </summary>
        void HandleBottom(object sender, DragDeltaEventArgs args)
        {
            var Adorned = this.AdornedElement as FrameworkElement;
            var Thumb = sender as Thumb;

            if (CanHandle(Thumb))
            {
                var height = System.Math.Max(args.VerticalChange + Adorned.Height, Thumb.DesiredSize.Height);
                Adorned.Height = height.NearestFactor(SnapToGrid);
            }
        }

        /// <summary>
        /// Helper method to instantiate the corner Thumbs, 
        /// set the Cursor property, set some appearance properties, 
        /// and add the elements to the visual tree.
        /// </summary>
        void BuildAdornerCorner(ref Thumb Thumb, Cursor Cursor)
        {
            if (Thumb == null)
            {
                Thumb = new Thumb()
                {
                    Background = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    Cursor = Cursor,
                    Height = 10,
                    Style = ThumbStyle,
                    Width = 10,
                };
                visualChildren.Add(Thumb);
            }
        }

        /// <summary>
        /// This method ensures that the Widths and Heights 
        /// are initialized.  Sizing to content produces Width 
        /// and Height values of Double.NaN.  Because this Adorner 
        /// explicitly resizes, the Width and Height need to be 
        /// set first.  It also sets the maximum size of the 
        /// adorned element.
        /// </summary>
        void EnforceSize(FrameworkElement Adorned)
        {
            if (Adorned.Width.Equals(Double.NaN))
                Adorned.Width = Adorned.DesiredSize.Width;

            if (Adorned.Height.Equals(Double.NaN))
                Adorned.Height = Adorned.DesiredSize.Height;

            var parent = Adorned.Parent as FrameworkElement;
            if (parent != null)
            {
                Adorned.MaxHeight = parent.ActualHeight;
                Adorned.MaxWidth = parent.ActualWidth;
            }
        }

        /// <summary>
        /// Arrange the Adorners.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            //desiredWidth and desiredHeight are the width and height of the element that’s being adorned; these will be used to place the TransformAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;

            //adornerWidth & adornerHeight are used for placement as well.
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;

            //rotateLine.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), (-adornerHeight / 2) - 10, adornerWidth, adornerHeight));
            //rotateSphere.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), (-adornerHeight / 2) - 20, adornerWidth, adornerHeight));

            top.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), -adornerHeight / 2, adornerWidth, adornerHeight));
            left.Arrange(new Rect(-adornerWidth / 2, (desiredHeight / 2) - (adornerHeight / 2), adornerWidth, adornerHeight));
            right.Arrange(new Rect(desiredWidth - adornerWidth / 2, (desiredHeight / 2) - (adornerHeight / 2), adornerWidth, adornerHeight));
            bottom.Arrange(new Rect((desiredWidth / 2) - (adornerWidth / 2), desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => visualChildren[index];

        #endregion
    }
}