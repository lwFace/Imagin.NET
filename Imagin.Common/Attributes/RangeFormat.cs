using Imagin.Common.Data;
using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RangeFormatAttribute : Attribute
    {
        public readonly RangeFormat Format;

        public RangeFormatAttribute(RangeFormat format) : base()
        {
            Format = format;
        }
    }
}