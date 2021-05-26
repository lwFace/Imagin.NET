using Imagin.Common.Data;
using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StringFormatAttribute : Attribute
    {
        public readonly StringFormat StringFormat;

        public StringFormatAttribute(StringFormat stringFormat) : base() => StringFormat = stringFormat;
    }
}