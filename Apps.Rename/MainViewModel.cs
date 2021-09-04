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

namespace Rename
{
    [Serializable]
    public enum Warnings
    {
        None,
        Once,
        Twice
    }

    //............................................................................

    [Serializable]
    public enum FileExtensionFormats
    {
        Default,
        Lower,
        Upper
    }

    //............................................................................

    [Serializable]
    public enum Targets
    {
        FileExtension = 0,
        FileName = 1
    }

    //............................................................................

    [Serializable]
    public enum FileNameTargets
    {
        AnyCharacters,
        NoNumbers,
        NoLetters,
        OnlyNumbers,
        LettersAndNumbers
    }

    [Serializable]
    public enum FileNameOptions
    {
        Increment,
        Replace
    }

    public class MainViewModel : MainViewModel<MainWindow>
    {
        bool renaming;
        public bool Renaming
        {
            get => renaming;
            set => this.Change(ref renaming, value);
        }

        //............................................................................

        double progress;
        public double Progress
        {
            get => progress;
            set => this.Change(ref progress, value);
        }

        //............................................................................

        bool CoerceFileExtension(string fileExtension)
        {
            return (Get.Current<Options>().TargetFileExtensionsCase && Get.Current<Options>().TargetFileExtensions.Contains(fileExtension))
                    ||
                    (!Get.Current<Options>().TargetFileExtensionsCase && Get.Current<Options>().TargetFileExtensions.ToLower().Contains(fileExtension.ToLower()));
        }

        bool CoerceFileName(string fileName)
        {
            switch (Get.Current<Options>().TargetFileNames)
            {
                case FileNameTargets.AnyCharacters:
                    return true;

                case FileNameTargets.LettersAndNumbers:
                    return fileName.AlphaNumeric();

                case FileNameTargets.NoLetters:
                    return System.Text.RegularExpressions.Regex.IsMatch(fileName, "^[^a-zA-Z]+$");

                case FileNameTargets.NoNumbers:
                    return System.Text.RegularExpressions.Regex.IsMatch(fileName, "^[^0-9]+$");

                case FileNameTargets.OnlyNumbers:
                    return fileName.Numeric();
            }

            throw new NotSupportedException();
        }

        string ConvertFileExtension(string fileExtension)
        {
            switch (Get.Current<Options>().FileExtensionFormat)
            {
                case FileExtensionFormats.Default:
                    return fileExtension;
                case FileExtensionFormats.Lower:
                    return fileExtension.ToLower();
                case FileExtensionFormats.Upper:
                    return fileExtension.ToUpper();
            }
            throw new NotSupportedException();
        }

        //............................................................................

        bool viewOptions = false;
        public bool ViewOptions
        {
            get => viewOptions;
            set => this.Change(ref viewOptions, value);
        }

        //............................................................................

        public MainViewModel() : base() { }

        //............................................................................

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        ICommand startCommand;
        public ICommand StartCommand
        {
            get
            {
                startCommand = startCommand ?? new RelayCommand(async () =>
                {
                    Renaming = true;

                    var title = "Rename";
                    var message = $"You are about to rename files in \"{Get.Current<Options>().FolderPath}\". Continue?";
                    
                    //Show a confirmation warning before doing anything else!
                    var result = Get.Current<Options>().Warn ? Dialog.Show(title, message, DialogImage.Warning, DialogButtons.YesNo) : (int?)null;
                    if (result != 1)
                    {
                        var folderPath = Get.Current<Options>().FolderPath;
                        await Task.Run(async () =>
                        {
                            var files = Imagin.Common.Storage.Folder.Long.GetFiles(folderPath, !Get.Current<Options>().TopDirectoryOnly ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);
                            var count = files.Count();

                            var targetFileExtensions = Get.Current<Options>().TargetFileExtensions.Split(Array<char>.New(','), StringSplitOptions.RemoveEmptyEntries);

                            var j = 0;
                            var index = 0;
                            foreach (var i in files)
                            {
                                var fileName = System.IO.Path.GetFileName(i);
                                var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(i);

                                if (CoerceFileName(fileNameWithoutExtension))
                                {
                                    var fileExtension = System.IO.Path.GetExtension(i).Remove(0, 1);
                                    var newFilePath = string.Empty;

                                    //If no file extensions are specified or if the file extension is a target
                                    if (targetFileExtensions.Empty() || CoerceFileExtension(fileExtension))
                                    {
                                        switch (Get.Current<Options>().Target)
                                        {
                                            case Targets.FileExtension:
                                                //Check if a file with the new name exists
                                                newFilePath = $@"{folderPath}\{fileNameWithoutExtension}.{ConvertFileExtension(fileExtension)}";
                                                if (!Imagin.Common.Storage.File.Long.Exists(newFilePath))
                                                {
                                                    try
                                                    {
                                                        //Change the file extension by renaming (or "moving") it
                                                        Imagin.Common.Storage.File.Long.Move(i, newFilePath);
                                                    }
                                                    catch (Exception) { }
                                                }
                                                break;

                                            case Targets.FileName:
                                                switch (Get.Current<Options>().FileNameOption)
                                                {
                                                    case FileNameOptions.Increment:

                                                        if (Get.Current<Options>().StartAtForAllFileExtensions)
                                                            index = Get.Current<Options>().FileNameIndex;

                                                        //Get a new file name with an index starting at whatever number is specified
                                                        while (true)
                                                        {
                                                            newFilePath = $@"{folderPath}\{string.Format(Get.Current<Options>().FileNameFormat, index)}.{ConvertFileExtension(fileExtension)}";
                                                            //If a file with that name doesn't already exist, rename it; otherwise, try index + 1
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
                                                                index += Get.Current<Options>().FileNameIncrement;
                                                            }
                                                            catch (Exception) { break; }
                                                        }
                                                        break;

                                                    case FileNameOptions.Replace:
                                                        newFilePath = $@"{folderPath}\{fileNameWithoutExtension.Replace(Get.Current<Options>().FileNameReplace, Get.Current<Options>().FileNameReplaceWith)}.{ConvertFileExtension(fileExtension)}";
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
                                                break;
                                        }
                                    }
                                }

                                j++;
                                await Application.Current.Dispatcher.BeginInvoke(() => Progress = Int32Extensions.Double(j) / Int32Extensions.Double(count));
                            }

                            await Application.Current.Dispatcher.BeginInvoke(() => Progress = 0);
                        });
                    }

                    Renaming = false;
                },
                () => !renaming && System.IO.Directory.Exists(Get.Current<Options>().FolderPath));
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