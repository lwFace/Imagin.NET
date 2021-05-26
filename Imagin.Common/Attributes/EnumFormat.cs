using System;
using Imagin.Common.Data;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnumFormatAttribute : Attribute
    {
        public readonly EnumFormat EnumFormat;

        public EnumFormatAttribute(EnumFormat enumFormat) : base() => EnumFormat = enumFormat;
    }
}