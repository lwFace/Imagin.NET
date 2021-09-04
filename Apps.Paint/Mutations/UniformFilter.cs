using System;

namespace Paint
{
    /// <summary>
    /// A filter with only one logical value.
    /// </summary>
    [Serializable]
    public abstract class UniformFilter : Filter
    {
        public UniformFilter() : base(FilterType.Uniform)
        {
        }
    }
}
