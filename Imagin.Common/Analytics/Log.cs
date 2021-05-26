using Imagin.Common.Linq;

namespace Imagin.Common.Diagnostics
{
    public static class Log
    {
        public static ILog Current;

        public class Entry : ObjectBase
        {
            public Types Type { get; private set; }

            public string Message { get; private set; }

            public Entry(Types type, string message)
            {
                Type = type;
                Message = message;
            }
        }

        public enum Types
        {
            Error,
            Message,
            Warning
        }

        public static void Write(string message) => Write(Types.Message, message);

        public static void Write(Types type, object message) => Write(type, null, message);

        public static void Write(Types type, object source, object message)
        {
            var result = message is Error ? $"({message.To<Error>().Exception.GetType().FullName}) {message}" : $"{message}";
            Current.Text.Add(new Entry(type, result));
        }
    }
}