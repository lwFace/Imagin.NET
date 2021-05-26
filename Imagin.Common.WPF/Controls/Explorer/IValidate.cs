using Imagin.Common.Storage;
using System;

namespace Imagin.Common.Controls
{
    public interface IValidate
    {
        bool Validate(ExplorerWindow.Modes mode, string path);
    }

    public class LocalValidateHandler : IValidate
    {
        public virtual bool Validate(ExplorerWindow.Modes mode, string path)
        {
            switch (mode)
            {
                case ExplorerWindow.Modes.Open:
                    return File.Long.Exists(path) ? true : path == Folder.Long.Root ? false : Folder.Long.Exists(path);

                case ExplorerWindow.Modes.OpenFile:
                case ExplorerWindow.Modes.SaveFile:
                    return File.Long.Exists(path);

                case ExplorerWindow.Modes.OpenFolder:
                    return path == Folder.Long.Root ? false : Folder.Long.Exists(path);
            }
            throw new InvalidOperationException();
        }
    }
}