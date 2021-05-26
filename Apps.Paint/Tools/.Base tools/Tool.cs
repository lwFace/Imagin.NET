using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public abstract class Tool : Base, ISelectable
    {
        public const string LayerNotSelected = "A layer must be selected.";

        public const string InvalidLayer = "Layer doesn't support this type of operation";

        [field: NonSerialized]
        public event SelectedEventHandler Selected;

        [field: NonSerialized]
        protected Point? MouseDown;

        [field: NonSerialized]
        public Point? MouseDownAbsolute = null;

        [field: NonSerialized]
        protected Point? MouseMove;

        [field: NonSerialized]
        public Point? MouseMoveAbsolute = null;

        [field: NonSerialized]
        protected Point? MouseUp;

        [field: NonSerialized]
        public Point? MouseUpAbsolute;

        [field: NonSerialized]
        public LayerView LayerView = null;

        public abstract string Icon { get; }

        public virtual CursorPosition CursorPosition => CursorPosition.Origin;

        protected double DifferenceX => (MouseDown.Value.X - MouseMove.Value.X).Absolute().Round().Int32();

        protected double DifferenceY => (MouseDown.Value.Y - MouseMove.Value.Y).Absolute().Round().Int32();

        public int Quadrant
        {
            get
            {
                if (MouseDown.Value.X > MouseMove.Value.X && MouseDown.Value.Y > MouseMove.Value.Y)
                    return 0;

                if (MouseDown.Value.X < MouseMove.Value.X && MouseDown.Value.Y > MouseMove.Value.Y)
                    return 1;

                if (MouseDown.Value.X > MouseMove.Value.X && MouseDown.Value.Y < MouseMove.Value.Y)
                    return 2;

                if (MouseDown.Value.X < MouseMove.Value.X && MouseDown.Value.Y < MouseMove.Value.Y)
                    return 3;

                return -1;
            }
        }

        protected VisualLayer Layer => Get.Current<MainViewModel>().Panel<LayersPanel>().SelectedLayer as VisualLayer;

        protected LayerCollection Layers
        {
            get
            {
                return Get.Current<MainViewModel>().Panel<LayersPanel>().Layers;
            }
        }

        protected WriteableBitmap Bitmap => (Layer as PixelLayer).Pixels;

        protected MainViewModel MainViewModel => Get.Current<MainViewModel>();

        protected IEnumerable<Document> Documents => Get.Current<MainViewModel>().Documents.Select(i => i as Document);

        public virtual System.Windows.Input.Cursor Cursor { get; } = System.Windows.Input.Cursors.None;

        Document document;
        public Document Document
        {
            get => document;
            set
            {
                this.Change(ref document, value);
                OnDocumentChanged(value);
            }
        }

        public virtual bool Hidden => false;

        bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                this.Change(ref isSelected, value);
                if (value) OnSelected();
            }
        }

        bool showCompass = false;
        public bool ShowCompass
        {
            get => showCompass;
            set => this.Change(ref showCompass, value);
        }

        public Tool() : base() { }

        protected void Error(string text) => Dialog.Show(ToString(), text, DialogImage.Error, DialogButtons.Ok);

        protected bool AssertPixelLayer()
        {
            if (Layer == null)
            {
                Error(LayerNotSelected);
                return false;
            }
            else if (Layer is PixelLayer)
                return true;

            Error(InvalidLayer);
            return false;
        }

        protected virtual bool AssertLayer() => true;

        protected virtual void OnDocumentChanged(Document document) { }

        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new SelectedEventArgs(null));
        }

        public virtual bool OnMouseDown(Point point)
        {
            var result = AssertLayer();
            if (result)
            {
                MouseUp = null;
                MouseDown = point;
            }
            return result;
        }

        public virtual void OnMouseDoubleClick(Point point)
        {
        }

        public virtual void OnMouseMove(Point point)
        {
            MouseMove = point;
        }

        public virtual void OnMouseUp(Point point)
        {
            MouseUp = point;
            MouseDown = null;
            MouseMove = null;
        }

        public virtual void OnPreviewRendered(System.Windows.Media.DrawingContext input, double zoom) { }
    }
}