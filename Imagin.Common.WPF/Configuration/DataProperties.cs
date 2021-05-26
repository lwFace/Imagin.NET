using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;

namespace Imagin.Common.Configuration
{
    public class DataProperties
    {
        public readonly DataFolders Folder;

        public readonly string FileName;

        public readonly string FileExtension;

        public string FilePath => $@"{FolderPath}\{FileName}.{FileExtension}";

        public string FolderPath
        {
            get
            {
                var result = GetFolderPath(Folder);
                switch (Folder)
                {
                    case DataFolders.Documents:
                        return $@"{result}\{Get.Where<SingleApplication>().Name}";

                    case DataFolders.ExecutionPath:
                        return result;
                }
                throw new NotSupportedException();
            }
        }

        public readonly Type Type;

        public DataProperties(DataFolders folder, string fileName, string fileExtension, Type type)
        {
            Folder = folder;
            FileName = fileName;
            FileExtension = fileExtension;
            Type = type;
        }

        public static string GetFolderPath(DataFolders folder)
        {
            switch (folder)
            {
                case DataFolders.Documents:
                    var i = Get.Where<SingleApplication>();
                    return $@"{Environment.SpecialFolder.MyDocuments.Path()}\{i.Publisher}";

                case DataFolders.ExecutionPath:
                    return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            throw new NotSupportedException();
        }
    }

    public class DataProperties<T> : DataProperties where T : Data
    {
        public DataProperties(DataFolders folder, string fileName, string fileExtension) : base(folder, fileName, fileExtension, typeof(T)) { }
    }
}