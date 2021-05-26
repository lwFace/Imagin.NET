using System;

namespace Imagin.Common.Input
{
    public delegate void CheckedEventHandler(object sender, CheckedEventArgs e);

    public class CheckedEventArgs : EventArgs
    {
        readonly bool? _state = null;
        public bool? State => _state;

        public CheckedEventArgs(bool? state) : base() => _state = state;
    }
}
