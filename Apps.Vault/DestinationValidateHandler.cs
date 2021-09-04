using Imagin.Common.Controls;
using System;
using System.IO;

namespace Vault
{
    /// <summary>
    /// A destination path is valid if:
    /// 1a) it resides on the main drive, 
    /// 1b) starts with the current user's folder path, but doesn't equal it (for security reasons), and 
    /// 1c) exists 
    /// or 
    /// 2a) doesn't reside on the main drive and 
    /// 2b) exists.
    /// </summary>
    public class DestinationValidateHandler : LocalValidateHandler
    {
        public override bool Validate(ExplorerWindow.Modes mode, string path)
        {
            var result = base.Validate(mode, path);
            var a = Environment.GetFolderPath(Environment.SpecialFolder.System);
            if (path.StartsWith(Path.GetPathRoot(a)))
            {
                var b = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                return path.StartsWith(b) && !path.Equals(b) && result;
            }
            return result;
        }
    }
}