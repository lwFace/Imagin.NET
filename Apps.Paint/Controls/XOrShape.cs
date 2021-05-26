﻿using Imagin.Common.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Paint
{
    public abstract class XOrShape : FrameworkElement
    {
        #region Legacy

        /*
        public const int R2_NOT = 6;  // Inverted drawing mode

        [DllImport("gdi32.dll", EntryPoint = "SetROP2", CallingConvention = CallingConvention.StdCall)]
        public extern static int SetROP2(IntPtr hdc, int fnDrawMode);

        [DllImport("user32.dll", EntryPoint = "GetDC", CallingConvention = CallingConvention.StdCall)]
        public extern static IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC", CallingConvention = CallingConvention.StdCall)]
        public extern static IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", EntryPoint = "MoveToEx", CallingConvention = CallingConvention.StdCall)]
        public extern static bool MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

        [DllImport("gdi32.dll", EntryPoint = "LineTo", CallingConvention = CallingConvention.StdCall)]
        public extern static bool LineTo(IntPtr hdc, int x, int y);
        */

        #endregion

        public static DependencyProperty DashStyle1Property = DependencyProperty.Register(nameof(DashStyle1), typeof(DashStyle), typeof(XOrShape), new FrameworkPropertyMetadata(default(DashStyle), FrameworkPropertyMetadataOptions.None, OnChanged));
        [TypeConverter(typeof(DashStyleTypeConverter))]
        public DashStyle DashStyle1
        {
            get => (DashStyle)GetValue(DashStyle1Property);
            set => SetValue(DashStyle1Property, value);
        }

        public static DependencyProperty DashStyle2Property = DependencyProperty.Register(nameof(DashStyle2), typeof(DashStyle), typeof(XOrShape), new FrameworkPropertyMetadata(default(DashStyle), FrameworkPropertyMetadataOptions.None, OnChanged));
        [TypeConverter(typeof(DashStyleTypeConverter))]
        public DashStyle DashStyle2
        {
            get => (DashStyle)GetValue(DashStyle2Property);
            set => SetValue(DashStyle2Property, value);
        }

        public static DependencyProperty StrokeThickness1Property = DependencyProperty.Register(nameof(StrokeThickness1), typeof(double), typeof(XOrShape), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public double StrokeThickness1
        {
            get => (double)GetValue(StrokeThickness1Property);
            set => SetValue(StrokeThickness1Property, value);
        }

        public static DependencyProperty StrokeThickness2Property = DependencyProperty.Register(nameof(StrokeThickness2), typeof(double), typeof(XOrShape), new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.None, OnChanged));
        public double StrokeThickness2
        {
            get => (double)GetValue(StrokeThickness2Property);
            set => SetValue(StrokeThickness2Property, value);
        }

        public static DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(XOrShape), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        public XOrShape() : base()
        {
            SetCurrentValue(DashStyle1Property, DashStyles.Solid);
            SetCurrentValue(DashStyle2Property, DashStyles.Solid);
        }

        void OnChanged() => InvalidateVisual();

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }

        protected static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var shape = d as XOrShape;
            //var handle = GetDC(IntPtr.Zero);
            //SetROP2(handle, R2_NOT);
            shape.OnChanged();
            //ReleaseDC(IntPtr.Zero, handle);
        }
    }

    public abstract class XOrRegion : XOrShape
    {
        public static DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int X
        {
            get => (int)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int Y
        {
            get => (int)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public static DependencyProperty HProperty = DependencyProperty.Register(nameof(H), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int H
        {
            get => (int)GetValue(HProperty);
            set => SetValue(HProperty, value);
        }

        public static DependencyProperty WProperty = DependencyProperty.Register(nameof(W), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int W
        {
            get => (int)GetValue(WProperty);
            set => SetValue(WProperty, value);
        }
    }

    public class XOrLine : XOrShape
    {
        public static DependencyProperty X1Property = DependencyProperty.Register(nameof(X1), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int X1
        {
            get => (int)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        public static DependencyProperty Y1Property = DependencyProperty.Register(nameof(Y1), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int Y1
        {
            get => (int)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public static DependencyProperty X2Property = DependencyProperty.Register(nameof(X2), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int X2
        {
            get => (int)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public static DependencyProperty Y2Property = DependencyProperty.Register(nameof(Y2), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int Y2
        {
            get => (int)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawLine(new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, new Point(X1, Y1), new Point(X2, Y2));
            drawingContext.DrawLine(new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, new Point(X1, Y1), new Point(X2, Y2));
        }
    }

    public class XOrEllipse : XOrRegion
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, new Point(X, Y), H, W);
            drawingContext.DrawEllipse(Brushes.Transparent, new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, new Point(X, Y), H, W);
        }
    }

    public class XOrPolygon : XOrShape
    {
        public static DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(XOrPolygon), new FrameworkPropertyMetadata(default(PointCollection), FrameworkPropertyMetadataOptions.None, OnChanged));
        public PointCollection Points
        {
            get => (PointCollection)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Points?.Count > 1)
            {
                var geometry = new CustomPath(Points).Geometry();
                drawingContext.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, geometry);
                drawingContext.DrawGeometry(Brushes.Transparent, new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, geometry);
            }
        }
    }

    public class XOrRectangle : XOrRegion
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, new Rect(X, Y, H, W));
            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, new Rect(X, Y, H, W));
        }
    }
}