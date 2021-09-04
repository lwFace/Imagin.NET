using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class TextLayer : RegionShapeLayer
    {
        #region Properties

        [Hidden]
        public override SolidColorBrush Fill
        {
            get => base.Fill;
            set => base.Fill = value;
        }

        SolidColorBrush fontColor = System.Windows.Media.Brushes.Black;
        [DisplayName("Font color")]
        public SolidColorBrush FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                this.fontColor = value;
                OnPropertyChanged("FontColor");
            }
        }

        System.Windows.Media.FontFamily fontFamily = new System.Windows.Media.FontFamily("Arial");
        [DisplayName("Font family")]
        public System.Windows.Media.FontFamily FontFamily
        {
            get
            {
                return fontFamily;
            }
            set
            {
                fontFamily = value;
                OnPropertyChanged("FontFamily");
            }
        }

        double fontSize = 12.0;
        [DisplayName("Font size")]
        public double FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                this.fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;
        [DisplayName("Font style")]
        public System.Drawing.FontStyle FontStyle
        {
            get => fontStyle;
            set => this.Change(ref fontStyle, value);
        }

        [Hidden]
        public override SolidColorBrush Stroke
        {
            get => base.Stroke;
            set => base.Stroke = value;
        }

        [Hidden]
        public override double StrokeThickness
        {
            get => base.StrokeThickness;
            set => base.StrokeThickness = value;
        }

        string text = string.Empty;
        public string Text
        {
            get => text;
            set
            {
                this.Change(ref text, value);
                Name = value;
            }
        }

        TextAlignment horizontalTextAlignment = TextAlignment.Left;
        [DisplayName("Text alignment (horizontal)")]
        public TextAlignment HorizontalTextAlignment
        {
            get => horizontalTextAlignment;
            set => this.Change(ref horizontalTextAlignment, value);
        }

        VerticalAlignment verticalTextAlignment = VerticalAlignment.Top;
        [DisplayName("Text alignment (vertical)")]
        public VerticalAlignment VerticalTextAlignment
        {
            get => verticalTextAlignment;
            set => this.Change(ref verticalTextAlignment, value);
        }

        #endregion

        #region TextLayer

        public TextLayer(string name, int height, int width, SolidColorBrush fontColor, System.Windows.Media.FontFamily fontFamily, double fontSize, System.Drawing.FontStyle fontStyle, string text, TextAlignment horizontalTextAlignment, VerticalAlignment verticalTextAlignment) : base(LayerType.Text, name, height, width, new SolidColorBrush(Colors.Transparent), new SolidColorBrush(Colors.Transparent), 0)
        {
            FontColor = fontColor;
            FontFamily = fontFamily;
            FontSize = fontSize;
            FontStyle = fontStyle;
            Text = text;
            HorizontalTextAlignment = horizontalTextAlignment;
            VerticalTextAlignment = verticalTextAlignment;
        }

        #endregion

        #region Methods

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(FontColor):
                case nameof(FontFamily):
                case nameof(FontSize):
                case nameof(FontStretch):
                case nameof(FontStyle):
                case nameof(FontWeight):
                case nameof(Text):
                case nameof(HorizontalTextAlignment):
                case nameof(VerticalTextAlignment):
                    Render(Preview);
                    break;
            }
        }

        public sealed override Layer Clone()
        {
            return new TextLayer(Name, Height, Width, FontColor, FontFamily, FontSize, FontStyle, Text, HorizontalTextAlignment, VerticalTextAlignment)
            {
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Opacity = Opacity,
                Style = Style.Clone(),
                X = X,
                Y = Y,
            };
        }

        public override IEnumerable<System.Windows.Point> GetPoints()
        {
            yield return new System.Windows.Point(Region.X, Region.Y);
            yield return new System.Windows.Point(Region.X, Region.Y + Region.Height);
            yield return new System.Windows.Point(Region.X + Region.Width, Region.Y + Region.Height);
            yield return new System.Windows.Point(Region.X + Region.Width, Region.Y);
        }

        public override void Render(Graphics g)
        {
            int x = X, y = Y;

            switch (HorizontalTextAlignment)
            {
                case TextAlignment.Center:
                case TextAlignment.Justify:
                    //x = x + (RegionWidth.Double() / 2.0).Int32();
                    break;
                case TextAlignment.Left:
                    break;
                case TextAlignment.Right:
                    break;
            }
            switch (VerticalTextAlignment)
            {
                case VerticalAlignment.Bottom:
                    break;
                case VerticalAlignment.Center:
                case VerticalAlignment.Stretch:
                    //y = y + (RegionWidth.Double() / 2.0).Int32();
                    break;
                case VerticalAlignment.Top:
                    break;
            }

            var font = new Font(FontFamily.Source, FontSize.Single(), FontStyle);
            g.DrawString(Text, font, new SolidBrush(FontColor.Color.Int32()), x, y);
        }

        public override void Render(WriteableBitmap input)
        {
            input.DrawString(X, Y, new IntRect(new IntPoint(X, Y), new IntSize(Width, Height)), FontColor.Color, new PortableFontDesc(FontFamily.Source, FontSize.Int32()), Text);
        }

        public override string ToString() => "Text";

        #endregion
    }
}