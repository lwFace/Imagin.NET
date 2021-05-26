using Imagin.Common;
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
    }
}