using System;

namespace Paint
{
    /// <summary>
    /// A filter with multiple logical values.
    /// </summary>
    [Serializable]
    public abstract class VariableFilter : Filter
    {
        public VariableFilter() : base(FilterType.Variable)
        {
        }
    }
}
