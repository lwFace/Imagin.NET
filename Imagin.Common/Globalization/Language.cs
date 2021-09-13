
using System;

namespace Imagin.Common.Globalization
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LanguageAttribute : Attribute
    {
        public readonly string Code;

        public LanguageAttribute(string code) : base() => Code = code;
    }
}