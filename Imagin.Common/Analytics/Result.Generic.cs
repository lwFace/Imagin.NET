namespace Imagin.Common.Analytics
{
    /// <summary>
    /// Represents a result with data.
    /// </summary>
    public abstract class Result<T> : Result
    {
        public new T Data => (T)base.Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        public Result() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="Data"></param>
        public Result(T Data) : base(Data) { }

        /// <summary>
        /// True, if <see cref="Result"/> is <see cref="Success"/>; false, otherwise.
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator bool(Result<T> a) => a is Success<T>;
    }
}