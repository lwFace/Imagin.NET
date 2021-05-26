using System;

namespace Imagin.Common.Analytics
{
    /// <summary>
    /// Represents an unsuccessful <see cref="Result"/> that may encapsulate an <see cref="System.Exception"/>.
    /// </summary>
    public class Error<Data> : Result<Data>
    {
        readonly Exception exception;
        public Exception Exception => exception;
        
        public Exception InnerException => exception?.InnerException;

        public override string Message => exception?.Message;

        public override string ToString() => Message ?? base.Data.ToString() ?? string.Empty;

        public Error() : this(string.Empty) { }

        public Error(Exception exception) : this(exception, default(Data))
        {
            this.exception = exception;
        }

        public Error(Exception exception, Data data) : base(data)
        {
            this.exception = exception;
        }

        public Error(string message) : this(message, default(Data)) { }

        public Error(string message, Data data) : this(new Exception(message), data) { }
    }
}