using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FeaturedAttribute : Attribute
    {
        public readonly bool Featured;

        public readonly int Index;

        public FeaturedAttribute() : this(true) { }

        public FeaturedAttribute(bool featured) : base() => Featured = featured;

        public FeaturedAttribute(int index) : this(true)
        {
            Index = index;
        }
    }
}