using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace Notes
{
    public class MainViewModel : MainViewModel<MainWindow, TextDocument>
    {
        public readonly static Dictionary<Type, string> FileExtensions = new Dictionary<Type, string>()
        {
            { typeof(List),
                "list" },
            { typeof(Note),
                "txt" },
        };

        public readonly static Dictionary<string, Type> FileTypes = new Dictionary<string, Type>()
        {
            { "list",
                typeof(List) },
            { "txt",
                typeof(Note) },
        };

        /// ........................................................................

        public MainViewModel() : base()
        {
            Panel<NotesPanel>().FileSelected += (sender, e) => Open(e.Value.Path);
        }

        /// ........................................................................

        public override IEnumerable<Panel> Load()
        {
            yield return new FavoritesPanel();
            yield return new FindPanel();
            yield return new NotesPanel();
            yield return new OptionsPanel();
            yield return new PropertiesPanel();
        }

        /// ........................................................................

        public void Open(string filePath)
        {
            void error() => Dialog.Show(nameof(Open), $"Cannot open '{filePath}'.", DialogImage.Error, DialogButtons.Ok);

            foreach (var i in Documents)
            {
                var j = (TextDocument)i;
                if (j.Path == filePath)
                {
                    ActiveContent = j;
                    return;
                }
            }

            var text = TextDocument.Read(filePath, Get.Current<Options>().Encoding.Convert());
            if (text == null)
            {
                error();
                return;
            }

            var fileName = filePath.Replace($"{Get.Current<Options>().Folder}\\", string.Empty);

            var fileExtension = Path.GetExtension(fileName);
            fileName = fileName.Replace($"{fileExtension}", string.Empty);

            var document = TextDocument.Create(fileName, fileExtension, text);
            if (!document.Open())
            {
                error();
                return;
            }

            Documents.Add(document);
        }

        /// ........................................................................

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                saveCommand = saveCommand ?? new RelayCommand(() => ActiveDocument?.Save());
                return saveCommand;
            }
        }
    }
}