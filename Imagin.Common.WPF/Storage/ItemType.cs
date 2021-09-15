using System;

namespace Imagin.Common.Storage
{
    [Flags]
    [Serializable]
    public enum ItemType
    {
        File = 1,
        Folder = 2,
        Shortcut = 4,
        Drive = 8,
    }
}
