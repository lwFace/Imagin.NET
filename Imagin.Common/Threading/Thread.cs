using System;
using System.Threading;
using System.Threading.Tasks;
using Imagin.Common.Linq;

namespace Imagin.Common
{
    public class Thread
    {
        public static async Task Sleep(TimeSpan timeSpan)
            => await Task.Run(() => System.Threading.Thread.Sleep(timeSpan.TotalMilliseconds.Int32()));
    }
}