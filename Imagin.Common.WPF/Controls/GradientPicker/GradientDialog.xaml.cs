using Imagin.Common.Input;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class GradientDialog : BaseWindow
    {
        public bool Saved { get; private set; } = false;

        public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(LinearGradientBrush), typeof(GradientDialog), new FrameworkPropertyMetadata(default(LinearGradientBrush), FrameworkPropertyMetadataOptions.None));
        public LinearGradientBrush Value
        {
            get => (LinearGradientBrush)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public GradientDialog() : this(string.Empty, GradientPicker.DefaultGradient, null) { }

        public GradientDialog(string title, LinearGradientBrush initialValue, GradientChip chip = null) : base()
        {
            InitializeComponent();
            PART_GradientPicker.Gradient = initialValue;
        }

        void OnCancel(object sender, System.EventArgs e)
        {
            Close();
        }

        void OnSave(object sender, System.EventArgs e)
        {
            Saved = true;
            Close();
        }

        protected virtual void OnGradientChanged(object sender, EventArgs<LinearGradientBrush> e)
        {
            SetCurrentValue(ValueProperty, e.Value);
        }
    }
}