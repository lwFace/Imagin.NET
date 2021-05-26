using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AlternativeNameAttribute : Attribute
    {
        public readonly string AlternativeName;

        public AlternativeNameAttribute(string alternativeName = "") : base() => AlternativeName = alternativeName;
    }
}