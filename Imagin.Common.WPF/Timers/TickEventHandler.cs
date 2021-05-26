using System;

namespace Imagin.Common.Timers
{
    public class TickEventArgs : EventArgs
    {
        public readonly TimeSpan Elapsed;

        public TickEventArgs(TimeSpan elapsed)
        {
            Elapsed = elapsed;
        }
    }

    public delegate void TickEventHandler(BaseTimer sender, TickEventArgs e);
}