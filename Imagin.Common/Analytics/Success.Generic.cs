using Imagin.Common.Linq;

namespace Imagin.Common.Analytics
{
    /// <summary>
    /// Represents a successful <see cref="Result"/>.
    /// </summary>
    public class Success<Data> : Result<Data>
    {
        readonly string message;
        public override string Message => message;

        public Success() : this(string.Empty, default(Data)) { }

        public Success(string message) : this(message, default(Data)) { }

        public Success(Data data) : this(string.Empty, data) { }

        public Success(string message, Data data) : base(data) => this.message = message;

        public override string ToString()
        {
            if (message.NullOrEmpty())
                return GetType().ToString();

            return message;
        }
    }
}