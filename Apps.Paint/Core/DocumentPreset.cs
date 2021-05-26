using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class DocumentPreset : NamedObject, Imagin.Common.ICloneable
    {
        string background = $"0,0,0,0";
        public Color Background
        {
            get
            {
                var result = background.Split(',');
                return Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
            }
            set => this.Change(ref background, $"{value.A},{value.R},{value.G},{value.B}");
        }

        int bitsPerPixel = 32;
        public int BitsPerPixel
        {
            get => bitsPerPixel;
            set
            {
                this.Change(ref bitsPerPixel, value);
                this.Changed(() => Size);
            }
        }

        int height = 256;
        public int Height
        {
            get => height;
            set
            {
                if (link)
                {
                    var newSize = new Int32Size(height, width).Resize(SizeField.Height, value);
                    this.Change(ref width, newSize.Width, () => Width);
                }

                this.Change(ref height, value);
                this.Changed(() => Size);
            }
        }

        bool link = false;
        public bool Link
        {
            get => link;
            set => this.Change(ref link, value);
        }

        Imagin.Common.Media.PixelFormat pixelFormat = Imagin.Common.Media.PixelFormat.Rgba128Float;
        public Imagin.Common.Media.PixelFormat PixelFormat
        {
            get => pixelFormat;
            set => this.Change(ref pixelFormat, value);
        }

        float resolution = 72f;
        public float Resolution
        {
            get => resolution;
            set => this.Change(ref resolution, value);
        }

        public long Size
        {
            get
            {
                var result = (height * width).Double();
                result *= bitsPerPixel.Double() / 8.0;
                return result.Int64();
            }
        }

        int width = 256;
        public int Width
        {
            get => width;
            set
            {
                if (link)
                {
                    var newSize = new Int32Size(height, width).Resize(SizeField.Width, value);
                    this.Change(ref height, newSize.Height, () => Height);
                }

                this.Change(ref width, value);
                this.Changed(() => Size);
            }
        }

        public DocumentPreset() : this(string.Empty) { }

        public DocumentPreset(string name) : base(name) { }

        object Imagin.Common.ICloneable.Clone()
        {
            return Clone();
        }
        public DocumentPreset Clone()
        {
            return new DocumentPreset()
            {
                Background = Background,
                Height = height,
                Name = Name,
                PixelFormat = pixelFormat,
                Resolution = resolution,
                Width = width
            };
        }
    }
}