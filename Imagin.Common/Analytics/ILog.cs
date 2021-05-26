using Imagin.Common.Collections.Generic;

namespace Imagin.Common.Diagnostics
{
    public interface ILog
    {
        ConcurrentCollection<Log.Entry> Text { get; set; }
    }
}