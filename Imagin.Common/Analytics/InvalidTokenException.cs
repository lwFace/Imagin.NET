using System;

namespace Imagin.Common.Analytics
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base() { }

        public InvalidTokenException(string Message) : base(Message) { }
    }
}