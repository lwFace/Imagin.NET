using Imagin.Common.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class PropertyCrumb : Base
    {
        object data;
        public object Data
        {
            get => data;
            set => this.Change(ref data, value);
        }

        public PropertyCrumb(object data)
        {
            Data = data;
        }
    }

    public class PropertyCrumbSeparator : PropertyCrumb
    {
        public PropertyCrumbSeparator() : base(null) { }
    }

    public partial class PropertyCrumbView : UserControl
    {
        public static DependencyProperty RouteProperty = DependencyProperty.Register(nameof(Route), typeof(MemberRoute), typeof(PropertyCrumbView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnRouteChanged));
        public MemberRoute Route
        {
            get => (MemberRoute)GetValue(RouteProperty);
            set => SetValue(RouteProperty, value);
        }
        static void OnRouteChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PropertyCrumbView>().OnRouteChanged(new OldNew<MemberRoute>(e));

        public static DependencyProperty SeparatorTemplateProperty = DependencyProperty.Register(nameof(SeparatorTemplate), typeof(DataTemplate), typeof(PropertyCrumbView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate SeparatorTemplate
        {
            get => (DataTemplate)GetValue(SeparatorTemplateProperty);
            set => SetValue(SeparatorTemplateProperty, value);
        }

        public static DependencyProperty CrumbTemplateProperty = DependencyProperty.Register(nameof(CrumbTemplate), typeof(DataTemplate), typeof(PropertyCrumbView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate CrumbTemplate
        {
            get => (DataTemplate)GetValue(CrumbTemplateProperty);
            set => SetValue(CrumbTemplateProperty, value);
        }

        public static DependencyProperty CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), typeof(PropertyCrumbView), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None));
        public int Count
        {
            get => (int)GetValue(CountProperty);
            private set => SetValue(CountProperty, value);
        }

        public PropertyCrumbView()
        {
            InitializeComponent();
        }

        void Refresh()
        {
            if (Route != null)
            {
                SetCurrentValue(CountProperty, (Route.Count * 2) - 1);
                var result = new ObservableCollection<PropertyCrumb>();
                for (var i = 0; i < Route.Count; i++)
                {
                    var source = Route[i];

                    if (source is PropertyGrid.Child child)
                        source = child.Value;

                    if (source is PropertyGrid.Owner parent)
                        source = Source.Split(parent.Value).First();

                    if (source == null)
                        return;

                    result.Add(new PropertyCrumb(source));
                    if (i < Route.Count - 1)
                        result.Add(new PropertyCrumbSeparator());
                }
                ItemsControl.ItemsSource = result;
            }
        }

        protected virtual void OnRouteChanged(OldNew<MemberRoute> input)
        {
            Refresh();

            if (input.Old != null)
                input.Old.CollectionChanged -= OnCrumbsChanged;

            if (input.New != null)
                input.New.CollectionChanged += OnCrumbsChanged;
        }

        void OnCrumbsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }
    }
}