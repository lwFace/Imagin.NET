using Imagin.Common.Input;
using System;

namespace Imagin.Common.Controls
{
    public interface IExplorer
    {
        event EventHandler<EventArgs<string>> PathChanged;

        string Path { get; set; }
    }
}