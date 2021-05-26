using System;

namespace Imagin.Common.Analytics
{
    public class InvalidResultException : Exception
    {
        public InvalidResultException() : base() { }

        public InvalidResultException(string message) : base(message) { }
    }
}