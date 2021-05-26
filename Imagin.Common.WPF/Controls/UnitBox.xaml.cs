using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public partial class UnitBox : UserControl
    {
        internal static DependencyProperty ActualValueProperty = DependencyProperty.Register(nameof(ActualValue), typeof(double), typeof(UnitBox), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None));
        public double ActualValue
        {
            get => (double)GetValue(ActualValueProperty);
            internal set => SetValue(ActualValueProperty, value);
        }

        public static DependencyProperty ResolutionProperty = DependencyProperty.Register(nameof(Resolution), typeof(float), typeof(UnitBox), new FrameworkPropertyMetadata(72f, FrameworkPropertyMetadataOptions.None, OnResolutionChanged));
        public float Resolution
        {
            get => (float)GetValue(ResolutionProperty);
            set => SetValue(ResolutionProperty, value);
        }
        static void OnResolutionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UnitBox>().OnResolutionChanged(new OldNew<float>(e));

        public static DependencyProperty StringFormatProperty = DependencyProperty.Register(nameof(StringFormat), typeof(string), typeof(UnitBox), new FrameworkPropertyMetadata("N0", FrameworkPropertyMetadataOptions.None));
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        public static DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(GraphicalUnit), typeof(UnitBox), new FrameworkPropertyMetadata(GraphicalUnit.Pixel, FrameworkPropertyMetadataOptions.None, OnUnitChanged));
        public GraphicalUnit Unit
        {
            get => (GraphicalUnit)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }
        static void OnUnitChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<UnitBox>().OnUnitChanged(new OldNew<GraphicalUnit>(e));

        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(UnitBox), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        BindingExpressionBase j = null;

        public UnitBox()
        {
            j = this.Bind(ActualValueProperty, nameof(Value), this, BindingMode.TwoWay, new DefaultConverter<double, double>
            (
                i => i.Convert(GraphicalUnit.Pixel, Unit, Resolution),
                i => i.Convert(Unit, GraphicalUnit.Pixel, Resolution)
            ));
            InitializeComponent();
        }

        protected virtual void OnResolutionChanged(OldNew<float> i) => j?.UpdateTarget();

        protected virtual void OnUnitChanged(OldNew<GraphicalUnit> i) => j?.UpdateTarget();
    }
}