using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Collections.Concurrent;
using System;

namespace Vault
{
    public class Log : ConcurrentCollection<Log.Entry>
    {
        public class Entry : Base
        {
            public Types Type { get; private set; }

            public string Path { get; private set; }

            public DateTime Time { get; private set; } = DateTime.Now;

            public Entry(Types type, string path)
            {
                Type = type;
                Path = path;
            }
        }

        public enum Types
        {
            Error,
            Enable,
            Disable,
            Synchronize,
            Watch,
            Create,
            Delete,
            Skip
        }

        public readonly CopyTask Task;

        public Log(CopyTask task)
        {
            Task = task;
        }

        public void Write(Types type, string path)
        {
            if (Task.Logs)
                Insert(0, new Entry(type, $"{path}"));
        }

        public void Write(Error e)
        {
            //{(e.Exception?.GetType().Name.Replace("Exception", string.Empty).SplitCamel().Capitalize() ?? typeof(InvalidOperationException).FullName)}: 
            if (Task.Logs)
                Insert(0, new Entry(Types.Error, $"{(e.Exception?.Message ?? "Unspecified error occurred.")}"));
        }
    }
}