using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Analytics
{
    /// <summary>
    /// Represents a result.
    /// </summary>
    public abstract class Result : Base
    {
        /// <summary>
        /// Abitrary data associated with the result.
        /// </summary>
        public readonly object Data;

        /// <summary>
        /// The message associated with the result.
        /// </summary>
        public abstract string Message { get; }

        public Result() : base() { }

        public Result(object data) : base() => Data = data;

        public static implicit operator bool(Result a) => a is Success;

        public void If(bool i, Action action) => ((bool)this).If(i, action);
    }
}