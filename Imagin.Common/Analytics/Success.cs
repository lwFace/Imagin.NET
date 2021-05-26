namespace Imagin.Common.Analytics
{
    /// <summary>
    /// Represents a successful <see cref="Result"/>.
    /// </summary>
    public class Success : Success<object>
    {
        public Success() : base(string.Empty, default(object)) { }

        public Success(string message) : base(message, default(object)) { }

        public Success(object data) : base(string.Empty, data) { }

        public Success(string message, object data) : base(data) { }
    }
}