using System;

namespace Imagin.Common
{
    public class Handle
    {
        bool value;

        public Handle(bool input) => value = input;

        public static implicit operator Handle(bool input) => new Handle(input);

        public static implicit operator bool(Handle input) => input.value;

        public void Invoke(Action action)
        {
            value = true;
            action();
            value = false;
        }

        public void InvokeIfFalse(Action action)
        {
            if (!value)
            {
                value = true;
                action();
                value = false;
            }
        }
    }
}