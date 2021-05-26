﻿using Imagin.Common;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Storage;
using Imagin.Common.Text;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Notes
{
    [Serializable]
    public class Options : Data<MainViewModel>
    {
        protected override bool SaveDocuments => false;

        enum Category
        {
            Documents,
            Find,
            Format,
            List,
            Sort,
            Theme,
            View,
            Window
        }

        bool autoSave = false;
        [Category(Category.Documents)]
        [DisplayName("Auto save")]
        public bool AutoSave
        {
            get => autoSave;
            set => this.Change(ref autoSave, value);
        }

        bool listDeleteEmptyLines = false;
        [Category(Category.List)]
        [DisplayName("Delete empty lines")]
        public bool ListDeleteEmptyLines
        {
            get => listDeleteEmptyLines;
            set => this.Change(ref listDeleteEmptyLines, value);
        }

        Encoding encoding;
        [Category(Category.Format)]
        public Encoding Encoding
        {
            get => encoding;
            set => this.Change(ref encoding, value);
        }

        Favorites favorites = new Favorites();
        [Hidden]
        public Favorites Favorites
        {
            get => favorites = favorites ?? new Favorites();
            set => this.Change(ref favorites, value);
        }

        MatchSource findMatchSource = MatchSource.CurrentDocument;
        [Hidden]
        public MatchSource FindMatchSource
        {
            get => findMatchSource;
            set => this.Change(ref findMatchSource, value);
        }

        string folder = Imagin.Common.Storage.Folder.Long.Root;
        [Hidden]
        public string Folder
        {
            get => folder;
            set => this.Change(ref folder, value);
        }

        string fontFamily;
        [Category(Category.Format)]
        [DisplayName("Font family")]
        public FontFamily FontFamily
        {
            get
            {
                if (fontFamily == null)
                    return default;

                FontFamily result = null;
                Try.Invoke(() => result = new FontFamily(fontFamily));
                return result;
            }
            set => this.Change(ref fontFamily, value.Source);
        }

        double fontSize = 16;
        [Category(Category.Format)]
        [DisplayName("Font size")]
        [Range(8.0, 72.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        History history = new History(Explorer.DefaultLimit);
        [Hidden]
        public History History
        {
            get => history;
            set => this.Change(ref history, value);
        }

        ItemProperty sortName = ItemProperty.Name;
        [Category(Category.Sort)]
        [DisplayName("Name")]
        public ItemProperty SortName
        {
            get => sortName;
            set => this.Change(ref sortName, value);
        }

        SortDirection sortDirection = SortDirection.Descending;
        [Category(Category.Sort)]
        [DisplayName("Direction")]
        public SortDirection SortDirection
        {
            get => sortDirection;
            set => this.Change(ref sortDirection, value);
        }

        Notes viewNotes = Notes.All;
        [Category(Category.View)]
        [DisplayName("Notes")]
        [EnumFormat(EnumFormat.Flags)]
        public Notes ViewNotes
        {
            get => viewNotes;
            set => this.Change(ref viewNotes, value);
        }

        string textWrap = $"{TextWrapping.Wrap}";
        [Category(Category.Format)]
        [DisplayName("Word wrap")]
        public TextWrapping TextWrap
        {
            get => (TextWrapping)Enum.Parse(typeof(TextWrapping), textWrap);
            set => this.Change(ref textWrap, value.ToString());
        }

        uint tab = 4;
        [Category(Category.Format)]
        [Range((uint)1, (uint)8, (uint)1)]
        [RangeFormat(RangeFormat.Slider)]
        public uint Tab
        {
            get => tab;
            set => this.Change(ref tab, value);
        }
    }
}