using System;
using System.Collections.Generic;
using System.Text;

namespace Imagin.Common.Input
{
    public class ResultEventArgs : EventArgs
    {
        readonly object _result;
        public object Result => _result;

        public ResultEventArgs(object result) : base() => _result = result;
    }
}
