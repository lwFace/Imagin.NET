using System;
using Imagin.Common.Data;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DateTimeFormatAttribute : Attribute
    {
        public readonly DateTimeFormat DateFormat;

        public DateTimeFormatAttribute(DateTimeFormat dateFormat) : base() => DateFormat = dateFormat;
    }
}