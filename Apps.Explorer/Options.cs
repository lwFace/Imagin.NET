using Imagin.Common;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Media;
using Imagin.Common.Storage;
using System;
using System.Windows;
using System.Windows.Media;

namespace Explorer
{
    [Serializable]
    public class Options : Data<MainViewModel>
    {
        #region Console

        string consoleOutput = string.Empty;
        [Hidden]
        public string ConsoleOutput
        {
            get => consoleOutput;
            set => this.Change(ref consoleOutput, value);
        }

        StringColor consoleBackground = Colors.Black;
        [Category(nameof(Imagin.Common.Controls.Console))]
        [DisplayName("Background")]
        public SolidColorBrush ConsoleBackground
        {
            get => consoleBackground.Brush;
            set => this.Change(ref consoleBackground, value.Color);
        }

        string consoleFontFamily = "Consolas";
        [Category(nameof(Imagin.Common.Controls.Console))]
        [DisplayName("Font family")]
        public FontFamily ConsoleFontFamily
        {
            get
            {
                if (consoleFontFamily == null)
                    return default;

                FontFamily result = null;
                Try.Invoke(() => result = new FontFamily(consoleFontFamily));
                return result;
            }
            set => this.Change(ref consoleFontFamily, value.Source);
        }

        double consoleFontSize = 16.0;
        [Category(nameof(Imagin.Common.Controls.Console))]
        [DisplayName("Font size")]
        [Range(12.0, 48.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double ConsoleFontSize
        {
            get => consoleFontSize;
            set => this.Change(ref consoleFontSize, value);
        }

        StringColor consoleForeground = Colors.White;
        [Category(nameof(Imagin.Common.Controls.Console))]
        [DisplayName("Foreground")]
        public SolidColorBrush ConsoleForeground
        {
            get => consoleForeground.Brush;
            set => this.Change(ref consoleForeground, value.Color);
        }

        string consoleTextWrap = $"{TextWrapping.NoWrap}";
        [Category(nameof(Imagin.Common.Controls.Console))]
        [DisplayName("Text wrap")]
        public TextWrapping ConsoleTextWrap
        {
            get => (TextWrapping)Enum.Parse(typeof(TextWrapping), consoleTextWrap);
            set => this.Change(ref consoleTextWrap, value.ToString());
        }

        #endregion

        #region Explorer

        string defaultFolderPath = Folder.Long.Root;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("Default folder path")]
        [StringFormat(StringFormat.FolderPath)]
        public string DefaultFolderPath
        {
            get => defaultFolderPath;
            set => this.Change(ref defaultFolderPath, value);
        }

        string root = Folder.Long.Root;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("Root")]
        [StringFormat(StringFormat.FolderPath)]
        [UpdateSourceTrigger(System.Windows.Data.UpdateSourceTrigger.LostFocus)]
        public string Root
        {
            get => root;
            set => this.Change(ref root, value);
        }

        ItemProperty groupName = ItemProperty.None;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("Group name")]
        public ItemProperty GroupName
        {
            get => groupName;
            set => this.Change(ref groupName, value);
        }

        System.ComponentModel.ListSortDirection sortDirection = System.ComponentModel.ListSortDirection.Ascending;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("Sort direction")]
        public System.ComponentModel.ListSortDirection SortDirection
        {
            get => sortDirection;
            set => this.Change(ref sortDirection, value);
        }

        ItemProperty sortName = ItemProperty.Name;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("Sort name")]
        public ItemProperty SortName
        {
            get => sortName;
            set => this.Change(ref sortName, value);
        }

        BrowserView view = BrowserView.Thumbnails;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("View")]
        public BrowserView View
        {
            get => view;
            set => this.Change(ref view, value);
        }

        bool viewFileExtensions = false;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("File extensions")]
        public bool ViewFileExtensions
        {
            get => viewFileExtensions;
            set => this.Change(ref viewFileExtensions, value);
        }

        bool viewFiles = true;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("View files")]
        public bool ViewFiles
        {
            get => viewFiles;
            set => this.Change(ref viewFiles, value);
        }

        bool viewHiddenItems = true;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("View hidden items")]
        public bool ViewHiddenItems
        {
            get => viewHiddenItems;
            set => this.Change(ref viewHiddenItems, value);
        }

        double viewSize = 64.0;
        [Category(nameof(Imagin.Common.Controls.Explorer))]
        [DisplayName("Item size")]
        [Range(8.0, 512.0, 4.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double ViewSize
        {
            get => viewSize;
            set => this.Change(ref viewSize, value);
        }

        #endregion

        #region Rename

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

        //...........................................................

        bool warnBeforeRenaming = true;
        [Category("Warnings")]
        [DisplayName("Warn before renaming")]
        public bool WarnBeforeRenaming
        {
            get => warnBeforeRenaming;
            set => this.Change(ref warnBeforeRenaming, value);
        }

        //...........................................................

        #endregion

        #region Window

        double windowHeight = 720;
        [Hidden]
        public double WindowHeight
        {
            get => windowHeight;
            set => this.Change(ref windowHeight, value);
        }

        double windowWidth = 1200;
        [Hidden]
        public double WindowWidth
        {
            get => windowWidth;
            set => this.Change(ref windowWidth, value);
        }

        #endregion
    }
}