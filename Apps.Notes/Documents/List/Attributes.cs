using Imagin.Common;
using System;

namespace Notes
{
    [Flags]
    [Serializable]
    public enum Attributes
    {
        [Hidden]
        None = 0,
        Bullet = 1,
        Check = 2,
        Image = 4
    }
}