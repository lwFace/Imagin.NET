using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class GradientPicker : UserControl
    {
        #region Properties

        Handle handleGradient = false;

        GradientStopCollection stops => Gradient.GradientStops;

        System.Random random = new System.Random();

        public event EventHandler<EventArgs<LinearGradientBrush>> GradientChanged;

        public static GradientStopCollection DefaultGradientStops
        {
            get
            {
                var GradientStops = new GradientStopCollection();
                GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                GradientStops.Add(new GradientStop(Colors.Black, 1.0));
                return GradientStops;
            }
        }

        public static LinearGradientBrush DefaultGradient => new LinearGradientBrush(DefaultGradientStops, new Point(0.0, 0.5), new Point(1.0, 0.5));

        public static DependencyProperty BandsProperty = DependencyProperty.Register(nameof(Bands), typeof(int), typeof(GradientPicker), new PropertyMetadata(2, new PropertyChangedCallback(OnBandsChanged)));
        public int Bands
        {
            get => (int)GetValue(BandsProperty);
            set => SetValue(BandsProperty, value);
        }
        static void OnBandsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientPicker)i).OnBandsChanged(new OldNew<int>(e));

        public static DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(LinearGradientBrush), typeof(GradientPicker), new PropertyMetadata(default(LinearGradientBrush), OnGradientChanged, new CoerceValueCallback(OnGradientCoerced)));
        public LinearGradientBrush Gradient
        {
            get => (LinearGradientBrush)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }
        static void OnGradientChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientPicker)i).OnGradientChanged(new OldNew<LinearGradientBrush>(e));
        static object OnGradientCoerced(DependencyObject i, object value)
        {
            return ((GradientPicker)i).OnGradientCoerced(value as LinearGradientBrush);
        }

        public static DependencyProperty OffsetProperty = DependencyProperty.Register(nameof(Offset), typeof(double), typeof(GradientPicker), new PropertyMetadata(0.0, OnOffsetChanged));
        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
        static void OnOffsetChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientPicker)i).OnOffsetChanged(new OldNew<double>(e));

        public static DependencyProperty PreviewBorderBrushProperty = DependencyProperty.Register(nameof(PreviewBorderBrush), typeof(Brush), typeof(GradientPicker), new PropertyMetadata(Brushes.Black));
        public Brush PreviewBorderBrush
        {
            get => (Brush)GetValue(PreviewBorderBrushProperty);
            set => SetValue(PreviewBorderBrushProperty, value);
        }

        public static DependencyProperty PreviewBorderThicknessProperty = DependencyProperty.Register(nameof(PreviewBorderThickness), typeof(Thickness), typeof(GradientPicker), new PropertyMetadata(default(Thickness)));
        public Thickness PreviewBorderThickness
        {
            get => (Thickness)GetValue(PreviewBorderThicknessProperty);
            set => SetValue(PreviewBorderThicknessProperty, value);
        }

        public static DependencyProperty SelectedBandProperty = DependencyProperty.Register(nameof(SelectedBand), typeof(int), typeof(GradientPicker), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedBandChanged)));
        public int SelectedBand
        {
            get => (int)GetValue(SelectedBandProperty);
            set => SetValue(SelectedBandProperty, value);
        }
        static void OnSelectedBandChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientPicker)i).OnSelectedBandChanged(new OldNew<int>(e));

        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(GradientPicker), new PropertyMetadata(Colors.Transparent, OnSelectedColorChanged));
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
        static void OnSelectedColorChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientPicker)i).OnSelectedColorChanged(new OldNew<Color>(e));

        #endregion

        #region GradientPicker

        public GradientPicker()
        {
            InitializeComponent();

            SetCurrentValue(GradientProperty, DefaultGradient);
            SetCurrentValue(PreviewBorderThicknessProperty, new Thickness(1.0));
            SetCurrentValue(SelectedBandProperty, 1);
        }

        #endregion

        #region Methods

        protected virtual void OnGradientChanged()
        {
            GradientChanged?.Invoke(this, new EventArgs<LinearGradientBrush>(Gradient));
        }

        /// ---------------------------------------------------------------------------------------

        void Shift(bool ShiftAll)
        {
            for (var k = 0; k < (ShiftAll ? stops.Count : stops.Count - 1); k++)
                stops[k].Offset = k == 0 ? 0 : stops[k - 1].Offset + 1d.Divide(stops.Count.Double() - 1d).Round(2);
        }

        /// ---------------------------------------------------------------------------------------

        protected virtual void OnBandsChanged(OldNew<int> input)
        {
            if (input.New > stops.Count)
            {
                //Number of bands to add
                var newValue = input.New - stops.Count;
                for (var i = 0; i < newValue; i++)
                {
                    var randomColor = Color.FromRgb(random.Next(0, 255).Byte(), random.Next(0, 255).Byte(), random.Next(0, 255).Byte());

                    //Add new band with random color
                    stops.Add(new GradientStop(randomColor, 1d));

                    //If next to last band index is valid
                    if (stops.Count > 1)
                        Shift(false);
                }
            }
            else if (input.New < stops.Count)
            {
                for (var i = stops.Count - 1; i >= Bands; i--)
                    stops.Remove(stops[i]);

                if (SelectedBand > stops.Count)
                    SelectedBand = stops.Count;

                Shift(true);
            }
            OnGradientChanged();
        }

        /// ---------------------------------------------------------------------------------------

        protected virtual void OnGradientChanged(OldNew<LinearGradientBrush> input)
        {
            if (!handleGradient)
            {
                SetCurrentValue(BandsProperty, stops.Count);
                SetCurrentValue(SelectedBandProperty, -1);
                SetCurrentValue(SelectedBandProperty, 1);
            }
        }

        protected virtual LinearGradientBrush OnGradientCoerced(LinearGradientBrush input)
        {
            if (input == null || input.GradientStops.Count == 0)
                return DefaultGradient;

            return input;
        }

        /// ---------------------------------------------------------------------------------------

        protected virtual void OnOffsetChanged(OldNew<double> input)
        {
            handleGradient = true;
            stops[SelectedBand - 1].Offset = input.New;
            handleGradient = false;
            OnGradientChanged();
        }

        protected virtual void OnSelectedBandChanged(OldNew<int> input)
        {
            if (input.New > -1)
            {
                handleGradient = true;
                SetCurrentValue(OffsetProperty, stops[input.New - 1].Offset);
                SetCurrentValue(SelectedColorProperty, stops[input.New - 1].Color);
                handleGradient = false;
            }
        }
        
        protected virtual void OnSelectedColorChanged(OldNew<Color> input)
        {
            handleGradient = true;
            stops[SelectedBand - 1].Color = input.New;
            handleGradient = false;
            OnGradientChanged();
        }

        #endregion
    }
}