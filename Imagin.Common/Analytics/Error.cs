using System;

namespace Imagin.Common.Analytics
{
    /// <summary>
    /// Represents an unsuccessful <see cref="Result"/> that may encapsulate an <see cref="Exception"/>.
    /// </summary>
    public class Error : Error<object>
    {
        public Error() : base() { }

        public Error(Exception exception) : base(exception) { }

        public Error(Exception exception, object data) : base(exception, data) { }

        public Error(string message) : base(message) { }

        public Error(string message, object data) : base(message, data) { }
    }
}