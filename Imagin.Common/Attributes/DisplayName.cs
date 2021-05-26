using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        readonly string displayName;
        public string DisplayName
        {
            get => displayName;
        }

        public DisplayNameAttribute(string DisplayName = "") : base()
        {
            displayName = DisplayName;
        }
    }
}
