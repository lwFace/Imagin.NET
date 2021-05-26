using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_TextBox), Type = typeof(TextBox))]
    [TemplatePart(Name = nameof(PART_ToolBar), Type = typeof(ToolBar))]
    public class CrumbBar : ComboBox, IExplorer
    {
        #region Properties

        public event EventHandler<EventArgs<string>> PathChanged;

        TextBox PART_TextBox;

        ToolBar PART_ToolBar;

        public static DependencyProperty CrumbChildStyleProperty = DependencyProperty.Register(nameof(CrumbChildStyle), typeof(Style), typeof(CrumbBar), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style CrumbChildStyle
        {
            get => (Style)GetValue(CrumbChildStyleProperty);
            set => SetValue(CrumbChildStyleProperty, value);
        }

        public static DependencyProperty CrumbChildTemplateProperty = DependencyProperty.Register(nameof(CrumbChildTemplate), typeof(DataTemplate), typeof(CrumbBar), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate CrumbChildTemplate
        {
            get => (DataTemplate)GetValue(CrumbChildTemplateProperty);
            set => SetValue(CrumbChildTemplateProperty, value);
        }

        public static DependencyProperty CrumbsProperty = DependencyProperty.Register(nameof(Crumbs), typeof(StringCollection), typeof(CrumbBar), new FrameworkPropertyMetadata(default(StringCollection), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public StringCollection Crumbs
        {
            get => (StringCollection)GetValue(CrumbsProperty);
            set => SetValue(CrumbsProperty, value);
        }

        public static DependencyProperty DropHandlerProperty = DependencyProperty.Register(nameof(DropHandler), typeof(ExplorerDropHandler), typeof(CrumbBar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ExplorerDropHandler DropHandler
        {
            get => (ExplorerDropHandler)GetValue(DropHandlerProperty);
            private set => SetValue(DropHandlerProperty, value);
        }

        public static DependencyProperty EditableAddressProperty = DependencyProperty.Register(nameof(EditableAddress), typeof(string), typeof(CrumbBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string EditableAddress
        {
            get => (string)GetValue(EditableAddressProperty);
            set => SetValue(EditableAddressProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(CrumbBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPathChanged, OnPathCoerced));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((CrumbBar)i).OnPathChanged(new OldNew<string>(e));
        static object OnPathCoerced(DependencyObject i, object value) => Explorer.Validate(i, value?.ToString());

        #endregion

        #region CrumbBar

        public CrumbBar()
        {
            DefaultStyleKey = typeof(CrumbBar);
            SetCurrentValue(CrumbsProperty, new StringCollection());
            SetCurrentValue(DropHandlerProperty, new ExplorerDropHandler(this));
        }

        #endregion

        #region Methods

        void OnPathChanged(bool input)
        {
            IsEditable = input;
            if (!Storage.Folder.Long.Exists(EditableAddress))
            {
                Dialog.Show("Folder not found", $"The folder '{EditableAddress}' does not exist.", DialogImage.Error, DialogButtons.Ok);
                SetCurrentValue(EditableAddressProperty, Explorer.DefaultPath);
                return;
            }
            SetCurrentValue(EditableAddressProperty, Storage.Folder.Long.ActualPath(EditableAddress));
            SetCurrentValue(PathProperty, EditableAddress);
        }

        void OnToolBarPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource.As<DependencyObject>().GetParent<Crumb>();
            if (source != null)
                e.Handled = true;

            if (!e.Handled)
            {
                IsEditable = true;
                PART_TextBox.Focus();
            }
            else e.Handled = false;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SetCurrentValue(PathProperty, SelectedItem.ToString());
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_TextBox = Template.FindName(nameof(PART_TextBox), this).As<TextBox>();
            PART_TextBox.Entered += (s, e) => OnPathChanged(false);
            PART_TextBox.LostFocus += (s, e) => OnPathChanged(false);

            PART_ToolBar = Template.FindName(nameof(PART_ToolBar), this).As<ToolBar>();
            PART_ToolBar.PreviewMouseDown += OnToolBarPreviewMouseDown;
        }

        public virtual void OnPathChanged(OldNew<string> input)
        {
            SetCurrentValue(EditableAddressProperty, input.New);

            Crumbs.Clear();
            try
            {
                var i = System.IO.Path.GetDirectoryName(input.New);
                while (!i.NullOrEmpty())
                {
                    Crumbs.Insert(0, i);
                    i = System.IO.Path.GetDirectoryName(i);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Crumbs.Add(input.New);
            }
            PathChanged?.Invoke(this, new EventArgs<string>(input.New));
        }

        ICommand setPathCommand;
        public ICommand SetPathCommand
        {
            get
            {
                setPathCommand = setPathCommand ?? new RelayCommand<object>(p => SetCurrentValue(PathProperty, p.ToString()), p => !p.Null());
                return setPathCommand;
            }
        }

        #endregion
    }
}