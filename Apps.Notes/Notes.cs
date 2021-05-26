using Imagin.Common;
using System;

namespace Notes
{
    [Flags]
    [Serializable]
    public enum Notes
    {
        [Hidden]
        None = 0,
        List = 1,
        Text = 2,
        [Hidden]
        All = List | Text
    }
}