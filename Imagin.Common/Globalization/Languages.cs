using System;

namespace Imagin.Common.Globalization
{
    [Serializable]
    public enum Languages
    {
        [Language("en")]
        English,
        [Language("es-ES")]
        Spanish,
        [Language("fr-FR")]
        French,
        [Language("it-IT")]
        Italian,
        [Language("ja-JP")]
        Japanese
    }
}