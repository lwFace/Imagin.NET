using Imagin.Common;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Configuration;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Random
{
    [Serializable]
    public class Options : Data
    {
        string characters = string.Empty;
        [Hidden]
        public string Characters
        {
            get => characters;
            set => this.Change(ref characters, string.Concat(value.Distinct()));
        }

        string customCharacters = string.Empty;
        [Category("Characters")]
        [DisplayName("Custom")]
        [StringFormat(StringFormat.Tokens, ' ')]
        public string CustomCharacters
        {
            get => customCharacters;
            set
            {
                var old = string.Empty;

                var result = string.Empty;
                foreach (var i in value)
                {
                    if (i != ' ')
                    {

                        if (!old.Contains(i))
                        {
                            result += i;
                            old += i;
                        }
                    }
                    else
                    {
                        result += i;
                        old = string.Empty;
                    }
                }

                this.Change(ref customCharacters, result);
                this.Changed(() => CustomCharactersList);
            }
        }

        [Hidden]
        public StringCollection CustomCharactersList
        {
            get
            {
                var result = new StringCollection();
                customCharacters.Split(Array<char>.New(' '), StringSplitOptions.RemoveEmptyEntries).ForEach(i => result.Add(i));
                return result;
            }
        }

        string fontFamily;
        [Category("Format")]
        [DisplayName("Font family")]
        public FontFamily FontFamily
        {
            get
            {
                if (fontFamily == null)
                    return default;

                FontFamily result = null;
                Try.Invoke(() => result = new FontFamily(fontFamily));
                return result;
            }
            set => this.Change(ref fontFamily, value.Source);
        }

        double fontSize = 16;
        [Category("Format")]
        [DisplayName("Font size")]
        [Range(8.0, 72.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        HorizontalAlignment textHorizontalAlignment = HorizontalAlignment.Left;
        [Category("Text")]
        [DisplayName("Horizontal alignment")]
        public HorizontalAlignment TextHorizontalAlignment
        {
            get => textHorizontalAlignment;
            set => this.Change(ref textHorizontalAlignment, value);
        }

        VerticalAlignment textVerticalAlignment = VerticalAlignment.Top;
        [Category("Text")]
        [DisplayName("Vertical alignment")]
        public VerticalAlignment TextVerticalAlignment
        {
            get => textVerticalAlignment;
            set => this.Change(ref textVerticalAlignment, value);
        }

        double windowHeight = 360;
        [Hidden]
        public double WindowHeight
        {
            get => windowHeight;
            set => this.Change(ref windowHeight, value);
        }

        double windowWidth = 720;
        [Hidden]
        public double WindowWidth
        {
            get => windowWidth;
            set => this.Change(ref windowWidth, value);
        }

        StringCollection history = new StringCollection();
        [Hidden]
        public StringCollection History
        {
            get => history ?? (history = new StringCollection());
            set => this.Change(ref history, value);
        }

        uint length = 50;
        [Hidden]
        public uint Length
        {
            get => length;
            set => this.Change(ref length, value);
        }

        string text = string.Empty;
        [Hidden]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }
    }
}