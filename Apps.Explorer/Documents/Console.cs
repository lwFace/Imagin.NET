using Imagin.Common;
using System;

namespace Explorer
{
    [Serializable]
    public class ConsoleDocument : ExplorerDocument
    {
        string line;
        public string Line
        {
            get => line;
            set => this.Change(ref line, value);
        }

        string output;
        public string Output
        {
            get => output;
            set => this.Change(ref output, value);
        }

        public ConsoleDocument() : base() { }
    }
}