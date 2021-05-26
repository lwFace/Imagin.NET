using Imagin.Common.Collections;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class LayoutContentGroupControl : TabControl, ILayoutControl
    {
        public delegate void ActivatedEventHandler(LayoutContentGroupControl sender, Content content);

        public delegate void ItemsChangedEventHandler(LayoutContentGroupControl sender);

        /// ......................................................................................................................

        public event ActivatedEventHandler Activated;

        public event DragEventHandler DragStarted;

        public event ItemsChangedEventHandler ItemsChanged;

        /// ......................................................................................................................

        DragReference drag;

        /// ......................................................................................................................

        public DockView DockView { get; private set; }

        public LayoutRootControl Root { get; private set; }

        /// <summary>
        /// Never assume this always exists!
        /// </summary>
        public ICollect Source => ItemsSource as ICollect;

        /// ......................................................................................................................

        public static DependencyProperty ActiveProperty = DependencyProperty.Register(nameof(Active), typeof(bool), typeof(LayoutContentGroupControl), new FrameworkPropertyMetadata(false));
        public bool Active
        {
            get => (bool)GetValue(ActiveProperty);
            set => SetValue(ActiveProperty, value);
        }

        /// ......................................................................................................................

        public LayoutContentGroupControl(LayoutRootControl root) : base()
        {
            DockView = root.DockView;
            Root = root;
        }

        /// ......................................................................................................................

        Content[] CanDrag(DependencyObject input)
        {
            var a = input.GetParent<TabItem>();
            if (a == null)
            {
                var b = input.GetParent<LayoutPanelTitleControl>();
                if (b != null)
                    return Source.Where<Content>(i => i.CanFloat).ToArray();

                return null;
            }
            return Array<Content>.New(a.DataContext as Content);
        }

        /// ......................................................................................................................

        void OnItemsChanged(object sender) => OnItemsChanged();

        /// ......................................................................................................................

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (oldValue is ICollect a)
                a.Changed -= OnItemsChanged;

            if (newValue is ICollect b)
            {
                b.Changed -= OnItemsChanged;
                b.Changed += OnItemsChanged;
            }
            OnItemsChanged();
        }

        /// ......................................................................................................................

        internal void Activate()
        {
            DockView.EachGroup(i =>
            {
                i.SetCurrentValue(ActiveProperty, i.Equals(this));
                return true;
            });

            var content = SelectedItem as Content;
            Root.Activate(content);

            Activated?.Invoke(this, content);
        }

        /// ......................................................................................................................

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Activate();
            base.OnPreviewMouseLeftButtonDown(e);

            Content[] content = CanDrag(e.OriginalSource as DependencyObject);
            if (content?.Length > 0)
                drag = new DragReference(content, e.GetPosition(this));
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (drag != null)
            {
                if (e.GetPosition(this).Distance(drag.Start) > DockView.DragDistance)
                {
                    var content = drag.Content;
                    drag = null;
                    OnDragStarted(content);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            drag = null;
        }

        /// ......................................................................................................................

        protected virtual void OnDragStarted(Content[] content)
        {
            DragStarted?.Invoke(this, new DragEvent(Root, content, DockView.Float(content)));
        }

        protected virtual void OnItemsChanged()
        {
            ItemsChanged?.Invoke(this);
        }
    }
}