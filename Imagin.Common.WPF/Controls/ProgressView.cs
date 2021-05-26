using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Imagin.Common.Controls
{
    public class InnerRadiusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double radius = (double)values[0];
            double stroke = (double)values[1];

            return radius - stroke;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AngleToPointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double angle = (double)values[0];
            double radius = (double)values[1];
            double stroke = (double)values[2];
            double piang = angle * Numbers.PI / 180;

            double px = System.Math.Sin(piang) * (radius - stroke / 2) + radius;
            double py = -System.Math.Cos(piang) * (radius - stroke / 2) + radius;
            return new Point(px, py);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StrokeToStartPointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double radius = (double)values[0];
            double stroke = (double)values[1];
            return new Point(radius, stroke / 2);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RadiusToSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double radius = (double)values[0];
            double stroke = (double)values[1];
            return new Size(radius - stroke / 2, radius - stroke / 2);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RadiusToCenterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double radius = (double)value;
            return new Point(radius, radius);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RadiusToDiameter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double radius = (double)value;
            return 2 * radius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AngleToIsLargeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double angle = (double)value;

            return angle > 180;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ProgressView : System.Windows.Controls.ProgressBar
    {
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(ProgressView), new PropertyMetadata(0.0));
        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty CenterBackgroundProperty = DependencyProperty.Register(nameof(CenterBackground), typeof(Brush), typeof(ProgressView), new PropertyMetadata(default(Brush)));
        public Brush CenterBackground
        {
            get => (Brush)GetValue(CenterBackgroundProperty);
            set => SetValue(CenterBackgroundProperty, value);
        }

        public static readonly DependencyProperty CenterTemplateProperty = DependencyProperty.Register(nameof(CenterTemplate), typeof(DataTemplate), typeof(ProgressView), new PropertyMetadata(null));
        public DataTemplate CenterTemplate
        {
            get => (DataTemplate)GetValue(CenterTemplateProperty);
            set => SetValue(CenterTemplateProperty, value);
        }
        
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ProgressView), new PropertyMetadata(10.0));
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(ProgressView), new PropertyMetadata(50.0));
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(nameof(InnerRadius), typeof(double), typeof(ProgressView), new PropertyMetadata(40.0));
        public double InnerRadius
        {
            get => (double)GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        public ProgressView()
        {
            DefaultStyleKey = typeof(ProgressView);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == RadiusProperty)
                Width = Height = Radius * 2;

            base.OnPropertyChanged(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Radius = System.Math.Min(ActualWidth, ActualHeight) / 2;
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            double currentAngle = Angle;
            double targetAngle = newValue / Maximum * 359.999;
            double duration = (currentAngle - targetAngle).Absolute() / 359.999 * 500;

            var animation = new DoubleAnimation(currentAngle, targetAngle, TimeSpan.FromMilliseconds(duration > 0 ? duration : 10));
            BeginAnimation(AngleProperty, animation, HandoffBehavior.Compose);
        }
    }
}