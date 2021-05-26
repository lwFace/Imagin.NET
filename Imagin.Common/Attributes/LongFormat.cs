using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LongFormatAttribute : Attribute
    {
        public readonly object LongFormat;

        public LongFormatAttribute(object longFormat) : base() => LongFormat = longFormat;
    }
}