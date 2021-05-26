using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Imagin.Common.Controls
{
    public abstract class BaseCommand : NamedObject
    {
        internal Console Console { get; set; }

        //..................................................................

        public abstract string Description { get; }

        public string AllNames => string.Join(", ", Names());

        new public virtual string Name => Names().First();

        public abstract IEnumerable<string> Names();

        public virtual IEnumerable<string> Usage() => Enumerable.Empty<string>();

        //..................................................................

        public string Folder
        {
            get => Console.Folder;
            set => Console.SetCurrentValue(Console.FolderProperty, value);
        }

        public string Output
        {
            get => Console.Output;
            set => Console.SetCurrentValue(Console.OutputProperty, value);
        }

        //..................................................................

        protected virtual string Outputs(params string[] arguments) => default;

        //..................................................................

        string[] Parse(string line, char separator) => line.Split(Array<char>.New(separator), 2, StringSplitOptions.RemoveEmptyEntries);

        protected virtual string[] Parse(string line) => line.NullOrEmpty() ? null : Array<string>.New(line);

        protected string Path(string input) => Path(Folder, input);

        protected string Path(string a, string b) => $@"{a}\{b}".Replace(@"\\", @"\");

        //..................................................................

        public virtual void Write(string line) => Console.Write($"{Folder}> {line}");

        //..................................................................

        protected abstract void Execute(string name, string[] arguments);

        public void Execute(string line)
        {
            Write(line);
            var arguments = Parse(line, ' ');
            Execute(arguments[0], Parse(arguments.ElementAtOrDefault(1)) ?? Array<string>.New());
        }
    }

    public abstract class ItemCommand : BaseCommand
    {
        protected virtual char Separator => '|';

        protected override string[] Parse(string line) => line?.Split(Array<char>.New(Separator), StringSplitOptions.RemoveEmptyEntries);
    }

    //..................................................................

    public class BackCommand : BaseCommand
    {
        public override string Description => "Go to the previous folder.";

        public override IEnumerable<string> Names()
        {
            yield return "Back";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (!Console.History.Undo(i => Console.HandleFolder.Invoke(() => Console.Folder = i)))
                throw new InvalidOperationException();
        }
    }

    public class CurrentCommand : BaseCommand
    {
        public override string Description => "Print the current folder path.";

        public override IEnumerable<string> Names()
        {
            yield return "Current";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            var result = new StringBuilder();
            result.AppendLine(Folder.PadLeft(' ', Console.LinePadding));
            Console.WriteBlock(result.ToString());
        }
    }

    public class NextCommand : BaseCommand
    {
        public override string Description => "Go to the next folder.";

        public override IEnumerable<string> Names()
        {
            yield return "Next";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (!Console.History.Redo(i => Console.HandleFolder.Invoke(() => Console.Folder = i)))
                throw new InvalidOperationException();
        }
    }

    //..................................................................

    public class AttributesCommand : BaseCommand
    {
        public override string Description => "Show attribute(s) of specified file or folder, or show attribute(s) of current folder if nothing is specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Attributes";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name} {{the current folder}}]";
            yield return $"[{Name}] [{{file or folder path in current folder}} or {{absolute file or folder path}}]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            string path = null;
            switch (arguments.Length)
            {
                case 0:
                    path = Folder;
                    break;

                case 1:
                    path = arguments[0];
                    if (!Storage.File.Long.Exists(path) && !Storage.Folder.Long.Exists(path))
                        path = Path(path);

                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            var attributes = Storage.File.Long.Attributes(path);

            var result = new StringBuilder();
            result.AppendLine(string.Empty);

            var line = attributes.ToString();
            line = line.PadLeft(line.Length + Console.LinePadding, ' ');

            result.AppendLine(line);
            Console.Write(result.ToString());
        }
    }

    public class ChangeFolderCommand : BaseCommand
    {
        public override string Description => "Set the current directory.";

        public override IEnumerable<string> Names()
        {
            yield return "..";
            yield return "Cd";
            yield return "Up";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[.. <the parent folder>]";
            yield return $"[Up <the parent folder>]";
            yield return $"[Cd] [{{folder name in current folder}} or {{absolute folder path}} or {{.., ..., etc.}}]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            switch (name.ToLower())
            {
                case "cd":

                    if (arguments.Length != 1)
                        throw new ArgumentOutOfRangeException();

                    string folderPath = arguments[0];
                    //First, check if it is a full path
                    if (Storage.Folder.Long.Exists(folderPath))
                        Folder = folderPath;

                    else
                    {
                        //Second, check if it only contains periods
                        if (folderPath.OnlyContains('.'))
                        {
                            //Supports '..', '...', '....', etc...
                            var up = folderPath.Repeats('.') - 1;
                            folderPath = Folder;
                            while (up > 0)
                            {
                                folderPath = System.IO.Path.GetDirectoryName(folderPath);
                                up--;
                            }
                        }
                        else
                        {
                            //Third, check if it is a partial path
                            folderPath = Path(folderPath);
                        }

                        //If what we have at this point, exists...
                        if (Storage.Folder.Long.Exists(folderPath))
                            Folder = Storage.Folder.Long.ActualPath(folderPath);

                        else throw new DirectoryNotFoundException(folderPath);
                    }
                    break;

                case "..":
                case "up":
                    if (arguments.Length == 0)
                    {
                        var parent = System.IO.Path.GetDirectoryName(Folder);
                        if (!Storage.Folder.Long.Exists(parent))
                            throw new DirectoryNotFoundException();

                        Folder = parent;
                        break;
                    }
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class ClearCommand : BaseCommand
    {
        public override string Description => "Clear a) the current output or b) history.";

        public override IEnumerable<string> Names()
        {
            yield return "Clr";
            yield return "Clear";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{AllNames}]";
            yield return $"[{AllNames}] [History]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments?.Length > 0)
            {
                if (arguments[0].ToLower() == "history")
                {
                    Console.History.Clear();
                    return;
                }
                throw new ArgumentOutOfRangeException();
            }
            Try.Invoke(() => Output = string.Empty);
        }
    }

    public class EchoCommand : BaseCommand
    {
        public override string Description => "Print text to output.";

        public override IEnumerable<string> Names()
        {
            yield return "Echo";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name}] [(*)*]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length > 0)
            {
                var result = new StringBuilder();
                result.AppendLine(string.Empty);

                var line = arguments[0];
                line = line.PadLeft(line.Length + Console.LinePadding, ' ');

                result.AppendLine(line);
                Console.Write(result.ToString());
            }
            throw new ArgumentOutOfRangeException();
        }
    }

    public class ExitCommand : BaseCommand
    {
        public override string Description => "Exit application.";

        public override IEnumerable<string> Names()
        {
            yield return "Exit";
        }

        protected override void Execute(string name, params string[] arguments) => Environment.Exit(0);
    }

    public class HelpCommand : BaseCommand
    {
        public override string Description => "Show list of available commands.";

        public override IEnumerable<string> Names()
        {
            yield return "?";
            yield return "Help";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            var result = new StringBuilder();

            int left = Console.LinePadding;
            int right = 40;

            var index = 0;
            var commands = Console.Commands.OrderBy(x => x.Names().First());

            var count = commands.Count();
            foreach (var i in commands)
            {
                string line1 = null;
                i.Names().ForEach(j => line1 = line1 == null ? j.ToString() : $"{line1}, {j}");

                line1 = line1.PadLeft(line1.Length + left, ' ');
                line1 = line1.PadRight(right, ' ');
                line1 = string.Concat(line1, i.Description);
                result.AppendLine(line1);

                var usage = i.Usage();
                if (usage.Any())
                {
                    result.AppendLine(string.Empty);
                    foreach (var j in usage)
                        result.AppendLine($"> {j}".PadLeft(j.Length + left + right, ' '));
                }

                if (index < count - 1)
                    result.AppendLine(string.Empty);

                index++;
            }
            Console.WriteBlock(result.ToString());
        }
    }

    public class HistoryCommand : BaseCommand
    {
        public override string Description => "Show list of visited folders.";

        public override IEnumerable<string> Names()
        {
            yield return "History";
            yield return "Visited";
        }

        protected override void Execute(string name, string[] arguments)
        {
            if (Console.History?.Count > 0)
            {
                var result = new StringBuilder();
                foreach (var i in Console.History)
                    result.AppendLine(i.PadLeft(i.Length + Console.LinePadding, ' '));

                Console.WriteBlock(result.ToString());
                return;
            }
            throw new NullReferenceException($"{nameof(Collections.ObjectModel.History)} does not exist");
        }
    }

    public class InCommand : BaseCommand
    {
        public override string Description => "Copy clipboard to output.";

        public override IEnumerable<string> Names()
        {
            yield return "In";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            var result = System.Windows.Clipboard.GetText();
            Console.Write(result.PadLeft(result.Length + Console.LinePadding, ' '));
        }
    }

    public class ListCommand : BaseCommand
    {
        public override string Description => "List names of all files and folders in the current folder (optionally, only include items with the specified extension).";

        public override IEnumerable<string> Names()
        {
            yield return "Ls";
            yield return "List";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{AllNames}]";
            yield return $"[{AllNames}] [.(*) {{a file extension}}]";
        }

        string Print(string folderPath, params string[] extensions)
        {
            var result = new StringBuilder();

            var j = 0;
            foreach (var i in Storage.Folder.Long.GetItems(folderPath))
            {
                if (extensions?.Length > 0)
                {
                    var include = false;
                    foreach (var k in extensions)
                    {
                        if (i.ToLower().EndsWith(k.ToLower()))
                        {
                            include = true;
                            break;
                        }
                    }
                    if (!include)
                        goto Continue;
                }

                var name = System.IO.Path.GetFileName(i);
                result.AppendLine(name.PadLeft(' ', Console.LinePadding));
                j++;

                Continue: continue;
            }
            return j > 0 ? result.ToString() : throw new ItemsNotFoundException(folderPath);
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length <= 1)
            {
                string[] extensions = null;

                string folderPath = null;
                if (arguments.Length == 0)
                {
                    folderPath = Folder;
                }
                else if (arguments[0].StartsWith("."))
                {
                    folderPath = Folder;
                    extensions = Array<string>.New(arguments[0]);
                }
                else if (Storage.Folder.Long.Exists(arguments[0]))
                {
                    folderPath = arguments[0];
                }
                else
                {
                    arguments[0] = Path(arguments[0]);
                    if (Storage.Folder.Long.Exists(arguments[0]))
                        folderPath = arguments[0];

                    else throw new DirectoryNotFoundException("Directory not found.");
                }

                if (Storage.Folder.Long.Exists(folderPath))
                {
                    var result = Print(folderPath, extensions);
                    Console.WriteBlock(result);
                }

                else throw new DirectoryNotFoundException($"Directory '{folderPath}' not found.");
                return;
            }
            throw new ArgumentOutOfRangeException("Only one argument is allowed.");
        }
    }

    public class MathCommand : BaseCommand
    {
        MathParser parser = new MathParser();

        public override string Description => "Solve a math equation [+, -, *, /, ()].";

        public override IEnumerable<string> Names()
        {
            yield return "Math";
            yield return "Solve";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{AllNames}] [a math equation]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments?.Length == 1)
            {
                var result = new StringBuilder();

                arguments[0] = $"{Equation.Clean(arguments[0])}";
                var line = $"{arguments[0]} = {parser.Solve(arguments[0])}";
                line = line.PadLeft(' ', Console.LinePadding);

                result.AppendLine(line);
                Console.WriteBlock(result.ToString());
            }
            throw new ArgumentOutOfRangeException();
        }
    }

    public class OutCommand : BaseCommand
    {
        public override string Description => "Copy output to clipboard.";

        public override IEnumerable<string> Names()
        {
            yield return "Out";
        }

        protected override void Execute(string name, params string[] arguments) => System.Windows.Clipboard.SetText(Console.Output);
    }

    //..................................................................

    public class OpenCommand : ItemCommand
    {
        public override string Description => "Open a file (if specified); otherwise, open the specified folder in WindowsExplorer, or open the current folder if nothing is specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Open";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name} <the current folder>]";
            yield return $"[{Name}] [folder name in current folder]";
            yield return $"[{Name}] ([file name(s) in current folder]{Separator})*";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            switch (arguments.Length)
            {
                case 0:
                    Machine.OpenInWindowsExplorer(Folder);
                    break;

                default:
                    var first = Path(arguments[0]);
                    if (Storage.Folder.Long.Exists(first))
                        Machine.OpenInWindowsExplorer(first);

                    return;
            }
            foreach (var i in arguments)
                Try.Invoke(() => System.Diagnostics.Process.Start(Path(i)), e => Write($"Command '{name}' failed: {e.Message}"));
        }
    }

    public class PropertiesCommand : ItemCommand
    {
        public override string Description => "Open properties for the specified file(s) and/or folder(s), or open properties for the current folder if nothing is specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Properties";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name} <the current folder>]";
            yield return $"[{Name}] ([file and/or folder name(s) in current folder]{Separator})*";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Machine.Properties.Show(Folder);
                return;
            }
            for (int i = 0; i < arguments?.Length; i++)
                arguments[i] = Path(arguments[i]);

            Machine.Properties.Show(arguments);
        }
    }

    //..................................................................

    public class CreateFileCommand : ItemCommand
    {
        public override string Description => "Create specified files.";

        public override IEnumerable<string> Names()
        {
            yield return "Mf";
            yield return "MakeFile";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{AllNames}] ([file name(s) in current folder]{Separator})*";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length > 0)
            {
                foreach (var i in arguments)
                {
                    Try.Invoke(() =>
                    {
                        var path = Path(i);
                        System.IO.File.Create(path);
                        Write($"Created '{path}'");
                    }, 
                    e => Write(e.Message));
                }
            }
            throw new Exception("No file name(s) were specified.");
        }
    }

    public class CreateFolderCommand : ItemCommand
    {
        public override string Description => "Create specified folders.";

        public override IEnumerable<string> Names()
        {
            yield return "Md";
            yield return "MkDir";
            yield return "MakeDirectory";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{AllNames}] ([folder name(s) in current folder]{Separator})*";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length > 0)
            {
                foreach (var i in arguments)
                {
                    Try.Invoke(() =>
                    {
                        var path = Path(i);
                        Storage.Folder.Long.Create(path);
                        Write($"Created '{path}'");
                    }, 
                    e => Write(e.Message));
                }
            }
            throw new Exception("No folder name(s) were specified.");
        }
    }

    /// <summary>
    /// Parsing will fail!
    /// </summary>
    public class CopyCommand : ItemCommand
    {
        protected override char Separator => ' ';

        public override string Description => "Copy specified files and/or folders, or copy the current folder if none are specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Copy";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name} <the current folder>] [absolute path of folder to copy to]";
            yield return $"[{Name}] [file or folder name(s) in current folder]  [absolute path of folder to copy to]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length.Within(1, 2))
            {
                switch (arguments.Length)
                {
                    case 1:
                        if (Storage.Folder.Long.Exists(Folder))
                        {
                            if (Storage.Folder.Long.Exists(arguments[0]))
                            {
                                Machine.Copy(Folder, arguments[0]);
                                Write($"Copied '{Folder}' to '{arguments[0]}'");
                                break;
                            }
                            throw new DirectoryNotFoundException(arguments[0]);
                        }
                        throw new DirectoryNotFoundException(Folder);

                    case 2:
                        if (Storage.Folder.Long.Exists(arguments[1]))
                        {
                            var source = Path(arguments[0]);
                            if (Storage.File.Long.Exists(source) || Storage.Folder.Long.Exists(source))
                            {
                                Machine.Copy(source, arguments[1]);
                                Write($"Copied '{source}' to '{arguments[1]}'");
                                break;
                            }
                            throw new ItemNotFoundException(source);
                        }
                        throw new DirectoryNotFoundException(arguments[1]);
                }
            }
            throw new ArgumentOutOfRangeException($"Only one or two arguments can be specified. If renaming the current folder, specify the new name only. If renaming a containing file or folder, specify the old and new name of the file or folder to be renamed separated by '{Separator}'.");
        }
    }

    public class DeleteCommand : ItemCommand
    {
        public override string Description => "Delete specified file(s) and/or folder(s), or delete the current folder if nothing is specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Del";
            yield return "Delete";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{AllNames} <the current folder>]";
            yield return $"[{AllNames}] ([file or folder name(s) in current folder]{Separator})*";
        }

        protected virtual string Completed() => "Deleted";

        protected virtual void DeleteFile(string filePath) => Storage.File.Long.Delete(filePath);

        protected virtual void DeleteFolder(string folderPath) => Storage.Folder.Long.Delete(folderPath);

        protected virtual int Show() => Dialog.Show("Delete", "Are you sure you want to delete this?", DialogImage.Warning, DialogButtons.ContinueCancel);

        protected override void Execute(string name, params string[] arguments)
        {
            var result = Show();
            if (result == 0)
            {
                if (arguments.Length > 0)
                {
                    foreach (var i in arguments)
                    {
                        Try.Invoke(() =>
                        {
                            var path = Path(i);
                            if (Storage.File.Long.Exists(path))
                            {
                                //DeleteFile(path);
                                Write($"{Completed()} '{path}'");
                            }
                            else if (Storage.Folder.Long.Exists(path))
                            {
                                //DeleteFolder(path);
                                Write($"{Completed()} '{path}'");
                            }
                        },
                        e => Write(e.Message));
                    }
                }
                else
                {
                    if (Folder.EndsWith(@":\"))
                        throw new InvalidOperationException("Cannot delete a drives.");

                    var newFolder = System.IO.Path.GetDirectoryName(Folder);
                    var oldFolder = Folder;

                    if (!Storage.Folder.Long.Exists(newFolder))
                        throw new InvalidOperationException("Cannot delete the current folder.");

                    Folder = newFolder;
                    //DeleteFolder(oldFolder);
                    Write($"{Completed()} '{oldFolder}'");
                }
            }
        }
    }

    public class HideShowCommand : ItemCommand
    {
        public override string Description => "Hide/show the specified file(s) and/or folder(s), or hide/show the current folder if nothing is specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Hide";
            yield return "Show";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{AllNames} <the current folder>]";
            yield return $"[{AllNames}] ([file or folder name(s) in current folder]{Separator})*";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            bool hide = name.ToLower() == "hide";

            string message(string i) => $"'{i}' is now {(hide ? "hidden" : "visible")}";

            if (arguments.Length > 0)
            {
                foreach (var i in arguments)
                {
                    Try.Invoke(() =>
                    {
                        var path = Path(i);
                        Storage.File.Long.Hide(path, hide);
                        Write(message(path));
                    },
                    e => Write(e.Message));
                }
            }
            else
            {
                Storage.File.Long.Hide(Folder, hide);
                Write(message(Folder));
            }
        }
    }

    /// <summary>
    /// Parsing will fail!
    /// </summary>
    public class MoveCommand : ItemCommand
    {
        protected override char Separator => ' ';

        public override string Description => "Move the specified file or folder.";

        public override IEnumerable<string> Names()
        {
            yield return "Move";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name} <the current folder>] [absolute path of folder to move to]";
            yield return $"[{Name}] [file or folder name in current folder] [absolute path of folder to move to]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length.Within(1, 2))
            {
                string oldPath = null, newPath = null;
                switch (arguments.Length)
                {
                    //Move the current folder
                    case 1:
                        oldPath = Folder;
                        newPath = arguments[0];

                        if (Storage.Folder.Long.Exists(newPath))
                        {
                            Storage.Folder.Long.Move(oldPath, Path(newPath, System.IO.Path.GetFileName(oldPath)));
                            Folder = newPath;
                            Write($"Moved '{oldPath}' to '{newPath}'");
                            return;
                        }
                        throw new DirectoryNotFoundException(newPath);

                    //Move the specified file or folder
                    case 2:
                        oldPath = Path(arguments[0]);
                        newPath = arguments[1];
                        if (Storage.Folder.Long.Exists(newPath))
                        {
                            newPath = Path(newPath, System.IO.Path.GetFileName(oldPath));
                            if (Storage.File.Long.Exists(oldPath))
                                Storage.File.Long.Move(oldPath, newPath);

                            else if (Storage.Folder.Long.Exists(oldPath))
                                Storage.Folder.Long.Move(oldPath, newPath);

                            else throw new ItemNotFoundException(oldPath);
                            Write($"Moved '{oldPath}' to '{newPath}'");
                            return;
                        }
                        throw new DirectoryNotFoundException(newPath);
                }
            }
            throw new ArgumentOutOfRangeException($"Only one or two arguments can be specified. If renaming the current folder, specify the new name only. If renaming a containing file or folder, specify the old and new name of the file or folder to be renamed separated by '{Separator}'.");
        }
    }

    public class RecycleCommand : DeleteCommand
    {
        public override string Description => "Recycle specified file(s) and/or folder(s), or recycle the current folder if nothing is specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Recycle";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name} <the current folder>]";
            yield return $"[{Name}] ([file or folder name(s) in current folder]{Separator})*";
        }

        protected override string Completed() => "Recycled";

        protected override void DeleteFile(string filePath) => Machine.Recycle(filePath);

        protected override void DeleteFolder(string folderPath) => Machine.Recycle(folderPath);

        protected override int Show() => Dialog.Show("Recycle", "Are you sure you want to recycle this?", DialogImage.Warning, DialogButtons.ContinueCancel);
    }

    /// <summary>
    /// Parsing will fail!
    /// </summary>
    public class RenameCommand : ItemCommand
    {
        protected override char Separator => ' ';

        public override string Description => "Renames the specified file or folder, or renames the current folder if nothing is specified.";

        public override IEnumerable<string> Names()
        {
            yield return "Rename";
        }

        public override IEnumerable<string> Usage()
        {
            yield return $"[{Name} <the current folder>] [new folder name]";
            yield return $"[{Name}] [file or folder in current folder] [new file or folder name]";
        }

        protected override void Execute(string name, params string[] arguments)
        {
            if (arguments.Length.Within(1, 2))
            {
                string oldPath = null, newPath = null;
                switch (arguments.Length)
                {
                    //Rename the current folder
                    case 1:
                        oldPath = Folder;
                        newPath = System.IO.Path.GetDirectoryName(oldPath);
                        Folder = newPath;

                        newPath = $@"{newPath}\{arguments[0]}";
                        Write($"Renamed '{oldPath}' to '{newPath}'");
                        return;
                        Storage.Folder.Long.Move(oldPath, newPath);

                    //Rename the specified file or folder
                    case 2:
                        oldPath = Path(arguments[0]);
                        newPath = Path(arguments[1]);

                        Write($"Renamed '{oldPath}' to '{newPath}'");
                        return;
                        if (Storage.File.Long.Exists(oldPath))
                            Storage.File.Long.Move(oldPath, newPath);

                        else if (Storage.Folder.Long.Exists(oldPath))
                            Storage.Folder.Long.Move(oldPath, newPath);

                        else throw new ArgumentException($"The file or folder '{arguments[0]}' does not exist.");
                }
            }
            throw new ArgumentOutOfRangeException($"Only one or two arguments can be specified. If renaming the current folder, specify the new name only. If renaming a containing file or folder, specify the old and new name of the file or folder to be renamed separated by '{Separator}'.");
        }
    }
}