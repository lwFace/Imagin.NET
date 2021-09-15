using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    public abstract class MainViewModel<T> : ViewModel<T>, IMainViewModel where T : Window
    {
        public virtual string Title { get; }

        Window IMainViewModel.View => View;
        public override T View
        {
            get => base.View;
            set
            {
                base.View = value;
                View.If(i => i != null, i => i.DataContext = this);
            }
        }

        public MainViewModel() : base(null)
        {
            Get.New(GetType(), this);
        }

        ICommand exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                exitCommand = exitCommand ?? new RelayCommand(() => this.View.Close(), () => true);
                return exitCommand;
            }
        }
    }

    public abstract class MainViewModel<View, Document> : MainViewModel<View>, IDockViewModel where View : Window where Document : Models.Document
    {
        #region Events

        public event EventHandler<EventArgs<Content>> ActiveContentChanged;

        public event EventHandler<EventArgs<Document>> ActiveDocumentChanged;

        public event EventHandler<EventArgs<Panel>> ActivePanelChanged;

        public event EventHandler<EventArgs<Document>> DocumentAdded;

        public event DocumentClosedEventHandler DocumentClosed;

        #endregion

        #region Properties

        public DockView DockView { get; private set; }

        public Layouts Layouts => DockView.Layouts;

        Content activeContent = null;
        public Content ActiveContent
        {
            get => activeContent;
            set
            {
                this.Change(ref activeContent, value);
                OnActiveContentChanged(value);
            }
        }

        Document activeDocument = null;
        public Document ActiveDocument
        {
            get => activeDocument;
            private set
            {
                this.Change(ref activeDocument, value);
                OnActiveDocumentChanged(value);
            }
        }

        Panel activePanel = null;
        public Panel ActivePanel
        {
            get => activePanel;
            private set
            {
                this.Change(ref activePanel, value);
                OnActivePanelChanged(value);
            }
        }

        DocumentCollection documents = new DocumentCollection();
        public DocumentCollection Documents
        {
            get => documents;
            set => this.Change(ref documents, value);
        }

        PanelCollection panels = new PanelCollection();
        public PanelCollection Panels
        {
            get => panels;
            set => this.Change(ref panels, value);
        }

        #endregion

        #region MainViewModel

        public MainViewModel() : base()
        {
            Documents.Added += OnDocumentAdded;
            Documents.Inserted += OnDocumentAdded;
            Documents.Removed += OnDocumentRemoved;
            Documents.Closing += OnDocumentClosing;

            foreach (var i in Load())
                Panels.Add(i);
        }

        #endregion

        #region Commands

        ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                closeCommand = closeCommand ?? new RelayCommand(() => Documents.Remove(ActiveDocument), () => ActiveDocument != null);
                return closeCommand;
            }
        }

        ICommand closeAllCommand;
        public ICommand CloseAllCommand
        {
            get
            {
                closeAllCommand = closeAllCommand ?? new RelayCommand(() => Documents.Clear(), () => Documents.Count > 0);
                return closeAllCommand;
            }
        }

        ICommand saveAllCommand;
        public ICommand SaveAllCommand
        {
            get
            {
                saveAllCommand = saveAllCommand ?? new RelayCommand(() => Documents.Save(), () => Documents.Count > 0);
                return saveAllCommand;
            }
        }

        #endregion

        #region Methods

        void OnDocumentAdded(object sender, EventArgs<Models.Document> e)
        {
            ActiveContent = e.Value;
            OnDocumentAdded(e.Value as Document);
        }

        void OnDocumentRemoved(object sender, EventArgs<Models.Document> e) => OnDocumentClosed(e.Value as Document);

        void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (e.Document.IsModified)
            {
                var result = Dialog.Show("Close", "Are you sure you want to close?", DialogImage.Warning, DialogButtons.YesNo);
                e.Cancel = result == 1;
            }
        }

        ///-------------------------------------------------------------------

        protected virtual void OnActiveContentChanged(Content content)
        {
            if (content == null)
            {
                ActiveDocument = null;
                ActivePanel = null;
            }
            else if (content is Document document && ActiveDocument?.Equals(document) != true)
            {
                ActiveDocument = document;
            }
            else if (content is Panel panel && ActivePanel?.Equals(panel) != true)
                ActivePanel = panel;

            ActiveContentChanged?.Invoke(this, new EventArgs<Content>(content));
        }

        protected virtual void OnActiveDocumentChanged(Document document)
        {
            ActiveDocumentChanged?.Invoke(this, new EventArgs<Document>(document));
        }

        protected virtual void OnActivePanelChanged(Panel panel)
        {
            ActivePanelChanged?.Invoke(this, new EventArgs<Panel>(panel));
        }

        ///-------------------------------------------------------------------

        protected virtual void OnDocumentAdded(Document document) => DocumentAdded?.Invoke(this, new EventArgs<Document>(document));

        protected virtual void OnDocumentClosed(Document document)
        {
            DocumentClosed?.Invoke(this, new DocumentClosedEventArgs(document));
        }

        ///-------------------------------------------------------------------

        public abstract IEnumerable<Panel> Load();

        ///-------------------------------------------------------------------

        public object Panel(Type type) => Panels.First(x => x.GetType() == type);

        public Panel Panel<Panel>() where Panel : Models.Panel => Panels.First(i => i.GetType().Equals<Panel>()).As<Panel>();

        #endregion
    }
}