using Imagin.Common;
using Imagin.Common.Configuration;
using Imagin.Common.Data;
using System;
using System.Linq;
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