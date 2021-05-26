using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_ContentHeader), Type = typeof(ContentControl))]
    public class TreeViewContent : TreeView
    {
        #region Properties

        ContentControl PART_ContentHeader;

        public static DependencyProperty ContentBackgroundProperty = DependencyProperty.Register(nameof(ContentBackground), typeof(Brush), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush ContentBackground
        {
            get => (Brush)GetValue(ContentBackgroundProperty);
            set => SetValue(ContentBackgroundProperty, value);
        }

        public static DependencyProperty ContentBorderBrushProperty = DependencyProperty.Register(nameof(ContentBorderBrush), typeof(Brush), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush ContentBorderBrush
        {
            get => (Brush)GetValue(ContentBorderBrushProperty);
            set => SetValue(ContentBorderBrushProperty, value);
        }

        public static DependencyProperty ContentBorderThicknessProperty = DependencyProperty.Register(nameof(ContentBorderThickness), typeof(Thickness), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ContentBorderThickness
        {
            get => (Thickness)GetValue(ContentBorderThicknessProperty);
            set => SetValue(ContentBorderThicknessProperty, value);
        }

        public static DependencyProperty ContentHeaderTemplateProperty = DependencyProperty.Register(nameof(ContentHeaderTemplate), typeof(DataTemplate), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ContentHeaderTemplate
        {
            get => (DataTemplate)GetValue(ContentHeaderTemplateProperty);
            set => SetValue(ContentHeaderTemplateProperty, value);
        }

        public static DependencyProperty ContentHeaderVisibilityProperty = DependencyProperty.Register(nameof(ContentHeaderVisibility), typeof(Visibility), typeof(TreeViewContent), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility ContentHeaderVisibility
        {
            get => (Visibility)GetValue(ContentHeaderVisibilityProperty);
            set => SetValue(ContentHeaderVisibilityProperty, value);
        }

        public static DependencyProperty ContentPaddingProperty = DependencyProperty.Register(nameof(ContentPadding), typeof(Thickness), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ContentPadding
        {
            get => (Thickness)GetValue(ContentPaddingProperty);
            set => SetValue(ContentPaddingProperty, value);
        }

        public static DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ContentTemplate
        {
            get => (DataTemplate)GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public static DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(nameof(ContentTemplateSelector), typeof(DataTemplateSelector), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(DataTemplateSelector), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty);
            set => SetValue(ContentTemplateSelectorProperty, value);
        }

        public static DependencyProperty ContentTransitionProperty = DependencyProperty.Register(nameof(ContentTransition), typeof(Transitions), typeof(TreeViewContent), new FrameworkPropertyMetadata(Transitions.LeftReplace, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Transitions ContentTransition
        {
            get => (Transitions)GetValue(ContentTransitionProperty);
            set => SetValue(ContentTransitionProperty, value);
        }

        public static DependencyProperty ContentWidthProperty = DependencyProperty.Register(nameof(ContentWidth), typeof(GridLength), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GridLength ContentWidth
        {
            get => (GridLength)GetValue(ContentWidthProperty);
            set => SetValue(ContentWidthProperty, value);
        }

        public static DependencyProperty MenuWidthProperty = DependencyProperty.Register(nameof(MenuWidth), typeof(GridLength), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(GridLength), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GridLength MenuWidth
        {
            get => (GridLength)GetValue(MenuWidthProperty);
            set => SetValue(MenuWidthProperty, value);
        }

        public static DependencyProperty MenuBackgroundProperty = DependencyProperty.Register(nameof(MenuBackground), typeof(Brush), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MenuBackground
        {
            get => (Brush)GetValue(MenuBackgroundProperty);
            set => SetValue(MenuBackgroundProperty, value);
        }

        public static DependencyProperty MenuBorderBrushProperty = DependencyProperty.Register(nameof(MenuBorderBrush), typeof(Brush), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush MenuBorderBrush
        {
            get => (Brush)GetValue(MenuBorderBrushProperty);
            set => SetValue(MenuBorderBrushProperty, value);
        }

        public static DependencyProperty MenuBorderThicknessProperty = DependencyProperty.Register(nameof(MenuBorderThickness), typeof(Thickness), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness MenuBorderThickness
        {
            get => (Thickness)GetValue(MenuBorderThicknessProperty);
            set => SetValue(MenuBorderThicknessProperty, value);
        }

        public static DependencyProperty MenuPaddingProperty = DependencyProperty.Register(nameof(MenuPadding), typeof(Thickness), typeof(TreeViewContent), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness MenuPadding
        {
            get => (Thickness)GetValue(MenuPaddingProperty);
            set => SetValue(MenuPaddingProperty, value);
        }

        public static DependencyProperty SplitterStyleProperty = DependencyProperty.Register(nameof(SplitterStyle), typeof(Style), typeof(TreeViewContent), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Style SplitterStyle
        {
            get => (Style)GetValue(SplitterStyleProperty);
            set => SetValue(SplitterStyleProperty, value);
        }

        public static DependencyProperty SplitterVisibilityProperty = DependencyProperty.Register(nameof(SplitterVisibility), typeof(Visibility), typeof(TreeViewContent), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.None));
        public Visibility SplitterVisibility
        {
            get => (Visibility)GetValue(SplitterVisibilityProperty);
            set => SetValue(SplitterVisibilityProperty, value);
        }

        #endregion

        #region TreeViewContent

        public TreeViewContent() : base()
        {
            DefaultStyleKey = typeof(TreeViewContent);
            SelectedItemChanged += OnSelectedItemChanged;
        }

        #endregion

        #region Methods

        protected override void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (PART_ContentHeader != null)
                PART_ContentHeader.Content = e.NewValue.As<TreeViewItem>()?.Header.ToString() ?? e.NewValue;
        }

        public override void OnApplyTemplate()
        {
            PART_ContentHeader = Template.FindName(nameof(PART_ContentHeader), this) as ContentControl;
        }

        #endregion
    }
}