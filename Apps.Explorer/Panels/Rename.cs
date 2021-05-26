using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Explorer
{
    public enum Case
    {
        Default,
        Lower,
        Upper
    }

    public enum Targets
    {
        FileExtension = 0,
        FileName = 1
    }

    public enum FileNameTargets
    {
        AnyCharacters,
        NoNumbers,
        NoLetters,
        Numerical,
        Alphanumerical
    }

    public enum FileNameOptions
    {
        Increment,
        Replace
    }

    public class RenamePanel : Panel
    {
        #region Properties

        bool renaming = false;

        Case fileExtensionCase = Case.Default;
        public Case FileExtensionCase
        {
            get => fileExtensionCase;
            set => this.Change(ref fileExtensionCase, value);
        }

        string fileNameFormat = "{0}";
        public string FileNameFormat
        {
            get => fileNameFormat;
            set => this.Change(ref fileNameFormat, value);
        }
        
        int fileNameIndex = 0;
        public int FileNameIndex
        {
            get => fileNameIndex;
            set => this.Change(ref fileNameIndex, value);
        }

        FileNameOptions fileNameOption = FileNameOptions.Increment;
        public FileNameOptions FileNameOption
        {
            get => fileNameOption;
            set => this.Change(ref fileNameOption, value);
        }
        
        string fileNameReplace = string.Empty;
        public string FileNameReplace
        {
            get => fileNameReplace;
            set => this.Change(ref fileNameReplace, value);
        }

        string fileNameReplaceWith = string.Empty;
        public string FileNameReplaceWith
        {
            get => fileNameReplaceWith;
            set => this.Change(ref fileNameReplaceWith, value);
        }

        string newFileExtension = string.Empty;
        public string NewFileExtension
        {
            get => newFileExtension;
            set => this.Change(ref newFileExtension, value);
        }
        
        string folderPath;
        public string FolderPath
        {
            get => folderPath;
            set => this.Change(ref folderPath, value);
        }

        Targets target = Targets.FileName;
        public Targets Target
        {
            get => target;
            set => this.Change(ref target, value);
        }

        string targetFileExtensions = string.Empty;
        public string TargetFileExtensions
        {
            get => targetFileExtensions;
            set => this.Change(ref targetFileExtensions, value);
        }

        bool targetFileExtensionsCase = false;
        public bool TargetFileExtensionsCase
        {
            get => targetFileExtensionsCase;
            set => this.Change(ref targetFileExtensionsCase, value);
        }

        FileNameTargets targetFileNames = FileNameTargets.AnyCharacters;
        public FileNameTargets TargetFileNames
        {
            get => targetFileNames;
            set => this.Change(ref targetFileNames, value);
        }

        double progress;
        public double Progress
        {
            get => progress;
            set => this.Change(ref progress, value);
        }

        public override string Title => "Rename";

        #endregion

        public RenamePanel() : base(Resources.Uri(nameof(Explorer), "/Images/Rename.png"))
        {
            Get.Current<MainViewModel>().PropertyChanged += OnPropertyChanged;
        }

        bool CoerceFileExtension(string fileExtension)
        {
            return (targetFileExtensionsCase && targetFileExtensions.Contains(fileExtension))
                    ||
                    (!targetFileExtensionsCase && targetFileExtensions.ToLower().Contains(fileExtension.ToLower()));
        }

        bool CoerceFileName(string fileName)
        {
            switch (targetFileNames)
            {
                case FileNameTargets.AnyCharacters:
                    return true;

                case FileNameTargets.Alphanumerical:
                    return fileName.AlphaNumeric();

                case FileNameTargets.NoLetters:
                    return System.Text.RegularExpressions.Regex.IsMatch(fileName, "^[^a-zA-Z]+$");

                case FileNameTargets.NoNumbers:
                    return System.Text.RegularExpressions.Regex.IsMatch(fileName, "^[^0-9]+$");

                case FileNameTargets.Numerical:
                    return fileName.Numeric();
            }

            throw new NotSupportedException();
        }

        string ConvertFileExtension(string fileExtension)
        {
            switch (fileExtensionCase)
            {
                case Case.Default:
                    return fileExtension;
                case Case.Lower:
                    return fileExtension.ToLower();
                case Case.Upper:
                    return fileExtension.ToUpper();
            }
            throw new NotSupportedException();
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Title")
            {
                if (!renaming)
                    Update();
            }
        }

        void Update() => FolderPath = Get.Current<MainViewModel>().ActiveDocument?.Path ?? string.Empty;

        ICommand startCommand;
        public ICommand StartCommand
        {
            get
            {
                startCommand = startCommand ?? new RelayCommand(async () =>
                {
                    renaming = true;

                    //Show a confirmation warning before doing anything else!
                    var result = Dialog.Show("Rename", "You are about to rename multiple files. Proceed?", DialogImage.Warning, DialogButtons.YesNo);
                    if (result == 0)
                    {
                        await Task.Run(async () =>
                        {
                            var files = System.IO.Directory.GetFiles(folderPath);
                            var count = files.Count();

                            var fileExtensions = targetFileExtensions.Split(Array<char>.New(','), StringSplitOptions.RemoveEmptyEntries);

                            //Make sure at least one file extension is specified!
                            if (target == Targets.FileExtension && fileExtensions.Empty())
                                return;

                            var j = 0;

                            foreach (var i in files)
                            {
                                var fileName = System.IO.Path.GetFileName(i);
                                var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(i);

                                if (CoerceFileName(fileNameWithoutExtension))
                                {
                                    var fileExtension = System.IO.Path.GetExtension(i).Remove(0, 1);
                                    var newFilePath = string.Empty;
                                    switch (target)
                                    {
                                        case Targets.FileExtension:

                                            //1. Check if the file extension is a target
                                            if (CoerceFileExtension(fileExtension))
                                            {
                                                //2. Check if a file with the new file name exists (the new file name is constructed from scratch!)
                                                newFilePath = $@"{folderPath}\{fileNameWithoutExtension}.{ConvertFileExtension(fileExtension)}";
                                                if (!Imagin.Common.Storage.File.Long.Exists(newFilePath))
                                                {
                                                    try
                                                    {
                                                        //3. Change the file extension by renaming (or "moving") it
                                                        Imagin.Common.Storage.File.Long.Move(i, newFilePath);
                                                    }
                                                    catch (Exception) { }
                                                }
                                            }
                                            break;

                                        case Targets.FileName:

                                            //1. If no file extensions are specified or if the file extension is a target
                                            if (fileExtensions.Empty() || CoerceFileExtension(fileExtension))
                                            {
                                                switch (fileNameOption)
                                                {
                                                    case FileNameOptions.Increment:

                                                        var index = fileNameIndex;

                                                        //2. Get a new file name with an index starting at whatever number is specified
                                                        while (true)
                                                        {
                                                            newFilePath = $@"{folderPath}\{string.Format(fileNameFormat, index)}.{ConvertFileExtension(fileExtension)}";
                                                            //3. If a file with that name doesn't already exist, rename it; otherwise, try index + 1
                                                            try
                                                            {
                                                                if (!Imagin.Common.Storage.File.Long.Exists(newFilePath))
                                                                {
                                                                    try
                                                                    {
                                                                        Imagin.Common.Storage.File.Long.Move(i, newFilePath);
                                                                    }
                                                                    catch (Exception) { }
                                                                    break;
                                                                }
                                                                index++;
                                                            }
                                                            catch (Exception) { break; }
                                                        }
                                                        break;

                                                    case FileNameOptions.Replace:

                                                        newFilePath = $@"{folderPath}\{fileNameWithoutExtension.Replace(fileNameReplace, fileNameReplaceWith)}.{ConvertFileExtension(fileExtension)}";
                                                        if (!Imagin.Common.Storage.File.Long.Exists(newFilePath))
                                                        {
                                                            try
                                                            {
                                                                Imagin.Common.Storage.File.Long.Move(i, newFilePath);
                                                            }
                                                            catch (Exception) { }
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }

                                j++;
                                await Application.Current.Dispatcher.BeginInvoke(() => Progress = Int32Extensions.Double(j) / Int32Extensions.Double(count));
                            }

                            await Application.Current.Dispatcher.BeginInvoke(() => Progress = 0);
                        });
                    }

                    Update();
                    renaming = false;
                },
                () => !renaming && System.IO.Directory.Exists(folderPath));
                return startCommand;
            }
        }

        ICommand stopCommand;
        public ICommand StopCommand
        {
            get
            {
                stopCommand = stopCommand ?? new RelayCommand(() =>
                {
                },
                () => renaming);
                return stopCommand;
            }
        }
    }
}