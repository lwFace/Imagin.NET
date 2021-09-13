using Imagin.Common.Collections;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(Border), Type = typeof(Border))]
    public class DockView : Control, IPropertyChanged
    {
        #region Events

        public event EventHandler<EventArgs> LayoutChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        Window parentWindow = null;
        Window ParentWindow => parentWindow ?? (parentWindow = this.GetParent<Window>());

        /// <summary>
        /// The actual root element.
        /// </summary>
        Border Border;

        /// <summary>
        /// The root element.
        /// </summary>
        LayoutRootControl Root;

        /// ......................................................................................................................

        internal Handle handleActiveContent = false;

        DragEvent drag;
        public DragEvent Drag
        {
            get => drag;
            set
            {
                if (drag != null)
                    drag.Ended -= OnDragEnded;

                if (value != null)
                {
                    value.Ended -= OnDragEnded;
                    value.Ended += OnDragEnded;
                }

                this.Change(ref drag, value);
                SetCurrentValue(DraggingProperty, value != null);
                mouseHookListener.Enabled = Dragging;
            }
        }

        Layouts layouts = null;
        public Layouts Layouts => layouts;

        /// ......................................................................................................................

        internal Dictionary<Models.Panel, LayoutPanelGroupControl> Hidden = new Dictionary<Models.Panel, LayoutPanelGroupControl>();

        /// ......................................................................................................................

        internal List<LayoutDocumentGroupControl> DocumentGroups = new List<LayoutDocumentGroupControl>();

        internal List<LayoutPanelGroupControl> PanelGroups = new List<LayoutPanelGroupControl>();

        /// ......................................................................................................................

        List<LayoutWindowControl> Floating = new List<LayoutWindowControl>();

        /// ......................................................................................................................

        readonly HandleTask refresh;

        /// ......................................................................................................................

        public static DependencyProperty ActiveDocumentProperty = DependencyProperty.Register(nameof(ActiveDocument), typeof(Document), typeof(DockView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnActiveDocumentChanged));
        public Document ActiveDocument
        {
            get => (Document)GetValue(ActiveDocumentProperty);
            set => SetValue(ActiveDocumentProperty, value);
        }
        static void OnActiveDocumentChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnActiveDocumentChanged(e.NewValue as Document);

        public static DependencyProperty ActiveContentProperty = DependencyProperty.Register(nameof(ActiveContent), typeof(Content), typeof(DockView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnActiveContentChanged));
        public Content ActiveContent
        {
            get => (Content)GetValue(ActiveContentProperty);
            set => SetValue(ActiveContentProperty, value);
        }
        static void OnActiveContentChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnActiveContentChanged(e.NewValue as Content);

        public static DependencyProperty ActualPanelTemplateProperty = DependencyProperty.Register(nameof(ActualPanelTemplate), typeof(DataTemplate), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplate ActualPanelTemplate
        {
            get => (DataTemplate)GetValue(ActualPanelTemplateProperty);
            set => SetValue(ActualPanelTemplateProperty, value);
        }

        public static DependencyProperty DefaultProperty = DependencyProperty.Register(nameof(Default), typeof(bool), typeof(DockView), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnDefaultChanged));
        public bool Default
        {
            get => (bool)GetValue(DefaultProperty);
            set => SetValue(DefaultProperty, value);
        }
        static void OnDefaultChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnDefaultChanged(new OldNew<bool>(e));

        public static DependencyProperty DefaultLayoutProperty = DependencyProperty.Register(nameof(DefaultLayout), typeof(Uri), typeof(DockView), new FrameworkPropertyMetadata(default(Uri), OnDefaultLayoutChanged));
        public Uri DefaultLayout
        {
            get => (Uri)GetValue(DefaultLayoutProperty);
            set => SetValue(DefaultLayoutProperty, value);
        }
        static void OnDefaultLayoutChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnDefaultLayoutChanged(new OldNew<Uri>(e));

        public static DependencyProperty DocumentsProperty = DependencyProperty.Register(nameof(Documents), typeof(DocumentCollection), typeof(DockView), new FrameworkPropertyMetadata(null, OnDocumentsChanged));
        public DocumentCollection Documents
        {
            get => (DocumentCollection)GetValue(DocumentsProperty);
            set => SetValue(DocumentsProperty, value);
        }
        static void OnDocumentsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnDocumentsChanged(new OldNew<DocumentCollection>(e));

        public static DependencyProperty DocumentHeaderTemplateProperty = DependencyProperty.Register(nameof(DocumentHeaderTemplate), typeof(DataTemplate), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplate DocumentHeaderTemplate
        {
            get => (DataTemplate)GetValue(DocumentHeaderTemplateProperty);
            set => SetValue(DocumentHeaderTemplateProperty, value);
        }

        public static DependencyProperty DocumentStyleProperty = DependencyProperty.Register(nameof(DocumentStyle), typeof(Style), typeof(DockView), new FrameworkPropertyMetadata(null));
        public Style DocumentStyle
        {
            get => (Style)GetValue(DocumentStyleProperty);
            set => SetValue(DocumentStyleProperty, value);
        }

        public static DependencyProperty DocumentTemplateProperty = DependencyProperty.Register(nameof(DocumentTemplate), typeof(DataTemplate), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplate DocumentTemplate
        {
            get => (DataTemplate)GetValue(DocumentTemplateProperty);
            set => SetValue(DocumentTemplateProperty, value);
        }

        public static DependencyProperty DocumentTemplateSelectorProperty = DependencyProperty.Register(nameof(DocumentTemplateSelector), typeof(DataTemplateSelector), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector DocumentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(DocumentTemplateSelectorProperty);
            set => SetValue(DocumentTemplateSelectorProperty, value);
        }

        public static DependencyProperty DocumentTitleTemplateProperty = DependencyProperty.Register(nameof(DocumentTitleTemplate), typeof(DataTemplate), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplate DocumentTitleTemplate
        {
            get => (DataTemplate)GetValue(DocumentTitleTemplateProperty);
            set => SetValue(DocumentTitleTemplateProperty, value);
        }

        public static DependencyProperty DocumentTitleTemplateSelectorProperty = DependencyProperty.Register(nameof(DocumentTitleTemplateSelector), typeof(DataTemplateSelector), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector DocumentTitleTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(DocumentTitleTemplateSelectorProperty);
            set => SetValue(DocumentTitleTemplateSelectorProperty, value);
        }

        static DependencyProperty DragDistanceProperty = DependencyProperty.Register(nameof(DragDistance), typeof(double), typeof(DockView), new FrameworkPropertyMetadata(10.0));
        public double DragDistance
        {
            get => (double)GetValue(DragDistanceProperty);
            set => SetValue(DragDistanceProperty, value);
        }

        static DependencyProperty DraggingProperty = DependencyProperty.Register(nameof(Dragging), typeof(bool), typeof(DockView), new FrameworkPropertyMetadata(false));
        public bool Dragging
        {
            get => (bool)GetValue(DraggingProperty);
            private set => SetValue(DraggingProperty, value);
        }

        public static DependencyProperty LayoutProperty = DependencyProperty.Register(nameof(Layout), typeof(string), typeof(DockView), new FrameworkPropertyMetadata(default(string), OnLayoutChanged));
        public string Layout
        {
            get => (string)GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }
        static void OnLayoutChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnLayoutChanged(new OldNew<string>(e));
        
        public static DependencyProperty LayoutPathProperty = DependencyProperty.Register(nameof(LayoutPath), typeof(string), typeof(DockView), new FrameworkPropertyMetadata(default(string), OnLayoutPathChanged));
        public string LayoutPath
        {
            get => (string)GetValue(LayoutPathProperty);
            set => SetValue(LayoutPathProperty, value);
        }
        static void OnLayoutPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnLayoutPathChanged(new OldNew<string>(e));

        public static DependencyProperty PanelsProperty = DependencyProperty.Register(nameof(Panels), typeof(PanelCollection), typeof(DockView), new FrameworkPropertyMetadata(null, OnPanelsChanged));
        public PanelCollection Panels
        {
            get => (PanelCollection)GetValue(PanelsProperty);
            set => SetValue(PanelsProperty, value);
        }
        static void OnPanelsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<DockView>().OnPanelsChanged(new OldNew<PanelCollection>(e));

        public static DependencyProperty PanelHeaderTemplateProperty = DependencyProperty.Register(nameof(PanelHeaderTemplate), typeof(DataTemplate), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplate PanelHeaderTemplate
        {
            get => (DataTemplate)GetValue(PanelHeaderTemplateProperty);
            set => SetValue(PanelHeaderTemplateProperty, value);
        }

        public static DependencyProperty PanelStyleProperty = DependencyProperty.Register(nameof(PanelStyle), typeof(Style), typeof(DockView), new FrameworkPropertyMetadata(null));
        public Style PanelStyle
        {
            get => (Style)GetValue(PanelStyleProperty);
            set => SetValue(PanelStyleProperty, value);
        }

        public static DependencyProperty PanelTemplateProperty = DependencyProperty.Register(nameof(PanelTemplate), typeof(DataTemplate), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplate PanelTemplate
        {
            get => (DataTemplate)GetValue(PanelTemplateProperty);
            set => SetValue(PanelTemplateProperty, value);
        }

        public static DependencyProperty PanelTemplateSelectorProperty = DependencyProperty.Register(nameof(PanelTemplateSelector), typeof(DataTemplateSelector), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector PanelTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(PanelTemplateSelectorProperty);
            set => SetValue(PanelTemplateSelectorProperty, value);
        }

        public static DependencyProperty PanelTitleTemplateProperty = DependencyProperty.Register(nameof(PanelTitleTemplate), typeof(DataTemplate), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplate PanelTitleTemplate
        {
            get => (DataTemplate)GetValue(PanelTitleTemplateProperty);
            set => SetValue(PanelTitleTemplateProperty, value);
        }

        public static DependencyProperty PanelTitleTemplateSelectorProperty = DependencyProperty.Register(nameof(PanelTitleTemplateSelector), typeof(DataTemplateSelector), typeof(DockView), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector PanelTitleTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(PanelTitleTemplateSelectorProperty);
            set => SetValue(PanelTitleTemplateSelectorProperty, value);
        }

        #endregion

        #region DockView

        MouseHookListener mouseHookListener = new MouseHookListener(new Input.WinApi.GlobalHooker());

        public DockView() : base()
        {
            mouseHookListener.MouseMove += OnMouseMove;

            layouts = new Layouts();

            refresh = new HandleTask(Refresh);
            DefaultStyleKey = typeof(DockView);
        }

        void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Dragging)
            {
                if (Drag.Window != null)
                {
                    var position = Media.Display.Mouse;
                    Drag.Window.Left = position.X + 2;
                    Drag.Window.Top = position.Y + 2;
                }
            }
        }

        #endregion

        #region Methods

        #region Subscribe/Unsubscribe (internal)

        internal void Subscribe(LayoutDocumentGroupControl input)
        {
            Unsubscribe(input);
            input.Activated += OnContentActivated;
            input.DragStarted += OnDragStarted;
            input.ItemsChanged += OnItemsChanged;
            input.MouseEnter += OnContentMouseEnter;
            input.MouseLeave += OnContentMouseLeave;
            input.SelectionChanged += OnDocumentSelected;
        }

        internal void Unsubscribe(LayoutDocumentGroupControl input)
        {
            input.Activated -= OnContentActivated;
            input.DragStarted -= OnDragStarted;
            input.ItemsChanged -= OnItemsChanged;
            input.MouseEnter -= OnContentMouseEnter;
            input.MouseLeave -= OnContentMouseLeave;
            input.SelectionChanged -= OnDocumentSelected;
        }

        internal void Subscribe(LayoutPanelGroupControl input)
        {
            Unsubscribe(input);
            input.Activated += OnContentActivated;
            input.DragStarted += OnDragStarted;
            input.ItemsChanged += OnItemsChanged;
            input.MouseEnter += OnContentMouseEnter;
            input.MouseLeave += OnContentMouseLeave;
        }

        internal void Unsubscribe(LayoutPanelGroupControl input)
        {
            input.Activated -= OnContentActivated;
            input.DragStarted -= OnDragStarted;
            input.ItemsChanged -= OnItemsChanged;
            input.MouseEnter -= OnContentMouseEnter;
            input.MouseLeave -= OnContentMouseLeave;
        }

        internal void Subscribe(LayoutWindowControl input)
        {
            Unsubscribe(input);
            input.Activated += OnWindowActivated;
            input.Closed += OnWindowClosed;
            input.Closing += OnWindowClosing;
            input.DragStarted += OnDragStarted;
        }

        internal void Unsubscribe(LayoutWindowControl input)
        {
            input.Activated -= OnWindowActivated;
            input.Closed -= OnWindowClosed;
            input.DragStarted -= OnDragStarted;
        }

        #endregion

        #region Convert (ILayoutControl to ILayoutElement)

        LayoutElement Convert(LayoutGroupControl input)
        {
            var layoutGroup = new LayoutGroup(input.Orientation);

            var k = 0;
            foreach (var i in input.Children)
            {
                if (i is ILayoutControl j)
                {
                    layoutGroup.Elements.Add(Convert(j, input, k));
                    k += 2;
                }
            }

            return layoutGroup;
        }

        LayoutElement Convert(LayoutDocumentGroupControl input) => new LayoutDocumentGroup(input.Source)
        {
            Default = input.Default
        };

        LayoutElement Convert(LayoutPanelGroupControl input)
        {
            var result = new LayoutPanelGroup();
            foreach (var i in input.Items)
            {
                if (i is Models.Panel panel)
                    result.Panels.Add(new LayoutPanel(panel));
            }
            return result;
        }

        /// ......................................................................................................................

        public LayoutElement Convert(ILayoutControl input, LayoutGroupControl parent = null, int index = -1)
        {
            LayoutElement result = null;

            if (input is LayoutGroupControl layoutGroupControl)
                result = Convert(layoutGroupControl);

            if (input is LayoutDocumentGroupControl layoutDocumentGroupControl)
                result = Convert(layoutDocumentGroupControl);

            if (input is LayoutPanelGroupControl layoutPanelGroupControl)
                result = Convert(layoutPanelGroupControl);

            if (parent != null)
            {
                if (parent.Orientation == Orientation.Horizontal)
                {
                    var columnDefinition = parent.ColumnDefinitions.ElementAt(index);
                    result.MinWidth = columnDefinition.MinWidth;
                    result.Width = (LayoutLength)columnDefinition.Width;
                    result.MaxWidth = columnDefinition.MaxWidth;
                }
                if (parent.Orientation == Orientation.Vertical)
                {
                    var rowDefinition = parent.RowDefinitions.ElementAt(index);
                    result.MinHeight = rowDefinition.MinHeight;
                    result.Height = (LayoutLength)rowDefinition.Height;
                    result.MaxHeight = rowDefinition.MaxHeight;
                }
            }
            return result;
        }

        /// ......................................................................................................................

        public Layout Convert()
        {
            var layout = new Layout();
            layout.Root = (LayoutElement)Convert(Root.Child as ILayoutControl);
            foreach (var i in Floating)
            {
                var window = new LayoutWindow();
                window.Child = (LayoutElement)Convert(i.Content as ILayoutControl);
                window.Position = new Point2D(i.Left, i.Top);
                window.Size = new Math.DoubleSize(i.ActualHeight, i.ActualWidth);
                layout.Floating.Add(window);
            }

            layout.First<LayoutDocumentGroup>().Default = true;
            return layout;
        }

        #endregion

        #region Convert (ILayoutElement to ILayoutControl)

        /// <summary>
        /// Gets a <see cref="System.Windows.Controls.GridSplitter"/> with the given <see cref="Orientation"/>.
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        GridSplitter GridSplitter(Orientation orientation)
        {
            var result = new GridSplitter()
            {
                Background = System.Windows.Media.Brushes.Transparent,
                ResizeBehavior = GridResizeBehavior.PreviousAndNext,
                ShowsPreview = true
            };

            if (orientation == Orientation.Horizontal)
            {
                result.Height = double.NaN;
                result.ResizeDirection = GridResizeDirection.Columns;
                result.Width = 6;
            }
            else
            {
                result.Height = 6;
                result.ResizeDirection = GridResizeDirection.Rows;
                result.Width = double.NaN;
            }

            return result;
        }

        ILayoutControl Convert(LayoutRootControl root, LayoutElement input)
        {
            if (input is LayoutGroup layoutGroup)
                return Convert(root, layoutGroup);

            if (input is LayoutDocumentGroup layoutDocumentGroup)
                return Convert(root, layoutDocumentGroup);

            if (input is LayoutPanelGroup layoutPanelGroup)
                return Convert(root, layoutPanelGroup);

            return default;
        }

        LayoutGroupControl Convert(LayoutRootControl root, LayoutGroup input)
        {
            var result = new LayoutGroupControl(root, input.Orientation);

            var j = 0;
            foreach (var i in input.Elements)
            {
                var child = Convert(root, i);
                if (child == null)
                    continue;

                if (input.Orientation == Orientation.Horizontal)
                {
                    var columnDefinition = new ColumnDefinition();
                    if (!double.IsNaN(i.MinWidth))
                        columnDefinition.MinWidth = i.MinWidth;

                    columnDefinition.Width = (LayoutLength)i.Width;

                    if (!double.IsNaN(i.MaxWidth))
                        columnDefinition.MaxWidth = i.MaxWidth;

                    result.ColumnDefinitions.Add(columnDefinition);
                    Grid.SetColumn(child as UIElement, j * 2);

                }
                if (input.Orientation == Orientation.Vertical)
                {
                    var rowDefinition = new RowDefinition();
                    if (!double.IsNaN(i.MinHeight))
                        rowDefinition.MinHeight = i.MinHeight;

                    rowDefinition.Height = (LayoutLength)i.Height;

                    if (!double.IsNaN(i.MaxHeight))
                        rowDefinition.MaxHeight = i.MaxHeight;

                    result.RowDefinitions.Add(rowDefinition);
                    Grid.SetRow(child as UIElement, j * 2);
                }
                result.Children.Add(child as UIElement);

                GridSplitter gridSplitter = null;
                if (j < input.Elements.Count - 1)
                {
                    gridSplitter = GridSplitter(input.Orientation);
                    if (input.Orientation == Orientation.Horizontal)
                    {
                        result.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                        Grid.SetColumn(gridSplitter, (j * 2) + 1);
                    }
                    if (input.Orientation == Orientation.Vertical)
                    {
                        result.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        Grid.SetRow(gridSplitter, (j * 2) + 1);
                    }
                    result.Children.Add(gridSplitter);
                }

                j++;
            }
            return result;
        }

        LayoutDocumentGroupControl Convert(LayoutRootControl root, LayoutDocumentGroup input)
        {
            var result = new LayoutDocumentGroupControl(root, input.Default) { ItemContainerStyle = DocumentStyle, ItemTemplate = DocumentHeaderTemplate };

            var documents = new Collections.Generic.Collection<Document>();
            if (input.Default)
            {
                Documents.If(i => i?.Any<Document>() == true, i => i.ForEach(j => documents.Add(j)));
            }
            else input.Documents?.ForEach(i => documents.Add(i));

            result.ItemsSource = documents;
            DocumentGroups.Add(result);

            Subscribe(result);
            return result;
        }

        LayoutPanelGroupControl Convert(LayoutRootControl root, LayoutPanelGroup input)
        {
            var result = new LayoutPanelGroupControl(root);
            result.Bind(ItemsControl.ItemContainerStyleProperty, nameof(PanelStyle), this);
            result.Bind(ItemsControl.ItemTemplateProperty, nameof(PanelHeaderTemplate), this);

            var children = new PanelCollection();

            int index = 0, x = 0;
            foreach (var i in input.Panels)
            {
                foreach (var j in Panels)
                {
                    if (j.Name == i.Name && j.IsVisible)
                    {
                        children.Add(j);
                        if (i.IsSelected)
                            index = x;

                        x++;
                        break;
                    }
                }
            }

            result.ItemsSource = children;
            result.SelectedIndex = index;

            PanelGroups.Add(result);

            Subscribe(result);
            return result;
        }

        LayoutWindowControl Convert(LayoutWindow input, WindowStartupLocation location = WindowStartupLocation.Manual)
        {
            LayoutWindowControl result = new LayoutWindowControl(this);

            result.Buttons.Add(new WindowButton()
            {
                Command = DragCommand,
                CommandParameter = result,
                Content = new BitmapImage(Common.Resources.Uri("Imagin.Common.WPF", "Images/Scroll.png")),
                ToolTip = "Drag"
            });
            result.Buttons.Add(new WindowButton()
            {
                Command = DockCommand,
                CommandParameter = result,
                Content = new BitmapImage(Common.Resources.Uri("Imagin.Common.WPF", "Images/Pin.png")),
                ToolTip = "Dock"
            });

            result.WindowStartupLocation = location;
            result.Show();

            var root = new LayoutRootControl(this, result);
            root.Child = Convert(root, input.Child);
            result.Content = root;

            if (input.Position != null)
            {
                result.Left = input.Position.X;
                result.Top = input.Position.Y;
            }

            if (input.Size != null)
            {
                result.Height = input.Size.Height;
                result.Width = input.Size.Width;
            }

            ParentWindow.InputBindings.ForEach(i => result.InputBindings.Add(i as InputBinding));

            Subscribe(result);
            Floating.Add(result);
            return result;
        }

        public void Convert(Layout layout)
        {
            if (Root != null)
            {
                if (layout != null)
                {
                    EachGroup(i =>
                    {
                        if (i is LayoutDocumentGroupControl a)
                            Unsubscribe(a);

                        if (i is LayoutPanelGroupControl b)
                            Unsubscribe(b);

                        return true;
                    });

                    for (var i = Floating.Count - 1; i >= 0; i--)
                        Floating[i].Close(true);

                    DocumentGroups.Clear();
                    PanelGroups.Clear();

                    Floating.Clear();
                    Hidden.Clear();

                    Root.Child = Convert(Root, layout.Root);
                    foreach (var i in layout.Floating)
                        Convert(i);

                    if (DocumentGroups.Count > 0 && !DocumentGroups.Select(i => i.Default).Contains(true))
                        DocumentGroups.First<LayoutDocumentGroupControl>().Default = true;

                    foreach (var i in PanelGroups.Where(i => i.Source.Count == 0))
                        i.Remove();
                }
            }
        }

        #endregion

        #region Private (handlers)

        void OnContentActivated(LayoutContentGroupControl sender, Content content)
        {
            handleActiveContent.InvokeIfFalse(() => SetCurrentValue(ActiveContentProperty, content));
        }

        /// ......................................................................................................................

        void OnDocumentAdded(object sender, EventArgs<Document> e)
        {
            var documents = DocumentGroups.First<LayoutDocumentGroupControl>();
            if (documents != null)
            {
                documents.Source.Add(e.Value);
                documents.Activate();
                return;
            }
            //...
            Dock(Root, Docks.Left, null, e.Value);
        }

        void OnDocumentRemoved(object sender, EventArgs<Document> e)
        {
            Find(e.Value)?.Source.Remove(e.Value);
        }

        void OnDocumentSelected(object sender, SelectionChangedEventArgs e)
        {
            var document = e.AddedItems.First() as Document;
            if (document != null || Documents.Count == 0)
                handleActiveContent.InvokeIfFalse(() => SetCurrentValue(ActiveContentProperty, document));
        }

        /// ......................................................................................................................

        void OnContentMouseEnter(object sender, MouseEventArgs e) => Mark(sender as LayoutContentGroupControl);

        void OnContentMouseLeave(object sender, MouseEventArgs e) { }

        /// ......................................................................................................................

        void OnDragEnded(object sender, EventArgs<bool> e)
        {
            if (e.Value)
                Drag.Window.Close(true);

            Drag = null;
        }

        void OnDragStarted(object sender, DragEvent e)
        {
            Drag = e;
            if (sender is LayoutContentGroupControl c)
            {
                if (c?.Source?.Count > 0)
                    Mark(c);
            }
        }

        /// ......................................................................................................................

        void OnItemsChanged(LayoutContentGroupControl sender)
        {
            if (sender.Source?.Count == 0)
            {
                if (sender is ILayoutControl i)
                {
                    if (i is LayoutDocumentGroupControl j)
                    {
                        //1. Are we allowed to remove it?
                        if (j.Default)
                            return;
                    }

                    var window = i.Root.Window;
                    i.Remove();

                    if (window != null)
                    {
                        if (window.Root.Child == null)
                            window.Close(true);
                    }
                }
            }
        }

        /// ......................................................................................................................

        void Hide(Models.Panel panel)
        {
            var panels = Find(panel);
            if (panels != null)
            {
                Hidden.Add(panel, panels);
                panels.Source.Remove(panel);
            }
        }

        void Show(Models.Panel panel)
        {
            var panels = Find(panel);
            //It already exists, somehow...
            if (panels != null)
                return;

            //Check if we have a reference to the panel's previous home
            if (Hidden.ContainsKey(panel))
            {
                panels = Hidden[panel];
                Hidden.Remove(panel);
            }

            //If a reference to the previous home does not exist, the previous home was removed for one reason or another (irrelevant); try getting the first available home!
            panels = panels ?? PanelGroups.FirstOrDefault();
            if (panels != null)
            {
                panels.Source.Add(panel);
                return;
            }
            //...
            //If no home is available, create a new one!
            Dock(Root, Docks.Right, null, Array<Content>.New(panel));
        }

        /// ......................................................................................................................

        void OnPanelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var panel = sender as Models.Panel;
            switch (e.PropertyName)
            {
                case nameof(Models.Panel.IsVisible):
                    if (panel.IsVisible)
                        Show(panel);

                    else if (panel.CanHide)
                    {
                        Hide(panel);
                    }
                    else panel.IsVisible = true;
                    break;
            }
        }

        void OnPanelsChanged(object sender)
        {
            Subscribe(Panels);
        }

        /// ......................................................................................................................

        void OnParentWindowClosing(object sender, CancelEventArgs e)
        {
            if (Documents.IsModified)
            {
                var result = Dialog.Show("Close", "One are more documents have not been saved. Are you sure you want to close?", DialogImage.Warning, DialogButtons.YesNo);
                e.Cancel = result == 1;
            }

            if (!e.Cancel)
            {
                //In the future, get the current layout and store it
                //Imagin.Configuration.Data.AutoSaveLayout = true will then obtain that using Get.Current<DockView>().LastLayout
                for (var i = Floating.Count - 1; i >= 0; i--)
                    Floating[i].Close(true);
            }
        }

        void OnParentWindowMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Dragging)
                Drag.End(false);
        }

        /// ......................................................................................................................

        void OnWindowActivated(object sender, EventArgs e)
        {
            handleActiveContent.InvokeIfFalse(() =>
            {
                var root = (sender as LayoutWindowControl)?.Root ?? Root;
                if (root.ActiveContent != null)
                {
                    var control = Find(root.ActiveContent);
                    if (control != null)
                    {
                        SetCurrentValue(ActiveContentProperty, control.SelectedItem as Content);
                        control.Activate();
                        return;
                    }
                }

                bool result = false;
                EachGroup(j =>
                {
                    if (j.Root.Equals(root))
                    {
                        if (j.Active)
                        {
                            SetCurrentValue(ActiveContentProperty, j.SelectedItem as Content);
                            j.Activate();

                            result = true;
                            return false;
                        }
                    }
                    return true;
                });

                if (!result)
                {
                    EachGroup(j =>
                    {
                        if (j.Root.Equals(root))
                        {
                            SetCurrentValue(ActiveContentProperty, j.SelectedItem as Content);
                            j.Activate();
                            return false;
                        }
                        return true;
                    });
                }
            });
        }

        void OnWindowClosed(object sender, EventArgs e)
        {
            if (sender is LayoutWindowControl i)
            {
                Unsubscribe(i);
                Floating.Remove(i);
            }
        }

        void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (sender is LayoutWindowControl window)
            {
                window.Root.Child.EachControl<LayoutDocumentGroupControl>(i => DocumentGroups.Remove(i));
                window.Root.Child.EachControl<LayoutPanelGroupControl>(i => PanelGroups.Remove(i));
            }
        }

        #endregion

        #region Private (other)

        internal void Dock(LayoutRootControl root, Docks docks, ILayoutControl a, object b)
        {
            if (b is Content[] c)
                Dock(root, docks, a, c);

            else if (b is LayoutElement d)
                Dock(root, docks, a, d);

            else throw new NotSupportedException();
        }

        internal void Dock(LayoutRootControl root, Docks docks, ILayoutControl a, Content[] b)
        {
            LayoutElement layout = b.First<Content>() is Document d
                ? (LayoutElement)new LayoutDocumentGroup(b.Cast<Document>())
                : new LayoutPanelGroup(b.Cast<Models.Panel>());

            Dock(root, docks, a, layout);
        }

        internal void Dock(LayoutRootControl root, Docks docks, ILayoutControl a, LayoutElement b)
        {
            LayoutGroupControl result = null;
            switch (docks)
            {
                case Docks.Left:
                case Docks.Right:
                    result = new LayoutGroupControl(root, Orientation.Horizontal);
                    break;
                case Docks.Top:
                case Docks.Bottom:
                    result = new LayoutGroupControl(root, Orientation.Vertical);
                    break;
            }

            ILayoutControl aElement = a ?? root.Child;
            ILayoutControl bElement = Convert(root, b);

            if (aElement == null)
            {
                if (a == null)
                    root.Child = bElement;

                return;
            }

            var gridSplitter = GridSplitter(result.Orientation);

            if (a == null)
                root.Child = null;

            var index = a?.GetIndex();
            var parent = a?.GetParent();

            if (a is ILayoutControl)
                parent.Children.RemoveAt(index.Value);

            switch (docks)
            {
                case Docks.Left:
                    result.Children.Add(bElement as UIElement);
                    result.Children.Add(gridSplitter);
                    result.Children.Add(aElement as UIElement);
                    break;
                case Docks.Top:
                    result.Children.Add(bElement as UIElement);
                    result.Children.Add(gridSplitter);
                    result.Children.Add(aElement as UIElement);
                    break;
                case Docks.Right:
                    result.Children.Add(aElement as UIElement);
                    result.Children.Add(gridSplitter);
                    result.Children.Add(bElement as UIElement);
                    break;
                case Docks.Bottom:
                    result.Children.Add(aElement as UIElement);
                    result.Children.Add(gridSplitter);
                    result.Children.Add(bElement as UIElement);
                    break;
            }

            GridLength[] lengths = new GridLength[3]
            {
                new GridLength(1, GridUnitType.Star),
                GridLength.Auto,
                new GridLength(1, GridUnitType.Star)
            };

            for (var i = 0; i < 3; i++)
            {
                if (result.Orientation == Orientation.Horizontal)
                {
                    result.ColumnDefinitions.Add(new ColumnDefinition() { Width = lengths[i] });
                    Grid.SetColumn(result.Children[i], i);
                }
                else
                {
                    result.RowDefinitions.Add(new RowDefinition() { Height = lengths[i] });
                    Grid.SetRow(result.Children[i], i);
                }
            }

            if (a == null)
                root.Child = result;

            if (a is ILayoutControl)
            {
                parent.Children.Insert(index.Value, result);
                if (parent.Orientation == Orientation.Horizontal)
                    Grid.SetColumn(result, index.Value);

                if (parent.Orientation == Orientation.Vertical)
                    Grid.SetRow(result, index.Value);
            }
        }

        internal bool DockCenter(DragEvent e)
        {
            if (e.MouseOver is LayoutDocumentGroupControl documents)
            {
                if (e.Content?.First<Content>() is Document)
                {
                    foreach (Document i in e.Content)
                        documents.Source.Add(i);

                    return true;
                }
            }
            if (e.MouseOver is LayoutPanelGroupControl panels)
            {
                if (e.Content?.First<Content>() is Models.Panel)
                {
                    foreach (Models.Panel i in e.Content)
                        panels.Source.Add(i);

                    panels.SelectedIndex = panels.Source.Count - 1;
                    return true;
                }
            }
            return false;
        }

        /// ......................................................................................................................

        internal void EachGroup(Predicate<LayoutContentGroupControl> @continue)
        {
            foreach (var i in DocumentGroups)
            {
                if (!@continue(i))
                    return;
            }
            foreach (var i in PanelGroups)
            {
                if (!@continue(i))
                    return;
            }
        }

        /// ......................................................................................................................

        LayoutContentGroupControl Find(Content input) => (LayoutContentGroupControl)Find(input as Document) ?? Find(input as Models.Panel);

        /// ......................................................................................................................

        LayoutDocumentGroupControl Find(Document input)
        {
            foreach (var i in DocumentGroups)
            {
                foreach (var j in i.Source)
                {
                    if (j.Equals(input))
                        return i;
                }
            }
            return null;
        }

        LayoutPanelGroupControl Find(Models.Panel input)
        {
            foreach (var i in PanelGroups)
            {
                foreach (var j in i.Source)
                {
                    if (j == input)
                        return i;
                }
            }
            return null;
        }

        /// ......................................................................................................................

        internal LayoutWindowControl Float(Content[] i)
        {
            if (i?.Length > 0)
            {
                var result = new LayoutWindow();
                if (i?.First<Content>() is Document firstDocument)
                {
                    result.Child = new LayoutDocumentGroup(i.Cast<Document>());

                    var documents = Find(firstDocument);
                    result.Size = new Math.DoubleSize(documents.ActualHeight, documents.ActualWidth);

                    foreach (Document j in i)
                        documents.Source.Remove(j);
                }
                if (i?.First<Content>() is Models.Panel firstPanel)
                {
                    result.Child = new LayoutPanelGroup(i.Cast<Models.Panel>());

                    var panels = Find(firstPanel);
                    result.Size = new Math.DoubleSize(panels.ActualHeight, panels.ActualWidth);

                    foreach (Models.Panel j in i)
                        panels.Source.Remove(j);
                }
                return Convert(result, WindowStartupLocation.CenterScreen);
            }
            return null;
        }

        /// ......................................................................................................................

        void Mark(LayoutContentGroupControl input)
        {
            if (Dragging)
            {
                if (input != null)
                {
                    Drag.MouseOver = input;
                    Drag.MousePosition = input.GetPosition();
                    Canvas.SetLeft(input.Root.SecondaryMarkers, Drag.MousePosition.X - 50);
                    Canvas.SetTop(input.Root.SecondaryMarkers, Drag.MousePosition.Y - 50);
                }
            }
        }

        /// ......................................................................................................................

        async Task Refresh()
        {
            if (DefaultLayout != null)
            {
                var result = await Layouts.Apply(Layout, DefaultLayout);
                if (result != null)
                {
                    Convert(result);
                    LayoutChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// ......................................................................................................................

        void Subscribe(PanelCollection input)
        {
            foreach (var i in input)
            {
                i.PropertyChanged -= OnPanelPropertyChanged;
                i.PropertyChanged += OnPanelPropertyChanged;
            }
        }

        #endregion

        #region Protected

        protected virtual void OnActiveContentChanged(Content input)
        {
            if (!handleActiveContent)
            {
                handleActiveContent.Invoke(() =>
                {
                    var control = Find(input);
                    if (control != null)
                    {
                        if (control.Root.Window is LayoutWindowControl window)
                            window.Activate();

                        control.SelectedIndex = control.Items.IndexOf(input);
                        control.Activate();
                    }
                });
            }
            else handleActiveContent.Invoke(() => input.If(i => i is Document, i => SetCurrentValue(ActiveDocumentProperty, i as Document)));
        }

        protected virtual void OnActiveDocumentChanged(Document input)
        {
            if (!handleActiveContent)
                SetCurrentValue(ActiveContentProperty, input);
        }

        protected virtual void OnDefaultChanged(OldNew<bool> input)
        {
            if (input.New)
                Get.New<DockView>(this);
        }

        protected virtual void OnDefaultLayoutChanged(OldNew<Uri> input)
        {
            _ = refresh.Invoke();
        }

        protected virtual void OnDocumentsChanged(OldNew<DocumentCollection> input)
        {
            if (input.Old != null)
            {
                input.Old.Added -= OnDocumentAdded;
                input.Old.Removed -= OnDocumentRemoved;
            }
            if (input.New != null)
            {
                input.New.Added -= OnDocumentAdded;
                input.New.Removed -= OnDocumentRemoved;

                input.New.Added += OnDocumentAdded;
                input.New.Removed += OnDocumentRemoved;
            }
            _ = refresh.Invoke();
        }

        protected virtual void OnLayoutChanged(OldNew<string> input)
        {
            _ = refresh.Invoke();
        }

        protected virtual void OnLayoutPathChanged(OldNew<string> input)
        {
            _ = layouts.RefreshAsync(input.New);
        }
        
        protected virtual void OnPanelsChanged(OldNew<PanelCollection> input)
        {
            if (input.Old != null)
            {
                input.Old.Changed -= OnPanelsChanged;
                input.Old.ForEach(i => i.PropertyChanged -= OnPanelPropertyChanged);
            }
            if (input.New != null)
            {
                input.New.Changed -= OnPanelsChanged;
                input.New.Changed += OnPanelsChanged;
                Subscribe(input.New);
            }
            _ = refresh.Invoke();
        }

        #endregion

        #region Public 

        public override void OnApplyTemplate()
        {
            ParentWindow.Activated += OnWindowActivated;
            ParentWindow.Closing += OnParentWindowClosing;
            ParentWindow.MouseLeftButtonUp += OnParentWindowMouseLeftButtonUp;

            Border = Template.FindName(nameof(Border), this) as Border;
            Border.Child = new LayoutRootControl(this, null);
            Root = Border.Child as LayoutRootControl;
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Commands
        
        ICommand closeDocumentCommand;
        public ICommand CloseDocumentCommand
        {
            get
            {
                closeDocumentCommand = closeDocumentCommand ?? new RelayCommand<Document>(i => Documents.RemoveAt(Documents.IndexOf(i)), i => i?.CanClose == true);
                return closeDocumentCommand;
            }
        }

        ICommand closeAllDocumentsCommand;
        public ICommand CloseAllDocumentsCommand
        {
            get
            {
                closeAllDocumentsCommand = closeAllDocumentsCommand ?? new RelayCommand(() => Documents.Clear(), () => Documents.Any<Document>());
                return closeAllDocumentsCommand;
            }
        }

        ICommand closeAllDocumentsButThisCommand;
        public ICommand CloseAllDocumentsButThisCommand
        {
            get
            {
                closeAllDocumentsButThisCommand = closeAllDocumentsButThisCommand ?? new RelayCommand<Document>(i =>
                {
                    for (var j = Documents.Count - 1; j >= 0; j--)
                    {
                        if (!i.Equals(Documents[j]))
                            Documents.RemoveAt(j);
                    }
                }, 
                i => i != null);
                return closeAllDocumentsButThisCommand;
            }
        }
        
        ICommand dockCommand;
        public ICommand DockCommand
        {
            get
            {
                dockCommand = dockCommand ?? new RelayCommand<LayoutWindowControl>(i =>
                {
                    Dock(Root, Docks.Left, null, Convert(i.Root.Child));
                    i.Close(true);
                }, 
                i => i != null);
                return dockCommand;
            }
        }

        ICommand dragCommand;
        public ICommand DragCommand
        {
            get
            {
                dragCommand = dragCommand ?? new RelayCommand<LayoutWindowControl>(i => i.Drag(), i => !Dragging && i != null);
                return dragCommand;
            }
        }
        
        ICommand hidePanelCommand;
        public ICommand HidePanelCommand
        {
            get
            {
                hidePanelCommand = hidePanelCommand ?? new RelayCommand<Models.Panel>(i => i.IsVisible = false, i => i?.CanHide == true && i.IsVisible);
                return hidePanelCommand;
            }
        }
        
        ICommand floatCommand;
        public ICommand FloatCommand
        {
            get
            {
                floatCommand = floatCommand ?? new RelayCommand<Content>(i => Float(Array<Content>.New(i)), i => i?.CanFloat == true);
                return floatCommand;
            }
        }

        ICommand floatAllCommand;
        public ICommand FloatAllCommand
        {
            get
            {
                floatAllCommand = floatAllCommand ?? new RelayCommand<Content>(i =>
                {
                    var documents = Find(i as Document).Source?.Where<Document>(j => j.CanFloat).ToArray();
                    Float(documents);
                }, 
                i =>
                {
                    //If at least 1 can float...
                    if (i is Document j)
                    {
                        var documents = Find(j);
                        if (documents?.Source is ICollect k)
                        {
                            foreach (Document l in k)
                            {
                                if (l.CanFloat)
                                    return true;
                            }
                        }
                    }
                    return false;
                });
                return floatAllCommand;
            }
        }

        #endregion

        #endregion
    }
}