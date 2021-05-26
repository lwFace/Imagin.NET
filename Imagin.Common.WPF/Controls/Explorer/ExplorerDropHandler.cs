using GongSolutions.Wpf.DragDrop;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public sealed class ExplorerDropHandler : IDropTarget
    {
        public readonly Browser Browser;

        public readonly CrumbBar CrumbBar;

        public readonly Navigator Navigator;

        //....................................................................

        public ExplorerDropHandler(Browser browser) => Browser = browser;

        public ExplorerDropHandler(CrumbBar crumbBar) => CrumbBar = crumbBar;

        public ExplorerDropHandler(Navigator navigator) => Navigator = navigator;

        //....................................................................

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            var source = dropInfo.Data;
            var target = dropInfo.VisualTarget;

            if (From(source).Any())
            {
                if (Droppable(source, target))
                {
                    if (ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed())
                    {
                        dropInfo.Effects = DragDropEffects.Copy;
                    }
                    else dropInfo.Effects = DragDropEffects.Move;
                    return;
                }
            }

            dropInfo.Effects = DragDropEffects.None;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var source = dropInfo.Data;
            var target = dropInfo.VisualTarget;

            if (Droppable(source, target))
            {
                var items = From(source);

                int j = 0;
                foreach (var i in items)
                {
                    System.Console.WriteLine($"sourcePath ({j}) = {i.Path}");
                    j++;
                }

                var targetPath = TargetPath(target);
                System.Console.WriteLine($"targetPath = {targetPath}");
                if (targetPath == null)
                    return;

                if (ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed())
                {
                    Machine.Copy(items, targetPath);
                }
                else Machine.Move(items, targetPath);
            }
        }

        //....................................................................

        bool Droppable(object source, object target)
        {
            if (source != null)
            {
                if (target is ListView)
                    return true;

                if (target is FrameworkElement f)
                {
                    if (f.DataContext is Item item)
                    {
                        switch (item.Type)
                        {
                            case ItemType.Drive:
                                return false;

                            case ItemType.File:
                                return false;

                            case ItemType.Folder:
                                return true;

                            case ItemType.Shortcut:
                                return Shortcut.TargetsFolder(item.Path);
                        }
                    }
                    else if (f.DataContext is string path)
                        return Folder.Long.Exists(path);
                }
            }
            return false;
        }

        //....................................................................

        IEnumerable<Item> From(object source)
        {
            System.Console.WriteLine($"source = {(source?.GetType().FullName ?? "null")}");
            if (source is Item)
            {
                yield return (Item)source;
            }
            else
            if (source is IEnumerable<Item>)
            {
                if ((source as IEnumerable<Item>).Count() > 0)
                {
                    foreach (var i in (IEnumerable<Item>)source)
                        yield return i;
                }
            }
            else
            if (source is DataObject)
            {
                foreach (var i in From((DataObject)source))
                    yield return i;
            }
        }

        IEnumerable<Item> From(DataObject dataObject)
        {
            if (dataObject.ContainsFileDropList())
            {
                foreach (var i in dataObject.GetFileDropList())
                {
                    if (Folder.Long.Exists(i))
                    {
                        yield return new Folder(i);
                    }
                    else if (File.Long.Exists(i))
                        yield return new File(i);
                }
            }
        }

        //....................................................................

        string TargetPath(object target)
        {
            if (target is ListView)
                return Browser.Path;

            if (target is FrameworkElement f)
            {
                if (f.DataContext is Item i)
                {
                    return i.Path;
                }
                else if (f.DataContext is string s && Folder.Long.Exists(s))
                {
                    return f.DataContext.ToString();
                }
            }

            return null;
        }
    }
}