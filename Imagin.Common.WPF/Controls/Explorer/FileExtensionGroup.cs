using System.Collections.ObjectModel;

namespace Imagin.Common.Controls
{
    public class FileExtensionGroup : Base
    {
        public const string Wild = "*";

        string fileExtension;
        public string FileExtension
        {
            get => fileExtension;
            private set => this.Change(ref fileExtension, value);
        }

        public FileExtensionGroup(string fileExtension)
        {
            FileExtension = fileExtension;
        }
    }

    public class FileExtensionGroups : ObservableCollection<FileExtensionGroup>
    {
        public void Add(string fileExtension) => Add(new FileExtensionGroup(fileExtension));
    }
}