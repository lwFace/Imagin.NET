using Imagin.Common.Input;
using Imagin.Common.Models;
using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class DragEvent : Base
    {
        public event EventHandler<EventArgs<bool>> Ended;

        public object ActualContent => Content == null ? (object)Root.DockView.Convert(Window.Root.Child) : Content;

        public readonly Content[] Content;

        public readonly LayoutWindowControl Window;

        ILayoutControl mouseOver = null;
        public ILayoutControl MouseOver
        {
            get => mouseOver;
            set => this.Change(ref mouseOver, value);
        }

        public Point MousePosition { get; set; }

        public readonly LayoutRootControl Root;

        public DragEvent(LayoutRootControl root, Content[] content, LayoutWindowControl window)
        {
            Root = root;
            Content = content;
            Window = window;
        }

        public void End(bool result)
        {
            Ended?.Invoke(this, new EventArgs<bool>(result));
        }
    }
}