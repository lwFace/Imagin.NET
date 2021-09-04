using Imagin.Common;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Configuration;
using System;

namespace Rename
{
    [Serializable]
    public class Options : Data
    {
        string folderPath;
        [Hidden]
        public string FolderPath
        {
            get => folderPath;
            set => this.Change(ref folderPath, value);
        }

        FileExtensionFormats fileExtensionFormat = FileExtensionFormats.Default;
        [Hidden]
        public FileExtensionFormats FileExtensionFormat
        {
            get => fileExtensionFormat;
            set => this.Change(ref fileExtensionFormat, value);
        }

        string fileNameFormat = "{0}";
        [Hidden]
        public string FileNameFormat
        {
            get => fileNameFormat;
            set => this.Change(ref fileNameFormat, value);
        }

        int fileNameIndex = 0;
        [Hidden]
        public int FileNameIndex
        {
            get => fileNameIndex;
            set => this.Change(ref fileNameIndex, value);
        }

        int fileNameIncrement = 1;
        [Hidden]
        public int FileNameIncrement
        {
            get => fileNameIncrement;
            set => this.Change(ref fileNameIncrement, value);
        }

        FileNameOptions fileNameOption = FileNameOptions.Increment;
        [Hidden]
        public FileNameOptions FileNameOption
        {
            get => fileNameOption;
            set
            {
                this.Change(ref fileNameOption, value);
                this.Changed(() => FileNameOptionDescription);
            }
        }

        [Hidden]
        public string FileNameOptionDescription
        {
            get
            {
                switch (target)
                {
                    case Targets.FileExtension:
                        return "Change the file extension to this";
                    case Targets.FileName:
                        switch (fileNameOption)
                        {
                            case FileNameOptions.Increment:
                                return "Increment the name of each file by a number";
                            case FileNameOptions.Replace:
                                return "Replace text in each file name with something else";
                        }
                        break;
                }
                throw new NotSupportedException();
            }
        }
        
        bool topDirectoryOnly = true;
        [Hidden]
        public bool TopDirectoryOnly
        {
            get => topDirectoryOnly;
            set => this.Change(ref topDirectoryOnly, value);
        }

        string fileNameReplace = string.Empty;
        [Hidden]
        public string FileNameReplace
        {
            get => fileNameReplace;
            set => this.Change(ref fileNameReplace, value);
        }

        string fileNameReplaceWith = string.Empty;
        [Hidden]
        public string FileNameReplaceWith
        {
            get => fileNameReplaceWith;
            set => this.Change(ref fileNameReplaceWith, value);
        }

        History history = new History();
        [Hidden]
        public History History
        {
            get => history;
            set => this.Change(ref history, value);
        }

        string newFileExtension = string.Empty;
        [Hidden]
        public string NewFileExtension
        {
            get => newFileExtension;
            set => this.Change(ref newFileExtension, value);
        }

        Targets target = Targets.FileName;
        [Hidden]
        public Targets Target
        {
            get => target;
            set 
            {
                this.Change(ref target, value);
                this.Changed(() => FileNameOptionDescription);
            }
        }

        string targetFileExtensions = string.Empty;
        [Hidden]
        public string TargetFileExtensions
        {
            get => targetFileExtensions;
            set => this.Change(ref targetFileExtensions, value);
        }

        bool targetFileExtensionsCase = false;
        [Hidden]
        public bool TargetFileExtensionsCase
        {
            get => targetFileExtensionsCase;
            set => this.Change(ref targetFileExtensionsCase, value);
        }

        FileNameTargets targetFileNames = FileNameTargets.AnyCharacters;
        [Hidden]
        public FileNameTargets TargetFileNames
        {
            get => targetFileNames;
            set => this.Change(ref targetFileNames, value);
        }

        bool startAtForAllFileExtensions = true;
        [Hidden]
        public bool StartAtForAllFileExtensions
        {
            get => startAtForAllFileExtensions;
            set => this.Change(ref startAtForAllFileExtensions, value);
        }

        bool warn = true;
        [Category("General")]
        [DisplayName("Warn before changing anything")]
        public bool Warn
        {
            get => warn;
            set => this.Change(ref warn, value);
        }
        
        double windowHeight = 360;
        [Hidden]
        public double WindowHeight
        {
            get => windowHeight;
            set => this.Change(ref windowHeight, value);
        }

        double windowWidth = 720;
        [Hidden]
        public double WindowWidth
        {
            get => windowWidth;
            set => this.Change(ref windowWidth, value);
        }
    }
}