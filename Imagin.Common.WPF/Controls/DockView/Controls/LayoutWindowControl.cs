using Imagin.Common.Models;
using System.ComponentModel;
using System.Windows;

namespace Imagin.Common.Controls
{
    public sealed class LayoutWindowControl : BaseWindow
    {
        public event DragEventHandler DragStarted;

        readonly DockView DockView;

        public LayoutRootControl Root => Content as LayoutRootControl;

        bool closing = false;

        public LayoutWindowControl(DockView dockView) : base()
        {
            DockView = dockView;
            SetCurrentValue(PaddingProperty, new Thickness(5));
        }

        void OnDragStarted(DragEvent e) => DragStarted?.Invoke(this, e);

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!closing)
            {
                //The order here is important!
                Root.Child.Each<Panel>(i => i.IsVisible = false);
                Root.Child.Each<Document>(i => e.Cancel = !DockView.Documents.Remove(i));
            }
        }

        public void Close(bool handle)
        {
            closing = handle;
            Close();
        }

        public void Drag() => OnDragStarted(new DragEvent(Root, null, this));
    }
}