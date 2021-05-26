using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    /*
    public abstract class GradientRenderer
    {
        //ColorBgra now becomes Argb
        private Argb startColor;
        private Argb endColor;
        private Vector2<float> startPoint;
        private Vector2<float> endPoint;
        private bool alphaBlending;
        private bool alphaOnly;

        private bool lerpCacheIsValid = false;
        private byte[] lerpAlphas;
        private Argb[] lerpColors;

        public Argb StartColor
        {
            get
            {
                return this.startColor;
            }
            set
            {
                if (this.startColor != value)
                {
                    this.startColor = value;
                    this.lerpCacheIsValid = false;
                }
            }
        }

        public Argb EndColor
        {
            get
            {
                return this.endColor;
            }

            set
            {
                if (this.endColor != value)
                {
                    this.endColor = value;
                    this.lerpCacheIsValid = false;
                }
            }
        }

        public Vector2<float> StartPoint
        {
            get
            {
                return this.startPoint;
            }

            set
            {
                this.startPoint = value;
            }
        }

        public Vector2<float> EndPoint
        {
            get
            {
                return this.endPoint;
            }

            set
            {
                this.endPoint = value;
            }
        }

        public bool AlphaBlending
        {
            get
            {
                return this.alphaBlending;
            }
            set
            {
                this.alphaBlending = value;
            }
        }

        public bool AlphaOnly
        {
            get
            {
                return this.alphaOnly;
            }
            set
            {
                this.alphaOnly = value;
            }
        }

        public virtual void BeforeRender()
        {
            if (!this.lerpCacheIsValid)
            {
                byte startAlpha;
                byte endAlpha;

                if (this.alphaOnly)
                {
                    ComputeAlphaOnlyValuesFromColors(this.startColor, this.endColor, out startAlpha, out endAlpha);
                }
                else
                {
                    startAlpha = this.startColor.A;
                    endAlpha = this.endColor.A;
                }

                this.lerpAlphas = new byte[256];
                this.lerpColors = new Argb[256];

                for (int i = 0; i < 256; ++i)
                {
                    byte a = (byte)i;
                    this.lerpColors[a] = Argb.Blend(this.startColor, this.endColor, a);
                    this.lerpAlphas[a] = (byte)(startAlpha + ((endAlpha - startAlpha) * a) / 255);
                }

                this.lerpCacheIsValid = true;
            }
        }

        public abstract float ComputeUnboundedLerp(int x, int y);

        public abstract float BoundLerp(float t);

        public virtual void AfterRender()
        {
        }

        private static void ComputeAlphaOnlyValuesFromColors(Argb startColor, Argb endColor, out byte startAlpha, out byte endAlpha)
        {
            startAlpha = startColor.A;
            endAlpha = (byte)(255 - endColor.A);
        }

        // i = z * 3;
        // (x / z) = ((x * masTable[i]) + masTable[i + 1]) >> masTable[i + 2)
        private static readonly uint[] masTable =
        {
            0x00000000, 0x00000000, 0,  // 0
            0x00000001, 0x00000000, 0,  // 1
            0x00000001, 0x00000000, 1,  // 2
            0xAAAAAAAB, 0x00000000, 33, // 3
            0x00000001, 0x00000000, 2,  // 4
            0xCCCCCCCD, 0x00000000, 34, // 5
            0xAAAAAAAB, 0x00000000, 34, // 6
            0x49249249, 0x49249249, 33, // 7
            0x00000001, 0x00000000, 3,  // 8
            0x38E38E39, 0x00000000, 33, // 9
            0xCCCCCCCD, 0x00000000, 35, // 10
            0xBA2E8BA3, 0x00000000, 35, // 11
            0xAAAAAAAB, 0x00000000, 35, // 12
            0x4EC4EC4F, 0x00000000, 34, // 13
            0x49249249, 0x49249249, 34, // 14
            0x88888889, 0x00000000, 35, // 15
            0x00000001, 0x00000000, 4,  // 16
            0xF0F0F0F1, 0x00000000, 36, // 17
            0x38E38E39, 0x00000000, 34, // 18
            0xD79435E5, 0xD79435E5, 36, // 19
            0xCCCCCCCD, 0x00000000, 36, // 20
            0xC30C30C3, 0xC30C30C3, 36, // 21
            0xBA2E8BA3, 0x00000000, 36, // 22
            0xB21642C9, 0x00000000, 36, // 23
            0xAAAAAAAB, 0x00000000, 36, // 24
            0x51EB851F, 0x00000000, 35, // 25
            0x4EC4EC4F, 0x00000000, 35, // 26
            0x97B425ED, 0x97B425ED, 36, // 27
            0x49249249, 0x49249249, 35, // 28
            0x8D3DCB09, 0x00000000, 36, // 29
            0x88888889, 0x00000000, 36, // 30
            0x42108421, 0x42108421, 35, // 31
            0x00000001, 0x00000000, 5,  // 32
            0x3E0F83E1, 0x00000000, 35, // 33
            0xF0F0F0F1, 0x00000000, 37, // 34
            0x75075075, 0x75075075, 36, // 35
            0x38E38E39, 0x00000000, 35, // 36
            0x6EB3E453, 0x6EB3E453, 36, // 37
            0xD79435E5, 0xD79435E5, 37, // 38
            0x69069069, 0x69069069, 36, // 39
            0xCCCCCCCD, 0x00000000, 37, // 40
            0xC7CE0C7D, 0x00000000, 37, // 41
            0xC30C30C3, 0xC30C30C3, 37, // 42
            0x2FA0BE83, 0x00000000, 35, // 43
            0xBA2E8BA3, 0x00000000, 37, // 44
            0x5B05B05B, 0x5B05B05B, 36, // 45
            0xB21642C9, 0x00000000, 37, // 46
            0xAE4C415D, 0x00000000, 37, // 47
            0xAAAAAAAB, 0x00000000, 37, // 48
            0x5397829D, 0x00000000, 36, // 49
            0x51EB851F, 0x00000000, 36, // 50
            0xA0A0A0A1, 0x00000000, 37, // 51
            0x4EC4EC4F, 0x00000000, 36, // 52
            0x9A90E7D9, 0x9A90E7D9, 37, // 53
            0x97B425ED, 0x97B425ED, 37, // 54
            0x94F2094F, 0x94F2094F, 37, // 55
            0x49249249, 0x49249249, 36, // 56
            0x47DC11F7, 0x47DC11F7, 36, // 57
            0x8D3DCB09, 0x00000000, 37, // 58
            0x22B63CBF, 0x00000000, 35, // 59
            0x88888889, 0x00000000, 37, // 60
            0x4325C53F, 0x00000000, 36, // 61
            0x42108421, 0x42108421, 36, // 62
            0x41041041, 0x41041041, 36, // 63
            0x00000001, 0x00000000, 6,  // 64
            0xFC0FC0FD, 0x00000000, 38, // 65
            0x3E0F83E1, 0x00000000, 36, // 66
            0x07A44C6B, 0x00000000, 33, // 67
            0xF0F0F0F1, 0x00000000, 38, // 68
            0x76B981DB, 0x00000000, 37, // 69
            0x75075075, 0x75075075, 37, // 70
            0xE6C2B449, 0x00000000, 38, // 71
            0x38E38E39, 0x00000000, 36, // 72
            0x381C0E07, 0x381C0E07, 36, // 73
            0x6EB3E453, 0x6EB3E453, 37, // 74
            0x1B4E81B5, 0x00000000, 35, // 75
            0xD79435E5, 0xD79435E5, 38, // 76
            0x3531DEC1, 0x00000000, 36, // 77
            0x69069069, 0x69069069, 37, // 78
            0xCF6474A9, 0x00000000, 38, // 79
            0xCCCCCCCD, 0x00000000, 38, // 80
            0xCA4587E7, 0x00000000, 38, // 81
            0xC7CE0C7D, 0x00000000, 38, // 82
            0x3159721F, 0x00000000, 36, // 83
            0xC30C30C3, 0xC30C30C3, 38, // 84
            0xC0C0C0C1, 0x00000000, 38, // 85
            0x2FA0BE83, 0x00000000, 36, // 86
            0x2F149903, 0x00000000, 36, // 87
            0xBA2E8BA3, 0x00000000, 38, // 88
            0xB81702E1, 0x00000000, 38, // 89
            0x5B05B05B, 0x5B05B05B, 37, // 90
            0x2D02D02D, 0x2D02D02D, 36, // 91
            0xB21642C9, 0x00000000, 38, // 92
            0xB02C0B03, 0x00000000, 38, // 93
            0xAE4C415D, 0x00000000, 38, // 94
            0x2B1DA461, 0x2B1DA461, 36, // 95
            0xAAAAAAAB, 0x00000000, 38, // 96
            0xA8E83F57, 0xA8E83F57, 38, // 97
            0x5397829D, 0x00000000, 37, // 98
            0xA57EB503, 0x00000000, 38, // 99
            0x51EB851F, 0x00000000, 37, // 100
            0xA237C32B, 0xA237C32B, 38, // 101
            0xA0A0A0A1, 0x00000000, 38, // 102
            0x9F1165E7, 0x9F1165E7, 38, // 103
            0x4EC4EC4F, 0x00000000, 37, // 104
            0x27027027, 0x27027027, 36, // 105
            0x9A90E7D9, 0x9A90E7D9, 38, // 106
            0x991F1A51, 0x991F1A51, 38, // 107
            0x97B425ED, 0x97B425ED, 38, // 108
            0x2593F69B, 0x2593F69B, 36, // 109
            0x94F2094F, 0x94F2094F, 38, // 110
            0x24E6A171, 0x24E6A171, 36, // 111
            0x49249249, 0x49249249, 37, // 112
            0x90FDBC09, 0x90FDBC09, 38, // 113
            0x47DC11F7, 0x47DC11F7, 37, // 114
            0x8E78356D, 0x8E78356D, 38, // 115
            0x8D3DCB09, 0x00000000, 38, // 116
            0x23023023, 0x23023023, 36, // 117
            0x22B63CBF, 0x00000000, 36, // 118
            0x44D72045, 0x00000000, 37, // 119
            0x88888889, 0x00000000, 38, // 120
            0x8767AB5F, 0x8767AB5F, 38, // 121
            0x4325C53F, 0x00000000, 37, // 122
            0x85340853, 0x85340853, 38, // 123
            0x42108421, 0x42108421, 37, // 124
            0x10624DD3, 0x00000000, 35, // 125
            0x41041041, 0x41041041, 37, // 126
            0x10204081, 0x10204081, 35, // 127
            0x00000001, 0x00000000, 7,  // 128
            0x0FE03F81, 0x00000000, 35, // 129
            0xFC0FC0FD, 0x00000000, 39, // 130
            0xFA232CF3, 0x00000000, 39, // 131
            0x3E0F83E1, 0x00000000, 37, // 132
            0xF6603D99, 0x00000000, 39, // 133
            0x07A44C6B, 0x00000000, 34, // 134
            0xF2B9D649, 0x00000000, 39, // 135
            0xF0F0F0F1, 0x00000000, 39, // 136
            0x077975B9, 0x00000000, 34, // 137
            0x76B981DB, 0x00000000, 38, // 138
            0x75DED953, 0x00000000, 38, // 139
            0x75075075, 0x75075075, 38, // 140
            0x3A196B1F, 0x00000000, 37, // 141
            0xE6C2B449, 0x00000000, 39, // 142
            0xE525982B, 0x00000000, 39, // 143
            0x38E38E39, 0x00000000, 37, // 144
            0xE1FC780F, 0x00000000, 39, // 145
            0x381C0E07, 0x381C0E07, 37, // 146
            0xDEE95C4D, 0x00000000, 39, // 147
            0x6EB3E453, 0x6EB3E453, 38, // 148
            0xDBEB61EF, 0x00000000, 39, // 149
            0x1B4E81B5, 0x00000000, 36, // 150
            0x36406C81, 0x00000000, 37, // 151
            0xD79435E5, 0xD79435E5, 39, // 152
            0xD62B80D7, 0x00000000, 39, // 153
            0x3531DEC1, 0x00000000, 37, // 154
            0xD3680D37, 0x00000000, 39, // 155
            0x69069069, 0x69069069, 38, // 156
            0x342DA7F3, 0x00000000, 37, // 157
            0xCF6474A9, 0x00000000, 39, // 158
            0xCE168A77, 0xCE168A77, 39, // 159
            0xCCCCCCCD, 0x00000000, 39, // 160
            0xCB8727C1, 0x00000000, 39, // 161
            0xCA4587E7, 0x00000000, 39, // 162
            0xC907DA4F, 0x00000000, 39, // 163
            0xC7CE0C7D, 0x00000000, 39, // 164
            0x634C0635, 0x00000000, 38, // 165
            0x3159721F, 0x00000000, 37, // 166
            0x621B97C3, 0x00000000, 38, // 167
            0xC30C30C3, 0xC30C30C3, 39, // 168
            0x60F25DEB, 0x00000000, 38, // 169
            0xC0C0C0C1, 0x00000000, 39, // 170
            0x17F405FD, 0x17F405FD, 36, // 171
            0x2FA0BE83, 0x00000000, 37, // 172
            0xBD691047, 0xBD691047, 39, // 173
            0x2F149903, 0x00000000, 37, // 174
            0x5D9F7391, 0x00000000, 38, // 175
            0xBA2E8BA3, 0x00000000, 39, // 176
            0x5C90A1FD, 0x5C90A1FD, 38, // 177
            0xB81702E1, 0x00000000, 39, // 178
            0x5B87DDAD, 0x5B87DDAD, 38, // 179
            0x5B05B05B, 0x5B05B05B, 38, // 180
            0xB509E68B, 0x00000000, 39, // 181
            0x2D02D02D, 0x2D02D02D, 37, // 182
            0xB30F6353, 0x00000000, 39, // 183
            0xB21642C9, 0x00000000, 39, // 184
            0x1623FA77, 0x1623FA77, 36, // 185
            0xB02C0B03, 0x00000000, 39, // 186
            0xAF3ADDC7, 0x00000000, 39, // 187
            0xAE4C415D, 0x00000000, 39, // 188
            0x15AC056B, 0x15AC056B, 36, // 189
            0x2B1DA461, 0x2B1DA461, 37, // 190
            0xAB8F69E3, 0x00000000, 39, // 191
            0xAAAAAAAB, 0x00000000, 39, // 192
            0x15390949, 0x00000000, 36, // 193
            0xA8E83F57, 0xA8E83F57, 39, // 194
            0x15015015, 0x15015015, 36, // 195
            0x5397829D, 0x00000000, 38, // 196
            0xA655C439, 0xA655C439, 39, // 197
            0xA57EB503, 0x00000000, 39, // 198
            0x5254E78F, 0x00000000, 38, // 199
            0x51EB851F, 0x00000000, 38, // 200
            0x028C1979, 0x00000000, 33, // 201
            0xA237C32B, 0xA237C32B, 39, // 202
            0xA16B312F, 0x00000000, 39, // 203
            0xA0A0A0A1, 0x00000000, 39, // 204
            0x4FEC04FF, 0x00000000, 38, // 205
            0x9F1165E7, 0x9F1165E7, 39, // 206
            0x27932B49, 0x00000000, 37, // 207
            0x4EC4EC4F, 0x00000000, 38, // 208
            0x9CC8E161, 0x00000000, 39, // 209
            0x27027027, 0x27027027, 37, // 210
            0x9B4C6F9F, 0x00000000, 39, // 211
            0x9A90E7D9, 0x9A90E7D9, 39, // 212
            0x99D722DB, 0x00000000, 39, // 213
            0x991F1A51, 0x991F1A51, 39, // 214
            0x4C346405, 0x00000000, 38, // 215
            0x97B425ED, 0x97B425ED, 39, // 216
            0x4B809701, 0x4B809701, 38, // 217
            0x2593F69B, 0x2593F69B, 37, // 218
            0x12B404AD, 0x12B404AD, 36, // 219
            0x94F2094F, 0x94F2094F, 39, // 220
            0x25116025, 0x25116025, 37, // 221
            0x24E6A171, 0x24E6A171, 37, // 222
            0x24BC44E1, 0x24BC44E1, 37, // 223
            0x49249249, 0x49249249, 38, // 224
            0x91A2B3C5, 0x00000000, 39, // 225
            0x90FDBC09, 0x90FDBC09, 39, // 226
            0x905A3863, 0x905A3863, 39, // 227
            0x47DC11F7, 0x47DC11F7, 38, // 228
            0x478BBCED, 0x00000000, 38, // 229
            0x8E78356D, 0x8E78356D, 39, // 230
            0x46ED2901, 0x46ED2901, 38, // 231
            0x8D3DCB09, 0x00000000, 39, // 232
            0x2328A701, 0x2328A701, 37, // 233
            0x23023023, 0x23023023, 37, // 234
            0x45B81A25, 0x45B81A25, 38, // 235
            0x22B63CBF, 0x00000000, 37, // 236
            0x08A42F87, 0x08A42F87, 35, // 237
            0x44D72045, 0x00000000, 38, // 238
            0x891AC73B, 0x00000000, 39, // 239
            0x88888889, 0x00000000, 39, // 240
            0x10FEF011, 0x00000000, 36, // 241
            0x8767AB5F, 0x8767AB5F, 39, // 242
            0x86D90545, 0x00000000, 39, // 243
            0x4325C53F, 0x00000000, 38, // 244
            0x85BF3761, 0x85BF3761, 39, // 245
            0x85340853, 0x85340853, 39, // 246
            0x10953F39, 0x10953F39, 36, // 247
            0x42108421, 0x42108421, 38, // 248
            0x41CC9829, 0x41CC9829, 38, // 249
            0x10624DD3, 0x00000000, 36, // 250
            0x828CBFBF, 0x00000000, 39, // 251
            0x41041041, 0x41041041, 38, // 252
            0x81848DA9, 0x00000000, 39, // 253
            0x10204081, 0x10204081, 36, // 254
            0x80808081, 0x00000000, 39  // 255
        };

        public static int FastDivideShortByByte(ushort n, byte d)
        {
            int i = d * 3;
            uint m = masTable[i];
            uint a = masTable[i + 1];
            uint s = masTable[i + 2];

            uint nTimesMPlusA = unchecked((n * m) + a);
            uint shifted = nTimesMPlusA >> (int)s;
            int r = (int)shifted;

            return r;
        }

        public static byte FastScaleByteByByte(byte a, byte b)
        {
            int r1 = a * b + 0x80;
            int r2 = ((r1 >> 8) + r1) >> 8;
            return (byte)r2;
        }

        public unsafe void Render(System.Drawing.Rectangle[] rois, int startIndex, int length)
        {
            byte startAlpha;
            byte endAlpha;

            if (this.alphaOnly)
            {
                ComputeAlphaOnlyValuesFromColors(this.startColor, this.endColor, out startAlpha, out endAlpha);
            }
            else
            {
                startAlpha = this.startColor.A;
                endAlpha = this.endColor.A;
            }

            for (int ri = startIndex; ri < startIndex + length; ++ri)
            {
                System.Drawing.Rectangle rect = rois[ri];

                if (this.startPoint == this.endPoint)
                {
                    // Start and End point are the same ... fill with solid color.
                    for (int y = rect.Top; y < rect.Bottom; ++y)
                    {
                        //Argb* pixelPtr = surface.GetPointAddress(rect.Left, y);

                        for (int x = rect.Left; x < rect.Right; ++x)
                        {
                            Argb result;

                            if (this.alphaOnly && this.alphaBlending)
                            {
                                byte resultAlpha = (byte)FastDivideShortByByte((ushort)(pixelPtr->A * endAlpha), 255);
                                result = *pixelPtr;
                                result.A = resultAlpha;
                            }
                            else if (this.alphaOnly && !this.alphaBlending)
                            {
                                result = *pixelPtr;
                                result.A = endAlpha;
                            }
                            else if (!this.alphaOnly && this.alphaBlending)
                            {
                                result = this.normalBlendOp.Apply(*pixelPtr, this.endColor);
                            }
                            else //if (!this.alphaOnly && !this.alphaBlending)
                            {
                                result = this.endColor;
                            }

                            *pixelPtr = result;
                            ++pixelPtr;
                        }
                    }
                }
                else
                {
                    for (int y = rect.Top; y < rect.Bottom; ++y)
                    {
                        //Argb* pixelPtr = surface.GetPointAddress(rect.Left, y);

                        if (this.alphaOnly && this.alphaBlending)
                        {
                            for (int x = rect.Left; x < rect.Right; ++x)
                            {
                                float lerpUnbounded = ComputeUnboundedLerp(x, y);
                                float lerpBounded = BoundLerp(lerpUnbounded);
                                byte lerpByte = (byte)(lerpBounded * 255.0f);
                                byte lerpAlpha = this.lerpAlphas[lerpByte];
                                byte resultAlpha = FastScaleByteByByte(pixelPtr->A, lerpAlpha);
                                pixelPtr->A = resultAlpha;
                                ++pixelPtr;
                            }
                        }
                        else if (this.alphaOnly && !this.alphaBlending)
                        {
                            for (int x = rect.Left; x < rect.Right; ++x)
                            {
                                float lerpUnbounded = ComputeUnboundedLerp(x, y);
                                float lerpBounded = BoundLerp(lerpUnbounded);
                                byte lerpByte = (byte)(lerpBounded * 255.0f);
                                byte lerpAlpha = this.lerpAlphas[lerpByte];
                                pixelPtr->A = lerpAlpha;
                                ++pixelPtr;
                            }
                        }
                        else if (!this.alphaOnly && (this.alphaBlending && (startAlpha != 255 || endAlpha != 255)))
                        {
                            // If we're doing all color channels, and we're doing alpha blending, and if alpha blending is necessary
                            for (int x = rect.Left; x < rect.Right; ++x)
                            {
                                float lerpUnbounded = ComputeUnboundedLerp(x, y);
                                float lerpBounded = BoundLerp(lerpUnbounded);
                                byte lerpByte = (byte)(lerpBounded * 255.0f);
                                Argb lerpColor = this.lerpColors[lerpByte];
                                Argb result = this.normalBlendOp.Apply(*pixelPtr, lerpColor);
                                *pixelPtr = result;
                                ++pixelPtr;
                            }
                        }
                        else //if (!this.alphaOnly && !this.alphaBlending) // or sC.A == 255 && eC.A == 255
                        {
                            for (int x = rect.Left; x < rect.Right; ++x)
                            {
                                float lerpUnbounded = ComputeUnboundedLerp(x, y);
                                float lerpBounded = BoundLerp(lerpUnbounded);
                                byte lerpByte = (byte)(lerpBounded * 255.0f);
                                Argb lerpColor = this.lerpColors[lerpByte];
                                *pixelPtr = lerpColor;
                                ++pixelPtr;
                            }
                        }
                    }
                }
            }

            AfterRender();
        }

        protected internal GradientRenderer(bool alphaOnly)
        {
            this.alphaOnly = alphaOnly;
        }
    }

    public abstract class LinearBase
       : GradientRenderer
    {
        protected float dtdx;
        protected float dtdy;
        /// <summary>
        /// Returns the Magnitude (distance to origin) of a point
        /// </summary>
        // TODO: In v4.0 codebase, turn this into an extension method
        public static float Magnitude(Vector2<float> p)
        {
            return (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        public override void BeforeRender()
        {
            Vector2<float> vec = new Vector2<float>(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);
            float mag = Magnitude(vec);

            if (EndPoint.X == StartPoint.X)
            {
                this.dtdx = 0;
            }
            else
            {
                this.dtdx = vec.X / (mag * mag);
            }

            if (EndPoint.Y == StartPoint.Y)
            {
                this.dtdy = 0;
            }
            else
            {
                this.dtdy = vec.Y / (mag * mag);
            }

            base.BeforeRender();
        }

        protected internal LinearBase(bool alphaOnly) : base(alphaOnly)
        {
        }
    }

    public abstract class LinearStraight : LinearBase
    {
        public override float ComputeUnboundedLerp(int x, int y)
        {
            float dx = x - StartPoint.X;
            float dy = y - StartPoint.Y;

            float lerp = (dx * this.dtdx) + (dy * this.dtdy);

            return lerp;
        }

        protected internal LinearStraight(bool alphaOnly) : base(alphaOnly)
        {
        }
    }

    public sealed class LinearReflected : LinearStraight
    {        // TODO: In v4.0 codebase, turn this into an extension method
        public static float Clamp(float x, float min, float max)
        {
            if (x < min)
            {
                return min;
            }
            else if (x > max)
            {
                return max;
            }
            else
            {
                return x;
            }
        }

        public override float BoundLerp(float t)
        {
            return Clamp(Math.Abs(t), 0, 1);
        }

        public LinearReflected(bool alphaOnly) : base(alphaOnly) { }
    }
    */

    [Serializable]
    public class LinearGradient : Gradient
    {
        public LinearGradient(LinearGradient input) : base(input) { }

        public LinearGradient(LinearGradientBrush input) : base(input) { }

        public LinearGradient(params GradientStop[] input) : base(input) { }

        public override Matrix<Color> Render(Int32Size size)
        {
            var result = new Matrix<Color>(size.Height.UInt32(), size.Width.UInt32());

            if (Count == 0)
                return null;

            GradientBand a = null, b = null;

            var xyStart = 0;
            var xyEnd = size.Height;
            var xyIncrement = 1;
            var xyLimit = size.Height;

            switch (Direction)
            {
                case CardinalDirection.N:
                case CardinalDirection.S:
                    xyStart = 0;
                    xyEnd = size.Height;
                    xyIncrement = 1;
                    xyLimit = size.Height;
                    break;

                case CardinalDirection.E:
                case CardinalDirection.W:
                    xyStart = 0;
                    xyEnd = size.Width;
                    xyIncrement = 1;
                    xyLimit = size.Width;
                    break;
            }

            void fill(CardinalDirection d, Matrix<Color> m, uint xy, Color color)
            {
                uint xf(uint input) => (input + Math.Cos(Angle.FromDegreeToRadian())).Round().Coerce(UInt32.MaxValue).UInt32().Coerce(m.Rows - 1);
                uint yf(uint input) => (input + Math.Sin(Angle.FromDegreeToRadian())).Round().Coerce(UInt32.MaxValue).UInt32().Coerce(m.Columns - 1);

                switch (d)
                {
                    case CardinalDirection.N:
                        for (uint x = 0; x < m.Columns; x++)
                        {
                            var x6 = xf(x);
                            var y6 = yf((m.Rows - 1) - xy);

                            if (x6 > 0 && y6 > 0 && x6 < m.Columns && y6 < m.Rows)
                                m.SetValue(y6, x6, color);
                        }
                        break;
                    case CardinalDirection.S:
                        for (uint x = 0; x < m.Columns; x++)
                        {
                            var x6 = xf(x);
                            var y6 = yf(xy);

                            if (x6 > 0 && y6 > 0 && x6 < m.Columns && y6 < m.Rows)
                                m.SetValue(y6, x6, color);
                        }
                        break;
                    case CardinalDirection.W:
                        for (uint x = 0; x < m.Rows; x++)
                        {
                            var x6 = xf((m.Columns - 1) - xy);
                            var y6 = yf(x);

                            if (x6 > 0 && y6 > 0 && x6 < m.Columns && y6 < m.Rows)
                                m.SetValue(y6, x6, color);
                        }
                        break;
                    case CardinalDirection.E:
                        for (uint x = 0; x < m.Rows; x++)
                        {
                            var x6 = xf(xy);
                            var y6 = yf(x);

                            if (x6 > 0 && y6 > 0 && x6 < m.Columns && y6 < m.Rows)
                                m.SetValue(y6, x6, color);
                        }
                        break;
                }
            }

            var i = 0;
            var j = 0;

            for (var xy = xyStart; xy < xyEnd; xy += xyIncrement)
            {
                a = Stops[i];
                if (Count == 1)
                {
                    fill(Direction, result, xy.UInt32(), a.Color);
                    continue;
                }

                if (Count <= i + 1)
                    continue;

                b = Stops[i + 1];
                var offsetMinus = b.Offset - a.Offset;

                var a0 = j.Double() / (xyLimit.Double() * offsetMinus);
                var a1 = a0 * 255.0;
                var a2 = b.Color.Value.A((a1.Round().Coerce(255)).Byte());

                var color = a.Color.Value.Blend(a2);
                fill(Direction, result, xy.UInt32(), color);

                j++;

                var yOffset = xy.Double() / xyLimit.Double();
                if (yOffset >= b.Offset)
                {
                    i++;
                    j = 0;
                }
            }
            return result;
        }
    }

    [Serializable]
    public abstract class PathGradient : Gradient
    {
        public enum Shapes
        {
            Circle,
            Ellipse,
            Diamond,
            Square,
            Rectangle
        };

        [field: NonSerialized]
        List<double> aVals;          // Major axis value of the vignette shape.
        [field: NonSerialized]
        List<double> bVals;          // Minor axis value of the vignette shape.

        /// --------------------------------------------------------------------------

        [field: NonSerialized]
        List<double> aValsMidPoints; // Major axis of mid-figures of the vignette shape.
        [field: NonSerialized]
        List<double> bValsMidPoints; // Minor axis of mid-figures of the vignette shape.

        /// --------------------------------------------------------------------------

        [field: NonSerialized]
        List<double> weight1;        // Weights for the original image.
        [field: NonSerialized]
        List<double> weight2;        // Weights for the border colour.

        /// --------------------------------------------------------------------------

        [field: NonSerialized]
        double geometryFactor;       // See note below, in the constructor.

        public PathGradient(LinearGradient input) : base(input)
        {
            aVals = new List<double>();
            bVals = new List<double>();

            aValsMidPoints = new List<double>();
            bValsMidPoints = new List<double>();

            weight1 = new List<double>();
            weight2 = new List<double>();

            geometryFactor = 0.5 / 100.0;
        }

        private void SetupParameters(Shapes shape, Int32Size size)
        {
            aVals.Clear();
            bVals.Clear();

            aValsMidPoints.Clear();
            bValsMidPoints.Clear();

            weight1.Clear();
            weight2.Clear();

            double a0, b0, aLast, bLast, aEll, bEll;
            double stepSize = Bands * 1.0 / Steps;
            double bandPixelsBy2 = 0.5 * Bands;
            double arguFactor = Math.PI / Bands;

            double vignetteWidth = size.Width * Coverage / 100.0;
            double vignetteHeight = size.Height * Coverage / 100.0;

            double vwb2 = vignetteWidth * 0.5;
            double vhb2 = vignetteHeight * 0.5;

            a0 = vwb2 - bandPixelsBy2;
            b0 = vhb2 - bandPixelsBy2;

            // For a circle or square, both 'major' and 'minor' axes are identical
            if (shape == Shapes.Circle || shape == Shapes.Square)
            {
                a0 = Math.Min(a0, b0);
                b0 = a0;
            }

            if (shape == Shapes.Circle || shape == Shapes.Ellipse || shape == Shapes.Rectangle || shape == Shapes.Square)
            {
                for (int i = 0; i <= Steps; ++i)
                {
                    aEll = a0 + stepSize * i;
                    bEll = b0 + stepSize * i;
                    aVals.Add(aEll);
                    bVals.Add(bEll);
                }
                for (int i = 0; i < Steps; ++i)
                {
                    aEll = a0 + stepSize * (i + 0.5);
                    bEll = b0 + stepSize * (i + 0.5);
                    aValsMidPoints.Add(aEll);
                    bValsMidPoints.Add(bEll);
                }
            }
            else// if (Shape == VignetteShape.Diamond)
            {
                aLast = vwb2 + bandPixelsBy2;
                bLast = b0 * aLast / a0;
                double stepXdiamond = (aLast - a0) / Steps;
                double stepYdiamond = (bLast - b0) / Steps;

                for (int i = 0; i <= Steps; ++i)
                {
                    aEll = a0 + stepXdiamond * i;
                    bEll = b0 + stepYdiamond * i;
                    aVals.Add(aEll);
                    bVals.Add(bEll);
                }
                for (int i = 0; i <= Steps; ++i)
                {
                    aEll = a0 + stepXdiamond * (i + 0.5);
                    bEll = b0 + stepYdiamond * (i + 0.5);
                    aValsMidPoints.Add(aEll);
                    bValsMidPoints.Add(bEll);
                }
            }

            // The weight functions given below form the crux of the code. It was a struggle after which 
            // I got these weighting functions. 
            // Initially, I tried linear interpolation, and the effect was not so pleasing. The 
            // linear interpolation function is C0-continuous at the boundary, and therefore shows 
            // a distinct border.
            // Later, upon searching, I found a paper by Burt and Adelson on Mosaics. Though I did 
            // not use the formulas given there, one of the initial figures in that paper set me thinking
            // on using the cosine function. This function is C1-continuous at the boundary, and therefore
            // the effect is pleasing on the eye. Yields quite a nice blending effect. The cosine 
            // functions are incorporated into the wei1 and wei2 definitions below.
            //
            // Reference: Burt and Adelson [Peter J Burt and Edward H Adelson, A Multiresolution Spline
            //  With Application to Image Mosaics, ACM Transactions on Graphics, Vol 2. No. 4,
            //  October 1983, Pages 217-236].
            double wei1, wei2, arg, argCosVal;
            for (int i = 0; i < Steps; ++i)
            {
                arg = arguFactor * (aValsMidPoints[i] - a0);
                argCosVal = Math.Cos(arg);
                wei1 = 0.5 * (1.0 + argCosVal);
                wei2 = 0.5 * (1.0 - argCosVal);
                weight1.Add(wei1);
                weight2.Add(wei2);
            }
        }

        /// <summary>
        /// Method to apply the Circular, Elliptical or Diamond-shaped vignette on an image.
        /// </summary>
        protected Matrix<Color> CircleEllipseDiamond(Shapes Shape, Int32Size size)
        {
            var result = new Matrix<Color>(size.Height.UInt32(), size.Width.UInt32());

            var rOriginal = new List<byte>();
            var gOriginal = new List<byte>();
            var bOriginal = new List<byte>();

            var rModified = new List<byte>();
            var gModified = new List<byte>();
            var bModified = new List<byte>();

            for (var y = 0; y < size.Height; y++)
            {
                for (var x = 0; x < size.Width; x++)
                {
                    rOriginal.Add(Stops[0].Color.Value.R);
                    gOriginal.Add(Stops[0].Color.Value.G);
                    bOriginal.Add(Stops[0].Color.Value.B);

                    rModified.Add(Stops[0].Color.Value.R);
                    gModified.Add(Stops[0].Color.Value.G);
                    bModified.Add(Stops[0].Color.Value.B);
                }
            }
            SetupParameters(Shape, size);

            int k, el, w1, w2;
            byte r, g, b;
            double wb2 = size.Width * 0.5 + Center.X * size.Width * geometryFactor;
            double hb2 = size.Height * 0.5 + Center.Y * size.Height * geometryFactor;
            double thetaRadians = Angle * Math.PI / 180.0;
            double cos = Math.Cos(thetaRadians);
            double sin = Math.Sin(thetaRadians);
            double xprime, yprime, potential1, potential2, potential;
            double factor1, factor2, factor3, factor4;

            byte redBorder = Stops[1].Color.Value.R;
            byte greenBorder = Stops[1].Color.Value.G;
            byte blueBorder = Stops[1].Color.Value.B;

            // Loop over the number of pixels
            for (el = 0; el < size.Height; ++el)
            {
                w2 = size.Width * el;
                for (k = 0; k < size.Width; ++k)
                {
                    // This is the usual rotation formula, along with translation.
                    // I could have perhaps used the Transform feature of WPF.
                    xprime = (k - wb2) * cos + (el - hb2) * sin;
                    yprime = -(k - wb2) * sin + (el - hb2) * cos;

                    factor1 = 1.0 * Math.Abs(xprime) / aVals[0];
                    factor2 = 1.0 * Math.Abs(yprime) / bVals[0];
                    factor3 = 1.0 * Math.Abs(xprime) / aVals[Steps];
                    factor4 = 1.0 * Math.Abs(yprime) / bVals[Steps];

                    if (Shape == Shapes.Circle || Shape == Shapes.Ellipse)
                    {
                        // Equations for the circle / ellipse. 
                        // "Potentials" are analogous to distances from the inner and outer boundaries
                        // of the two ellipses.
                        potential1 = factor1 * factor1 + factor2 * factor2 - 1.0;
                        potential2 = factor3 * factor3 + factor4 * factor4 - 1.0;
                    }
                    else //if (Shape == VignetteShape.Diamond)
                    {
                        // Equations for the diamond. 
                        potential1 = factor1 + factor2 - 1.0;
                        potential2 = factor3 + factor4 - 1.0;
                    }
                    w1 = w2 + k;

                    if (potential1 <= 0.0)
                    {
                        // Point is within the inner circle / ellipse / diamond
                        r = rOriginal[w1];
                        g = gOriginal[w1];
                        b = bOriginal[w1];
                    }
                    else if (potential2 >= 0.0)
                    {
                        // Point is outside the outer circle / ellipse / diamond
                        r = redBorder;
                        g = greenBorder;
                        b = blueBorder;
                    }
                    else
                    {
                        // Point is in between the outermost and innermost circles / ellipses / diamonds
                        int j, j1;

                        for (j = 1; j < Steps; ++j)
                        {
                            factor1 = Math.Abs(xprime) / aVals[j];
                            factor2 = Math.Abs(yprime) / bVals[j];

                            if (Shape == Shapes.Circle ||
                                Shape == Shapes.Ellipse)
                            {
                                potential = factor1 * factor1 + factor2 * factor2 - 1.0;
                            }
                            else // if (Shape == VignetteShape.Diamond)
                            {
                                potential = factor1 + factor2 - 1.0;
                            }
                            if (potential < 0.0) break;
                        }
                        j1 = j - 1;
                        // The formulas where the weights are applied to the image, and border.
                        var q0 = weight1[j1];
                        var q1 = weight2[j1];

                        r = (byte)(rOriginal[w1] * q0 + redBorder * q1);
                        g = (byte)(gOriginal[w1] * q0 + greenBorder * q1);
                        b = (byte)(bOriginal[w1] * q0 + blueBorder * q1);
                    }
                    rModified[w1] = r;
                    gModified[w1] = g;
                    bModified[w1] = b;

                    result.SetValue(el.UInt32(), k.UInt32(), Color.FromArgb(255, r, g, b));
                }
            }

            return result;
        }

        /// <summary>
        /// Method to apply the Rectangular or Square-shaped vignette on an image.
        /// </summary>
        protected Matrix<Color> RectangleSquare(Shapes Shape, Vector2<int> center, Int32Size size)
        {
            var result = new Matrix<Color>(0, 0);

            var pixRedOrig = new List<byte>();
            var pixGreenOrig = new List<byte>();
            var pixBlueOrig = new List<byte>();

            var pixRedModified = new List<byte>();
            var pixGreenModified = new List<byte>();
            var pixBlueModified = new List<byte>();

            Rect rect1 = new Rect(), rect2 = new Rect(), rect3 = new Rect();
            Point point = new Point();
            int k, el, w1, w2;
            byte r, g, b;

            double wb2 = size.Width * 0.5 + center.X * size.Width * geometryFactor;
            double hb2 = size.Height * 0.5 + center.Y * size.Height * geometryFactor;
            double thetaRadians = Angle * Math.PI / 180.0;
            double cos = Math.Cos(thetaRadians);
            double sin = Math.Sin(thetaRadians);
            double xprime, yprime, potential;

            byte redBorder = Colors.Black.R;
            byte greenBorder = Colors.Black.G;
            byte blueBorder = Colors.Black.B;

            rect1.X = 0.0;
            rect1.Y = 0.0;
            rect1.Width = aVals[0];
            rect1.Height = bVals[0];
            rect2.X = 0.0;
            rect2.Y = 0.0;
            rect2.Width = aVals[Steps];
            rect2.Height = bVals[Steps];

            for (el = 0; el < size.Height; ++el)
            {
                w2 = size.Width * el;
                for (k = 0; k < size.Width; ++k)
                {
                    // The usual rotation-translation formula
                    xprime = (k - wb2) * cos + (el - hb2) * sin;
                    yprime = -(k - wb2) * sin + (el - hb2) * cos;

                    potential = 0.0;
                    point.X = Math.Abs(xprime);
                    point.Y = Math.Abs(yprime);

                    // For a rectangle, we can use the Rect.Contains(Point) method to determine
                    //  whether the point is in the rectangle or not
                    if (rect1.Contains(point))
                        potential = -2.0; // Arbitrary negative number N1

                    if (!rect2.Contains(point))
                        potential = 2.0; // Arbitrary positive number = - N1

                    w1 = w2 + k;

                    if (potential < -1.0) // Arbitrary negative number, greater than N1
                    {
                        // Point is within the inner square / rectangle,
                        r = pixRedOrig[w1];
                        g = pixGreenOrig[w1];
                        b = pixBlueOrig[w1];
                    }
                    else if (potential > 1.0) // Arbitrary positive number lesser than - N1
                    {
                        // Point is outside the outer square / rectangle
                        r = redBorder;
                        g = greenBorder;
                        b = blueBorder;
                    }
                    else
                    {
                        // Point is in between outermost and innermost squares / rectangles
                        int j, j1;

                        for (j = 1; j < Steps; ++j)
                        {
                            rect3.X = 0.0;
                            rect3.Y = 0.0;
                            rect3.Width = aVals[j];
                            rect3.Height = bVals[j];

                            if (rect3.Contains(point))
                                break;
                        }
                        j1 = j - 1;
                        r = (byte)(pixRedOrig[w1] * weight1[j1] + redBorder * weight2[j1]);
                        g = (byte)(pixGreenOrig[w1] * weight1[j1] + greenBorder * weight2[j1]);
                        b = (byte)(pixBlueOrig[w1] * weight1[j1] + blueBorder * weight2[j1]);
                    }
                    pixRedModified[w1] = r;
                    pixGreenModified[w1] = g;
                    pixBlueModified[w1] = b;
                }
            }

            return result;
        }
    }

    [Serializable]
    public class RadialGradient : PathGradient
    {
        public RadialGradient(LinearGradient input) : base(input) { }

        public override Matrix<Color> Render(Int32Size size)
            => CircleEllipseDiamond(Shapes.Ellipse, size);
    }

    /// --------------------------------------------------------------------------

    /*
    public class AngleGradient : PathGradient
    {
        public AngleGradient(AngleGradientBrush angleGradientBrush)
        {
            angleGradientBrush.GradientStops.ForEach(i => Stops.Add(i));
        }
    }

    public class ReflectedGradient : PathGradient
    {
        public ReflectedGradient(ReflectedGradientBrush reflectedGradientBrush)
        {
            reflectedGradientBrush.GradientStops.ForEach(i => Stops.Add(i));
        }
    }
    */

    [Serializable]
    public class DiamondGradient : PathGradient
    {
        public DiamondGradient(LinearGradient input) : base(input) { }

        public override Matrix<Color> Render(Int32Size size)
            => CircleEllipseDiamond(Shapes.Diamond, size);
    }

    /// --------------------------------------------------------------------------

    [Serializable]
    public abstract class Gradient : Base
    {
        [field: NonSerialized]
        public double Angle;

        public int Count => Stops.Count;

        [field: NonSerialized]
        public int Bands;

        [field: NonSerialized]
        public int Coverage;

        [field: NonSerialized]
        public int Steps;

        [field: NonSerialized]
        public Vector2<int> Center;

        [field: NonSerialized]
        public CardinalDirection Direction;

        ObservableCollection<GradientBand> stops = new ObservableCollection<GradientBand>();
        public ObservableCollection<GradientBand> Stops
        {
            get => stops;
            set => this.Change(ref stops, value);
        }

        public Gradient(LinearGradient input)
        {
            input.Stops.ForEach(i => Stops.Add(i));
        }

        public Gradient(LinearGradientBrush input) 
        {
            input.GradientStops.ForEach(i => Stops.Add(new GradientBand(i)));
        }

        public Gradient(params GradientStop[] input)
        {
            input.ForEach(i => Stops.Add(new GradientBand(i)));
        }

        public abstract Matrix<Color> Render(Int32Size size);

        public static Gradient New(GradientType type, LinearGradient input)
        {
            switch (type)
            {
                case GradientType.Angle:
                    return null;
                case GradientType.Diamond:
                    return new DiamondGradient(input);
                case GradientType.Linear:
                    return new LinearGradient(input);
                case GradientType.Radial:
                    return new RadialGradient(input);
                case GradientType.Reflected:
                    return null;
            }

            /*
            if (input is AngleGradientBrush)
                return new AngleGradient((AngleGradientBrush)input);

            if (input is DiamondGradientBrush)
                return new DiamondGradient((DiamondGradientBrush)input);

            if (input is ReflectedGradientBrush)
                return new ReflectedGradient((ReflectedGradientBrush)input);
            */

            return null;
        }
    }
}