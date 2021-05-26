using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Globalization;
using Imagin.Common.Globalization.Engine;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Configuration
{
    [Serializable]
    public abstract class Data : Base
    {
        Languages language;
        [Category(nameof(Language))]
        public Languages Language
        {
            get => language;
            set => this.Change(ref language, value);
        }

        Themes.Types theme = Themes.Types.Light;
        [Category(nameof(Theme))]
        public Themes.Types Theme
        {
            get => theme;
            set => this.Change(ref theme, value);
        }

        //....................................................................................

        public Data() : base() { }

        //....................................................................................

        internal static Data Load(DataProperties properties)
        {
            BinarySerializer.Deserialize(Get.Where<SingleApplication>().Data.FilePath, out Data result);
            result = result ?? properties.Type.Create<Data>();
            result.OnLoaded();
            return result;
        }

        //....................................................................................

        public virtual void OnApplicationExit() { }

        public virtual void OnApplicationReady() { }

        public virtual void OnApplicationStart() { }

        //....................................................................................

        protected virtual void OnLoaded() { }

        protected virtual void OnSaved() { }

        //....................................................................................

        public void Save()
        {
            OnSaved();
            BinarySerializer.Serialize(Get.Where<SingleApplication>().Data.FilePath, this);
        }

        //....................................................................................

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Language):
                    LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                    LocalizeDictionary.Instance.Culture = language.Convert();
                    break;

                case nameof(Theme):
                    Get.Current<Themes>().Change(Theme);
                    break;
            }
        }

        public override string ToString() => "Options";
    }

    [Serializable]
    public abstract class Data<T> : Data where T : class, IDockViewModel
    {
        protected virtual bool SaveDocuments => true;

        //....................................................................................

        bool autoSaveLayout = false;
        [Category(nameof(Layouts))]
        [DisplayName("Auto save")]
        public bool AutoSaveLayout
        {
            get => autoSaveLayout;
            set => this.Change(ref autoSaveLayout, value);
        }

        Document[] documents = null;
        [Hidden]
        public Document[] Documents
        {
            get => documents;
            set => this.Change(ref documents, value);
        }

        string layout = string.Empty;
        [Category(nameof(Layouts))]
        [StringFormat(StringFormat.File)]
        public virtual string Layout
        {
            get => layout.NullOrEmpty() ? Get.Current<DockView>().Layouts.Path : layout;
            set => this.Change(ref layout, value);
        }

        [field: NonSerialized]
        ICommand openLayoutsFolder;
        [Category(nameof(Layouts))]
        [AlternativeName("...")]
        [DisplayName("Open folder")]
        public virtual ICommand OpenLayoutsFolder
        {
            get
            {
                openLayoutsFolder = openLayoutsFolder ?? new RelayCommand(() => Storage.Machine.OpenInWindowsExplorer(Get.Current<DockView>().Layouts.Path), () => true);
                return openLayoutsFolder;
            }
        }

        [Category(nameof(Window))]
        public PanelCollection Panels => Get.Where<IDockViewModel>().Panels;

        //....................................................................................

        internal Dictionary<string, bool> PanelVisibility { get; private set; }

        //....................................................................................

        void OnPanelChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Panel.IsVisible):
                    PanelVisibility[(sender as Panel).Name] = (sender as Panel).IsVisible;
                    break;
            }
        }

        //....................................................................................

        public override void OnApplicationExit()
        {
            base.OnApplicationExit();

            if (autoSaveLayout)
            {
                if (!Storage.File.Long.Exists(layout))
                    layout = Storage.File.Long.ClonePath($@"{Get.Current<DockView>().Layouts.Path}\Untitled.xml");

                Get.Current<DockView>().Layouts?.Save(Path.GetFileNameWithoutExtension(layout));
            }

            if (SaveDocuments)
                Documents = Get.Where<T>().Documents.Cast<Document>().ToArray();
        }

        public override void OnApplicationReady()
        {
            base.OnApplicationReady();
            if (Documents?.Length > 0)
                0.For(Documents.Length, i => Get.Where<T>().Documents.Add(Documents[i]));

            if (PanelVisibility == null)
                PanelVisibility = new Dictionary<string, bool>();

            foreach (var i in Panels)
            {
                if (!PanelVisibility.ContainsKey(i.Name))
                    PanelVisibility.Add(i.Name, i.IsVisible);
            }

            for (var i = PanelVisibility.Count - 1; i >= 0; i--)
            {
                var item = PanelVisibility.ElementAt(i);
                if (!Panels.Contains(j => j.Name.Equals(item.Key)))
                {
                    PanelVisibility.Remove(item.Key);
                    continue;
                }
                Panels[item.Key].IsVisible = item.Value;
                Panels[item.Key].PropertyChanged += OnPanelChanged;
            }
        }

        //....................................................................................

        [field: NonSerialized]
        ICommand resetLayout;
        [Category(nameof(Layouts))]
        [AlternativeName("...")]
        [DisplayName("Reset layout")]
        public virtual ICommand ResetLayout
        {
            get
            {
                resetLayout = resetLayout ?? new RelayCommand(() => Layout = string.Empty, () => true);
                return resetLayout;
            }
        }

        [field: NonSerialized]
        ICommand saveLayout;
        [Category(nameof(Layouts))]
        [AlternativeName("...")]
        [DisplayName("Save layout")]
        public virtual ICommand SaveLayout
        {
            get
            {
                saveLayout = saveLayout ?? new RelayCommand(() =>
                {
                    var inputWindow = new InputWindow();
                    inputWindow.Placeholder = "File name without extension";
                    inputWindow.Title = "Save layout...";
                    inputWindow.ShowDialog();

                    if (!inputWindow.Result.NullOrEmpty())
                    {
                        Get.Current<DockView>().Layouts?.Save(inputWindow.Result);
                        Dialog.Show("Save layout", "Layout saved!", DialogImage.Information, DialogButtons.Ok);
                        return;
                    }

                }, () => true);
                return saveLayout;
            }
        }
    }
}