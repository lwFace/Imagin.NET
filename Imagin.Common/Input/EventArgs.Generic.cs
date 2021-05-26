using System;

namespace Imagin.Common.Input
{
    public class EventArgs<T> : EventArgs
    {
        readonly T _value;
        public T Value => _value;

        readonly object _parameter;
        public object Parameter => _parameter;

        public EventArgs(T value, object parameter = null)
        {
            _value = value;
            _parameter = parameter;
        }
    }
}
