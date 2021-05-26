using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// <see cref="ControlTemplate"/> defined at: <see langword="/Styles/PropertyGrid.xaml"/>.
    /// </summary>
    public class PropertyGrid : DataGrid
    {
        #region public class <Element, Child, Owner>

        public class Element
        {
            public readonly string Name;

            public readonly object Value;

            public Element(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }

        public class Child : Element
        {
            public Child(string name, object value) : base(name, value) { }
        }

        public class Owner : Element
        {
            public Owner(string name, object value) : base(name, value) { }
        }

        #endregion

        #region Fields

        public event EventHandler<EventArgs<object>> SourceChanged;

        //...........................................................................................

        bool loadedOnce = false;

        Handle handle = false;

        //...........................................................................................

        DataGridTemplateColumn NameColumn;

        DataGridTemplateColumn ValueColumn;

        //...........................................................................................

        Queue<OldNew> sourceChanges = new Queue<OldNew>();

        internal bool sourceChanging = false;

        #endregion

        #region Properties

        #region DefaultCategoryName

        public static DependencyProperty DefaultCategoryNameProperty = DependencyProperty.Register(nameof(DefaultCategoryName), typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata("General", FrameworkPropertyMetadataOptions.None));
        public string DefaultCategoryName
        {
            get => (string)GetValue(DefaultCategoryNameProperty);
            set => SetValue(DefaultCategoryNameProperty, value);
        }

        #endregion

        #region DefaultGroupStyle

        public static DependencyProperty DefaultGroupStyleProperty = DependencyProperty.Register(nameof(DefaultGroupStyle), typeof(GroupStyle), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public GroupStyle DefaultGroupStyle
        {
            get => (GroupStyle)GetValue(DefaultGroupStyleProperty);
            set => SetValue(DefaultGroupStyleProperty, value);
        }

        #endregion

        #region DescriptionBorderStyle

        public static DependencyProperty DescriptionBorderStyleProperty = DependencyProperty.Register(nameof(DescriptionBorderStyle), typeof(Style), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Style DescriptionBorderStyle
        {
            get => (Style)GetValue(DescriptionBorderStyleProperty);
            set => SetValue(DescriptionBorderStyleProperty, value);
        }

        #endregion

        #region DescriptionResizeMode

        public static DependencyProperty DescriptionResizeModeProperty = DependencyProperty.Register(nameof(DescriptionResizeMode), typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnDescriptionResizeModeChanged));
        public bool DescriptionResizeMode
        {
            get => (bool)GetValue(DescriptionResizeModeProperty);
            set => SetValue(DescriptionResizeModeProperty, value);
        }
        static void OnDescriptionResizeModeChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnDescriptionResizeModeChanged(new OldNew<bool>(e));

        #endregion

        #region DescriptionStringFormat

        public static DependencyProperty DescriptionStringFormatProperty = DependencyProperty.Register(nameof(DescriptionStringFormat), typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string DescriptionStringFormat
        {
            get => (string)GetValue(DescriptionStringFormatProperty);
            set => SetValue(DescriptionStringFormatProperty, value);
        }

        #endregion

        #region DescriptionTemplate

        public static DependencyProperty DescriptionTemplateProperty = DependencyProperty.Register(nameof(DescriptionTemplate), typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate DescriptionTemplate
        {
            get => (DataTemplate)GetValue(DescriptionTemplateProperty);
            set => SetValue(DescriptionTemplateProperty, value);
        }

        #endregion

        #region DescriptionTemplateSelector

        public static DependencyProperty DescriptionTemplateSelectorProperty = DependencyProperty.Register(nameof(DescriptionTemplateSelector), typeof(DataTemplateSelector), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplateSelector DescriptionTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(DescriptionTemplateSelectorProperty);
            set => SetValue(DescriptionTemplateSelectorProperty, value);
        }

        #endregion

        #region DescriptionVisibility

        public static DependencyProperty DescriptionVisibilityProperty = DependencyProperty.Register(nameof(DescriptionVisibility), typeof(Visibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.None, OnDescriptionVisibilityChanged));
        public Visibility DescriptionVisibility
        {
            get => (Visibility)GetValue(DescriptionVisibilityProperty);
            set => SetValue(DescriptionVisibilityProperty, value);
        }
        static void OnDescriptionVisibilityChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnDescriptionVisibilityChanged(new OldNew<Visibility>(e));

        #endregion

        #region FeaturedRepeats

        public static DependencyProperty FeaturedRepeatsProperty = DependencyProperty.Register(nameof(FeaturedRepeats), typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool FeaturedRepeats
        {
            get => (bool)GetValue(FeaturedRepeatsProperty);
            set => SetValue(FeaturedRepeatsProperty, value);
        }

        #endregion

        #region GroupName

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(PropertyGridGroupName), typeof(PropertyGrid), new FrameworkPropertyMetadata(PropertyGridGroupName.Category, FrameworkPropertyMetadataOptions.None, OnGroupNameChanged));
        public PropertyGridGroupName GroupName
        {
            get => (PropertyGridGroupName)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }
        static void OnGroupNameChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnGroupNameChanged(new OldNew<PropertyGridGroupName>(e));

        #endregion

        #region GroupVisibility

        public static DependencyProperty GroupVisibilityProperty = DependencyProperty.Register(nameof(GroupVisibility), typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool GroupVisibility
        {
            get => (bool)GetValue(GroupVisibilityProperty);
            set => SetValue(GroupVisibilityProperty, value);
        }

        #endregion

        #region HeaderButtons

        public static DependencyProperty HeaderButtonsProperty = DependencyProperty.Register(nameof(HeaderButtons), typeof(FrameworkElementCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public FrameworkElementCollection HeaderButtons
        {
            get => (FrameworkElementCollection)GetValue(HeaderButtonsProperty);
            set => SetValue(HeaderButtonsProperty, value);
        }

        #endregion

        #region HeaderVisibility

        public static DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(nameof(HeaderVisibility), typeof(Visibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.None));
        public Visibility HeaderVisibility
        {
            get => (Visibility)GetValue(HeaderVisibilityProperty);
            set => SetValue(HeaderVisibilityProperty, value);
        }

        #endregion

        #region Loading

        static DependencyProperty LoadingProperty = DependencyProperty.Register(nameof(Loading), typeof(Handle), typeof(PropertyGrid), new FrameworkPropertyMetadata((Handle)false, FrameworkPropertyMetadataOptions.None));
        public Handle Loading
        {
            get => (Handle)GetValue(LoadingProperty);
            private set => SetValue(LoadingProperty, value);
        }

        #endregion

        #region LoaderTemplate

        public static DependencyProperty LoaderTemplateProperty = DependencyProperty.Register(nameof(LoaderTemplate), typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate LoaderTemplate
        {
            get => (DataTemplate)GetValue(LoaderTemplateProperty);
            set => SetValue(LoaderTemplateProperty, value);
        }

        #endregion

        #region Members

        public static DependencyProperty PropertiesProperty = DependencyProperty.Register(nameof(Properties), typeof(MemberCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(MemberCollection), OnMembersChanged));
        public MemberCollection Properties
        {
            get => (MemberCollection)GetValue(PropertiesProperty);
            private set => SetValue(PropertiesProperty, value);
        }
        static void OnMembersChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnMembersChanged(new OldNew<MemberCollection>(e));

        #endregion

        #region MembersView

        public static DependencyProperty PropertiesViewProperty = DependencyProperty.Register(nameof(PropertiesView), typeof(ListCollectionView), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(ListCollectionView), OnMembersViewChanged));
        public ListCollectionView PropertiesView
        {
            get => (ListCollectionView)GetValue(PropertiesViewProperty);
            private set => SetValue(PropertiesViewProperty, value);
        }
        static void OnMembersViewChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnMembersViewChanged(new OldNew<ListCollectionView>(e));

        #endregion

        #region NameAboveValue

        public static DependencyProperty NameAboveValueProperty = DependencyProperty.Register(nameof(NameAboveValue), typeof(bool), typeof(PropertyGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool NameAboveValue
        {
            get => (bool)GetValue(NameAboveValueProperty);
            set => SetValue(NameAboveValueProperty, value);
        }

        #endregion

        #region NameColumnHeader

        public static DependencyProperty NameColumnHeaderProperty = DependencyProperty.Register(nameof(NameColumnHeader), typeof(object), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.None, OnNameColumnHeaderChanged));
        public object NameColumnHeader
        {
            get => GetValue(NameColumnHeaderProperty);
            set => SetValue(NameColumnHeaderProperty, value);
        }
        static void OnNameColumnHeaderChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnNameColumnHeaderChanged(e.NewValue);

        #endregion

        #region NameColumnVisibility

        public static DependencyProperty NameColumnVisibilityProperty = DependencyProperty.Register(nameof(NameColumnVisibility), typeof(Visibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.None, OnNameColumnVisibilityChanged));
        public Visibility NameColumnVisibility
        {
            get => (Visibility)GetValue(NameColumnVisibilityProperty);
            set => SetValue(NameColumnVisibilityProperty, value);
        }
        static void OnNameColumnVisibilityChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnNameColumnVisibilityChanged(new OldNew<Visibility>(e));

        #endregion

        #region NameColumnWidth

        public static DependencyProperty NameColumnWidthProperty = DependencyProperty.Register(nameof(NameColumnWidth), typeof(DataGridLength), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.None, OnNameColumnWidthChanged));
        public DataGridLength NameColumnWidth
        {
            get => (DataGridLength)GetValue(NameColumnWidthProperty);
            set => SetValue(NameColumnWidthProperty, value);
        }
        static void OnNameColumnWidthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnNameColumnWidthChanged(new OldNew<DataGridLength>(e));

        #endregion

        #region NameTemplate

        public static DependencyProperty NameTemplateProperty = DependencyProperty.Register(nameof(NameTemplate), typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnNameTemplateChanged));
        public DataTemplate NameTemplate
        {
            get => (DataTemplate)GetValue(NameTemplateProperty);
            set => SetValue(NameTemplateProperty, value);
        }
        static void OnNameTemplateChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().NameColumn.If(j => j != null, j => j.CellTemplate = new OldNew<DataTemplate>(e).New);

        #endregion

        #region OverrideTemplates

        public static DependencyProperty OverrideTemplatesProperty = DependencyProperty.Register(nameof(OverrideTemplates), typeof(DataTemplateCollection), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplateCollection OverrideTemplates
        {
            get => (DataTemplateCollection)GetValue(OverrideTemplatesProperty);
            set => SetValue(OverrideTemplatesProperty, value);
        }

        #endregion

        #region PropertyCrumbTemplate

        public static DependencyProperty PropertyCrumbTemplateProperty = DependencyProperty.Register(nameof(PropertyCrumbTemplate), typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataTemplate)));
        public DataTemplate PropertyCrumbTemplate
        {
            get => (DataTemplate)GetValue(PropertyCrumbTemplateProperty);
            set => SetValue(PropertyCrumbTemplateProperty, value);
        }

        #endregion

        #region ReferencePropertyStringFormat

        public static DependencyProperty ReferencePropertyStringFormatProperty = DependencyProperty.Register(nameof(ReferencePropertyStringFormat), typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string ReferencePropertyStringFormat
        {
            get => (string)GetValue(ReferencePropertyStringFormatProperty);
            set => SetValue(ReferencePropertyStringFormatProperty, value);
        }

        #endregion

        #region ReferencePropertyTemplate

        public static DependencyProperty ReferencePropertyTemplateProperty = DependencyProperty.Register(nameof(ReferencePropertyTemplate), typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate ReferencePropertyTemplate
        {
            get => (DataTemplate)GetValue(ReferencePropertyTemplateProperty);
            set => SetValue(ReferencePropertyTemplateProperty, value);
        }

        #endregion

        #region ReferencePropertyTemplateSelector

        public static DependencyProperty ReferencePropertyTemplateSelectorProperty = DependencyProperty.Register(nameof(ReferencePropertyTemplateSelector), typeof(DataTemplateSelector), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplateSelector ReferencePropertyTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ReferencePropertyTemplateSelectorProperty);
            set => SetValue(ReferencePropertyTemplateSelectorProperty, value);
        }

        #endregion

        #region Route

        public static DependencyProperty RouteProperty = DependencyProperty.Register(nameof(Route), typeof(MemberRoute), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        /// <summary>
        /// Stores a reference to every nested property relative to the original object; properties are stored in order of depth.
        /// </summary>
        public MemberRoute Route
        {
            get => (MemberRoute)GetValue(RouteProperty);
            private set => SetValue(RouteProperty, value);
        }

        #endregion

        #region Search

        public static DependencyProperty SearchProperty = DependencyProperty.Register(nameof(Search), typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnSearchChanged));
        public string Search
        {
            get => (string)GetValue(SearchProperty);
            set => SetValue(SearchProperty, value);
        }
        static void OnSearchChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().Refresh();

        #endregion

        #region SearchName

        public static DependencyProperty SearchNameProperty = DependencyProperty.Register(nameof(SearchName), typeof(PropertyGridSearchName), typeof(PropertyGrid), new FrameworkPropertyMetadata(PropertyGridSearchName.DisplayName, FrameworkPropertyMetadataOptions.None, OnSearchNameChanged));
        public PropertyGridSearchName SearchName
        {
            get => (PropertyGridSearchName)GetValue(SearchNameProperty);
            set => SetValue(SearchNameProperty, value);
        }
        static void OnSearchNameChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().If(j => !j.Search.NullOrEmpty(), j => j.Refresh());

        #endregion

        #region SortDirection

        public static DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(ListSortDirection), typeof(PropertyGrid), new FrameworkPropertyMetadata(ListSortDirection.Ascending, FrameworkPropertyMetadataOptions.None, OnSortDirectionChanged));
        public ListSortDirection SortDirection
        {
            get => (ListSortDirection)GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }
        static void OnSortDirectionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnSortDirectionChanged(new OldNew<ListSortDirection>(e));

        #endregion

        #region SortName

        public static DependencyProperty SortNameProperty = DependencyProperty.Register(nameof(SortName), typeof(PropertyGridSortName), typeof(PropertyGrid), new FrameworkPropertyMetadata(PropertyGridSortName.DisplayName, FrameworkPropertyMetadataOptions.None, OnSortNameChanged));
        public PropertyGridSortName SortName
        {
            get => (PropertyGridSortName)GetValue(SortNameProperty);
            set => SetValue(SortNameProperty, value);
        }
        static void OnSortNameChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnSortNameChanged(new OldNew<PropertyGridSortName>(e));

        #endregion

        #region Source

        public static DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnSourceChanged));
        public object Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        static void OnSourceChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => _ = i.As<PropertyGrid>().OnSourceChanged(new OldNew(e));

        #endregion

        #region SplitterStyle

        public static DependencyProperty SplitterStyleProperty = DependencyProperty.Register(nameof(SplitterStyle), typeof(Style), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.None));
        public Style SplitterStyle
        {
            get => (Style)GetValue(SplitterStyleProperty);
            set => SetValue(SplitterStyleProperty, value);
        }

        #endregion

        #region SplitterVisibility

        public static DependencyProperty SplitterVisibilityProperty = DependencyProperty.Register(nameof(SplitterVisibility), typeof(Visibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility SplitterVisibility
        {
            get => (Visibility)GetValue(SplitterVisibilityProperty);
            private set => SetValue(SplitterVisibilityProperty, value);
        }

        #endregion

        #region TypeStringFormat

        public static DependencyProperty TypeStringFormatProperty = DependencyProperty.Register(nameof(TypeStringFormat), typeof(string), typeof(PropertyGrid), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string TypeStringFormat
        {
            get => (string)GetValue(TypeStringFormatProperty);
            set => SetValue(TypeStringFormatProperty, value);
        }

        #endregion

        #region TypeTemplate

        public static DependencyProperty TypeTemplateProperty = DependencyProperty.Register(nameof(TypeTemplate), typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None));
        public DataTemplate TypeTemplate
        {
            get => (DataTemplate)GetValue(TypeTemplateProperty);
            set => SetValue(TypeTemplateProperty, value);
        }

        #endregion

        #region TypeTemplateSelector

        public static DependencyProperty TypeTemplateSelectorProperty = DependencyProperty.Register(nameof(TypeTemplateSelector), typeof(DataTemplateSelector), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.None));
        public DataTemplateSelector TypeTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(TypeTemplateSelectorProperty);
            set => SetValue(TypeTemplateSelectorProperty, value);
        }

        #endregion

        #region TypeVisibility

        public static DependencyProperty TypeVisibilityProperty = DependencyProperty.Register(nameof(TypeVisibility), typeof(Visibility), typeof(PropertyGrid), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.None));
        public Visibility TypeVisibility
        {
            get => (Visibility)GetValue(TypeVisibilityProperty);
            set => SetValue(TypeVisibilityProperty, value);
        }

        #endregion

        #region ValueColumnHeader

        public static DependencyProperty ValueColumnHeaderProperty = DependencyProperty.Register(nameof(ValueColumnHeader), typeof(object), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.None, OnValueColumnHeaderChanged));
        public object ValueColumnHeader
        {
            get => GetValue(ValueColumnHeaderProperty);
            set => SetValue(ValueColumnHeaderProperty, value);
        }
        static void OnValueColumnHeaderChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnValueColumnHeaderChanged(e.NewValue);

        #endregion

        #region ValueColumnWidth

        public static DependencyProperty ValueColumnWidthProperty = DependencyProperty.Register(nameof(ValueColumnWidth), typeof(DataGridLength), typeof(PropertyGrid), new FrameworkPropertyMetadata(default(DataGridLength), FrameworkPropertyMetadataOptions.None, OnValueColumnWidthChanged));
        public DataGridLength ValueColumnWidth
        {
            get => (DataGridLength)GetValue(ValueColumnWidthProperty);
            set => SetValue(ValueColumnWidthProperty, value);
        }
        static void OnValueColumnWidthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().OnValueColumnWidthChanged(new OldNew<DataGridLength>(e));

        #endregion

        #region ValueTemplate

        public static DependencyProperty ValueTemplateProperty = DependencyProperty.Register(nameof(ValueTemplate), typeof(DataTemplate), typeof(PropertyGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnValueTemplateChanged));
        public DataTemplate ValueTemplate
        {
            get => (DataTemplate)GetValue(ValueTemplateProperty);
            set => SetValue(ValueTemplateProperty, value);
        }
        static void OnValueTemplateChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyGrid>().ValueColumn.CellTemplate = new OldNew<DataTemplate>(e).New;

        #endregion

        #endregion

        #region PropertyGrid

        public PropertyGrid() : base()
        {
            SetCurrentValue(LoadingProperty, new Handle(false));
            SetCurrentValue(OverrideTemplatesProperty, new DataTemplateCollection());

            NameColumn = CreateName();
            NameColumn.CellTemplate = NameTemplate;

            ValueColumn = new DataGridTemplateColumn() { CellTemplate = ValueTemplate };

            Columns.Add(NameColumn);
            Columns.Add(ValueColumn);

            SetCurrentValue(NameColumnHeaderProperty, "Name");
            SetCurrentValue(ValueColumnHeaderProperty, "Value");

            SetCurrentValue(NameColumnWidthProperty, new DataGridLength(40d, DataGridLengthUnitType.Star));
            SetCurrentValue(ValueColumnWidthProperty, new DataGridLength(60d, DataGridLengthUnitType.Star));

            Loaded += OnLoaded;
            //Unloaded += OnUnloaded; is this necessary?

            SetCurrentValue(RouteProperty, new MemberRoute(this));

            //--------------------------------------------------------------------

            SetCurrentValue(HeaderButtonsProperty, new FrameworkElementCollection());
            SetCurrentValue(PropertiesProperty, new MemberCollection(this));

            BindingOperations.SetBinding(this, SelectedValueProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(nameof(Properties.Active)),
                Source = Properties
            });
        }

        #endregion

        #region Methods

        DataGridTemplateColumn CreateName()
        {
            return new DataGridTemplateColumn()
            {
                SortMemberPath = nameof(PropertyModel.DisplayName),
                Visibility = Visibility.Collapsed
            };
        }

        //...........................................................................................

        void Group()
        {
            if (PropertiesView != null)
            {
                PropertiesView.GroupDescriptions.Clear();

                GroupDescription description = null;
                switch (GroupName)
                {
                    case PropertyGridGroupName.Category:
                        description = new PropertyGroupDescription(nameof(PropertyGridGroupName.Category), new DefaultConverter<string, string>(i => i, i => i));
                        break;
                    case PropertyGridGroupName.DisplayName:
                        description = new PropertyGroupDescription(nameof(PropertyGridGroupName.DisplayName), new Converters.FirstLetterConverter());
                        break;
                    case PropertyGridGroupName.Type:
                        description = new PropertyGroupDescription(nameof(PropertyGridGroupName.Type));
                        break;
                }

                if (description != null)
                    PropertiesView.GroupDescriptions.Add(description);
            }
        }

        void Sort()
        {
            PropertiesView?.SortDescriptions.Clear();
            PropertiesView?.SortDescriptions.Add(new SortDescription($"{SortName}", SortDirection));
        }

        //...........................................................................................

        void OnLoaded(object sender, RoutedEventArgs e) => OnLoaded(e);

        //...........................................................................................

        //void OnUnloaded(object sender, RoutedEventArgs e) => OnUnloaded(e); is this necessary?

        //...........................................................................................

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            handle.If(i => !i, i => throw new NotSupportedException());
        }

        //...........................................................................................

        protected virtual void OnDescriptionResizeModeChanged(OldNew<bool> input)
        {
            SetCurrentValue(SplitterVisibilityProperty, !input.New ? Visibility.Collapsed : DescriptionVisibility);
        }

        protected virtual void OnDescriptionVisibilityChanged(OldNew<Visibility> input)
        {
            SetCurrentValue(SplitterVisibilityProperty, !DescriptionResizeMode ? Visibility.Collapsed : input.New);
        }

        //...........................................................................................

        protected virtual void OnGroupDirectionChanged(ListSortDirection input) => Group();

        protected virtual void OnGroupNameChanged(OldNew<PropertyGridGroupName> input) => Group();

        //...........................................................................................

        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            if (!loadedOnce)
            {
                OnFirstLoaded();
                loadedOnce = true;
            }
        }

        protected virtual void OnFirstLoaded()
        {
            SetCurrentValue(SplitterVisibilityProperty, !DescriptionResizeMode ? Visibility.Collapsed : DescriptionVisibility);
            Group(); Sort();
        }

        //...........................................................................................

        protected virtual void OnNameColumnHeaderChanged(object input)
        {
            if (NameColumn != null)
                NameColumn.Header = input;
        }

        protected virtual void OnNameColumnVisibilityChanged(OldNew<Visibility> input)
        {
            switch (input.New)
            {
                case Visibility.Collapsed:
                case Visibility.Hidden:

                    if (NameColumn != null)
                    {
                        Columns.Remove(NameColumn);
                        NameColumn = null;
                    }

                    break;

                case Visibility.Visible:

                    if (NameColumn == null)
                    {
                        NameColumn = CreateName();
                        Columns.Add(NameColumn);
                    }

                    break;
            }
        }

        protected virtual void OnNameColumnWidthChanged(OldNew<DataGridLength> input)
        {
            NameColumn.If(i => i != null, i => i.Width = input.New);
        }

        //...........................................................................................

        protected virtual void OnMembersChanged(OldNew<MemberCollection> input)
        {
            SetCurrentValue(PropertiesViewProperty, new ListCollectionView(input.New));
        }

        protected virtual void OnMembersViewChanged(OldNew<ListCollectionView> input)
        {
            handle.Invoke(() => SetCurrentValue(ItemsSourceProperty, input.New));
        }

        //...........................................................................................

        //protected virtual void OnUnloaded(RoutedEventArgs e) => Unsubscribe(Sources(Source)); is this necessary?

        //...........................................................................................

        protected virtual void OnSortDirectionChanged(OldNew<ListSortDirection> input) => Sort();

        protected virtual void OnSortNameChanged(OldNew<PropertyGridSortName> input) => Sort();

        //...........................................................................................

        protected virtual async Task OnSourceChanged(OldNew input)
        {
            if (sourceChanging)
            {
                sourceChanges.Enqueue(input);
                return;
            }

            sourceChanging = true;

            if (input.New.Not<Child>())
                Route.Clear();

            Properties.Clear();
            if (input.New != null)
            {
                var actualSource = Route.Append(input.New);

                Loading = true;
                await Properties.Load(actualSource);
                Loading = false;
            }

            SourceChanged?.Invoke(this, new EventArgs<object>(input.New));

            OldNew next = null;

            if (sourceChanges.Any())
                next = sourceChanges.Dequeue();

            sourceChanging = false;

            if (next != null)
                await OnSourceChanged(next);
        }

        //...........................................................................................

        protected virtual void OnValueColumnHeaderChanged(object input)
        {
            ValueColumn.Header = input;
        }

        protected virtual void OnValueColumnWidthChanged(OldNew<DataGridLength> input)
        {
            ValueColumn.Width = input.New;
        }

        //...........................................................................................

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            NameColumn.If(i => i != null, i => i.CellTemplate = NameTemplate);
            ValueColumn.CellTemplate = ValueTemplate;

            GroupStyle.Add(DefaultGroupStyle);
        }

        async public void Refresh() => await OnSourceChanged(new OldNew(Source, Source));

        //...........................................................................................

        ICommand backwardCommand;
        public ICommand BackwardCommand => backwardCommand = backwardCommand ?? new RelayCommand(() => Route.Previous(), () => Route.Count > 1);

        ICommand forwardCommand;
        public ICommand ForwardCommand => forwardCommand = forwardCommand ?? new RelayCommand<MemberModel>(i => Route.Next(i), i => i != null);

        //...........................................................................................

        ICommand groupCommand;
        public ICommand GroupCommand
        {
            get
            {
                groupCommand = groupCommand ?? new RelayCommand<object>(p => SetCurrentValue(GroupNameProperty, (PropertyGridGroupName)p), p => p is PropertyGridGroupName);
                return groupCommand;
            }
        }

        ICommand sortCommand;
        public ICommand SortCommand
        {
            get
            {
                sortCommand = sortCommand ?? new RelayCommand<object>(p =>
                {
                    if (p is PropertyGridSortName)
                        SetCurrentValue(SortNameProperty, (PropertyGridSortName)p);

                    if (p is ListSortDirection)
                        SetCurrentValue(SortDirectionProperty, (ListSortDirection)p);
                }, p => p is PropertyGridSortName || p is ListSortDirection);
                return sortCommand;
            }
        }

        //...........................................................................................

        ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                refreshCommand = refreshCommand ?? new RelayCommand(() => Refresh(), () => Source != null);
                return refreshCommand;
            }
        }

        ICommand resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                resetCommand = resetCommand ?? new RelayCommand(() => Properties.ForEach(i => i.Value = null), () => Source != null);
                return resetCommand;
            }
        }

        ICommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                searchCommand = searchCommand ?? new RelayCommand<PropertyGridSearchName>(i => SetCurrentValue(SearchNameProperty, i), i => true);
                return searchCommand;
            }
        }

        #endregion
    }
}