using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ColorChip : Chip<SolidColorBrush>
    {
        Handle handleColor = false;

        Handle handleValue = false;

        public event EventHandler<EventArgs<Color>> ColorChanged;

        public static DependencyProperty AlphaProperty = DependencyProperty.Register(nameof(Alpha), typeof(byte), typeof(ColorChip), new FrameworkPropertyMetadata((byte)255, FrameworkPropertyMetadataOptions.None));
        public byte Alpha
        {
            get => (byte)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorChip), new PropertyMetadata(Colors.Gray, new PropertyChangedCallback(OnColorChanged)));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static void OnColorChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((ColorChip)i).OnColorChanged(new OldNew<Color>(e));

        public ColorChip() : base() { }

        protected virtual void OnColorChanged(OldNew<Color> input)
        {
            if (!handleColor)
            {
                ColorChanged?.Invoke(this, new EventArgs<Color>(input.New));

                handleValue = true;
                Value = new SolidColorBrush(input.New);
                handleValue = false;
            }
        }

        protected override void OnValueChanged(OldNew<SolidColorBrush> input)
        {
            if (!handleValue && input.New != null)
            {
                base.OnValueChanged(input);

                handleColor = true;
                Color = input.New.Color;
                handleColor = false;
            }
        }

        public override bool? ShowDialog()
        {
            if (Value == null)
                Value = Brushes.Transparent;

            var window = new ColorWindow(Value.Color);
            window.Title = Title;

            var result = window.ShowDialog();
            if (window.Result)
                Value = new SolidColorBrush(window.Color);

            return result;
        }
    }
}