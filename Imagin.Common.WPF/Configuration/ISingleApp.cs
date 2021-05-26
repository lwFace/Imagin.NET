using System.Collections.Generic;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISingleApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        bool OnReopened(IList<string> Arguments);
    }
}