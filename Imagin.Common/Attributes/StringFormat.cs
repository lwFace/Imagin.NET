using Imagin.Common.Data;
using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class StringFormatAttribute : Attribute
    {
        public readonly StringFormat StringFormat;

        public readonly char StringFormatDelimiter = ';';

        public StringFormatAttribute(StringFormat stringFormat) : base() => StringFormat = stringFormat;

        public StringFormatAttribute(StringFormat stringFormat, char stringFormatDelimiter) : this(stringFormat) => StringFormatDelimiter = stringFormatDelimiter;
    }
}