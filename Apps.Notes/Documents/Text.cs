using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Imagin.Common.Threading;
using System;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Notes
{
    public class TextDocument : Document
    {
        readonly BackgroundQueue queue = new BackgroundQueue();

        /// ........................................................................

        public const string DefaultName = "Untitled";

        /// ........................................................................

        [Hidden]
        [XmlIgnore]
        public override string Title => $"{name}{(IsModified ? "*" : string.Empty)}";

        [Hidden]
        [XmlIgnore]
        public override object ToolTip => Path;

        int caretIndex;
        [Hidden]
        [XmlIgnore]
        public int CaretIndex
        {
            get => caretIndex;
            set => this.Change(ref caretIndex, value);
        }

        int selectionStart;
        [Hidden]
        [XmlIgnore]
        public int SelectionStart
        {
            get => selectionStart;
            set => this.Change(ref selectionStart, value);
        }

        int selectionLength;
        [Hidden]
        [XmlIgnore]
        public int SelectionLength
        {
            get => selectionLength;
            set => this.Change(ref selectionLength, value);
        }

        string name;
        [Featured]
        [UpdateSourceTrigger(System.Windows.Data.UpdateSourceTrigger.LostFocus)]
        [XmlIgnore]
        public string Name
        {
            get => name;
            set
            {
                var n = Imagin.Common.Storage.File.Long.CleanName(value);
                var nameWithExtension = $"{n}.{MainViewModel.FileExtensions[Type]}";

                //This is where we stop the change if a) the name is invalid or b) a file with that name already exists.
                var can = !System.IO.File.Exists(GetPath(nameWithExtension));

                //If the change can be made, a) 
                if (can)
                {
                    //Rename the actual file
                    var oldPath = Path;
                    var newPath = string.Empty;

                    this.Change(ref name, n);
                    this.Changed(() => Extension);
                    this.Changed(() => NameWithExtension);
                    this.Changed(() => Title);
                    this.Changed(() => ToolTip);

                    newPath = Path;
                    System.IO.File.Move(oldPath, newPath);
                }
                //Otherwise...
                else
                {
                    Dialog.Show("Rename", "Cannot rename the file. Please try again.", DialogImage.Error, DialogButtons.Ok);
                    return;
                }

            }
        }

        [Hidden]
        [XmlIgnore]
        public string NameWithExtension => $"{name}.{Extension}";

        string text;
        [Hidden]
        [XmlIgnore]
        public string Text
        {
            get => text;
            set
            {
                if (this.Change(ref text, value))
                {
                    IsModified = true;
                    OnTextChanged();
                }
            }
        }

        [Hidden]
        [XmlIgnore]
        public string Path => GetPath(NameWithExtension);

        [Hidden]
        [XmlIgnore]
        public Type Type => GetType();

        [Hidden]
        [XmlIgnore]
        public string Extension => MainViewModel.FileExtensions[Type];

        /// ........................................................................

        public TextDocument(string name, string text) : base()
        {
            this.name = name;
            this.text = text;
        }

        /// ........................................................................

        string GetPath(string nameWithExtension) => $@"{Get.Current<Options>().Folder}\{nameWithExtension}";

        /// ........................................................................

        protected virtual void OnTextChanged()
        {
            if (Get.Current<Options>().AutoSave)
                Save();
        }

        public virtual Result Open() => new Success();

        /// ........................................................................

        /// <summary>
        /// Renames the file under special circumstances (it is assumed the file extension is the same!)
        /// </summary>
        /// <param name="newName"></param>
        public void Rename(string newName)
        {
            name = newName;
            this.Changed(() => Name);
            this.Changed(() => Extension);
            this.Changed(() => NameWithExtension);
            this.Changed(() => Title);
        }

        /// ........................................................................

        protected virtual void Save(string path) => Imagin.Common.Storage.File.Long.WriteAllText(path, Text, Get.Current<Options>().Encoding.Convert());

        public override void Save()
        {
            queue.Add(() => Try.Invoke(() => Save(Path)));
            IsModified = false;
        }

        /// ........................................................................

        public static TextDocument Create(string fileName, string fileExtension, string text)
            => Activator.CreateInstance(MainViewModel.FileTypes[fileExtension.Replace(".", string.Empty)], fileName, text) as TextDocument;

        /// ........................................................................

        public static string Read(string filePath, Encoding encoding)
        {
            try
            {
                filePath = new System.IO.FileInfo(filePath).FullName;
                return Imagin.Common.Storage.File.Long.ReadAllText(filePath, encoding);
            }
            catch
            {
                return null;
            }
        }

        /// ........................................................................

        ICommand saveCommand;
        [Category(nameof(Save))]
        [DisplayName("Save")]
        [XmlIgnore]
        public ICommand SaveCommand
        {
            get
            {
                saveCommand = saveCommand ?? new RelayCommand(Save);
                return saveCommand;
            }
        }

        ICommand saveAsCommand;
        [Category(nameof(Save))]
        [DisplayName("SaveAs")]
        [XmlIgnore]
        public ICommand SaveAsCommand
        {
            get
            {
                saveAsCommand = saveAsCommand ?? new RelayCommand(() =>
                {
                    var result = ExplorerWindow.Show(out string path, "Save as", ExplorerWindow.Modes.SaveFile, null, Get.Current<Options>().Folder);
                    result.If(true, () => queue.Add(() => Try.Invoke(() => Save(path))));
                });
                return saveAsCommand;
            }
        }
    }
}