using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public partial class ThicknessBox : UserControl
    {
        public static DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(double.MaxValue, FrameworkPropertyMetadataOptions.None, OnMaximumChanged));
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        static void OnMaximumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ThicknessBox).OnMaximumChanged(new OldNew<double>(e));

        public static DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(ThicknessBox), new FrameworkPropertyMetadata(double.MinValue, FrameworkPropertyMetadataOptions.None, OnMinimumChanged));
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        static void OnMinimumChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ThicknessBox).OnMinimumChanged(new OldNew<double>(e));

        public static DependencyProperty SpacingProperty = DependencyProperty.Register(nameof(Spacing), typeof(Thickness), typeof(ThicknessBox), new FrameworkPropertyMetadata(new Thickness(0, 0, 5, 0), FrameworkPropertyMetadataOptions.None));
        public Thickness Spacing
        {
            get => (Thickness)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public static DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(Thickness), typeof(ThicknessBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.None));
        public Thickness Thickness
        {
            get => (Thickness)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        List<BindingExpressionBase> bindings = new List<BindingExpressionBase>();

        public ThicknessBox()
        {
            InitializeComponent();
            Grid.DataContext = this;

            var index = 0;
            foreach (DoubleUpDown i in Grid.Children)
            {
                IValueConverter converter = null;
                switch (index)
                {
                    case 0:
                        converter = new DefaultConverter<Thickness, double>
                        (
                            j => j.Left,
                            j => new Thickness(j, Thickness.Top, Thickness.Right, Thickness.Bottom)
                        );
                        break;
                    case 1:
                        converter = new DefaultConverter<Thickness, double>
                        (
                            j => j.Top,
                            j => new Thickness(Thickness.Left, j, Thickness.Right, Thickness.Bottom)
                        );
                        break;
                    case 2:
                        converter = new DefaultConverter<Thickness, double>
                        (
                            j => j.Right,
                            j => new Thickness(Thickness.Left, Thickness.Top, j, Thickness.Bottom)
                        );
                        break;
                    case 3:
                        converter = new DefaultConverter<Thickness, double>
                        (
                            j => j.Bottom,
                            j => new Thickness(Thickness.Left, Thickness.Top, Thickness.Right, j)
                        );
                        break;
                }

                var result = i.Bind(DoubleUpDown.ValueProperty.Property, nameof(Thickness), this, BindingMode.TwoWay, converter);
                bindings.Add(result);
                index++;
            }
        }

        void Update()
        {
            foreach (var i in bindings)
                i.UpdateTarget();
        }

        protected virtual void OnMaximumChanged(OldNew<double> input) => Update();

        protected virtual void OnMinimumChanged(OldNew<double> input) => Update();
    }
}