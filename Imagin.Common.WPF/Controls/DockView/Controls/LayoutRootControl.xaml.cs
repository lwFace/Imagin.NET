using Imagin.Common.Converters;
using Imagin.Common.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class LayoutRootControl : UserControl
    {
        public ILayoutControl Child
        {
            get => Root.Child as ILayoutControl;
            set => Root.Child = value as UIElement;
        }

        public DockView DockView { get; private set; }

        public readonly LayoutWindowControl Window;

        internal Content ActiveContent { get; private set; }

        /// ......................................................................................................................

        public static DependencyProperty EmptyMarkerStyleProperty = DependencyProperty.Register(nameof(EmptyMarkerStyle), typeof(Style), typeof(LayoutRootControl), new FrameworkPropertyMetadata(null));
        public Style EmptyMarkerStyle
        {
            get => (Style)GetValue(EmptyMarkerStyleProperty);
            set => SetValue(EmptyMarkerStyleProperty, value);
        }

        public static DependencyProperty PrimaryMarkerStyleProperty = DependencyProperty.Register(nameof(PrimaryMarkerStyle), typeof(Style), typeof(LayoutRootControl), new FrameworkPropertyMetadata(null));
        public Style PrimaryMarkerStyle
        {
            get => (Style)GetValue(PrimaryMarkerStyleProperty);
            set => SetValue(PrimaryMarkerStyleProperty, value);
        }

        public static DependencyProperty SecondaryMarkerStyleProperty = DependencyProperty.Register(nameof(SecondaryMarkerStyle), typeof(Style), typeof(LayoutRootControl), new FrameworkPropertyMetadata(null));
        public Style SecondaryMarkerStyle
        {
            get => (Style)GetValue(SecondaryMarkerStyleProperty);
            set => SetValue(SecondaryMarkerStyleProperty, value);
        }

        public static DependencyProperty SelectionStyleProperty = DependencyProperty.Register(nameof(SelectionStyle), typeof(Style), typeof(LayoutRootControl), new FrameworkPropertyMetadata(null));
        public Style SelectionStyle
        {
            get => (Style)GetValue(SelectionStyleProperty);
            set => SetValue(SelectionStyleProperty, value);
        }

        /// ......................................................................................................................

        public LayoutRootControl(DockView input, LayoutWindowControl window)
        {
            DockView = input;
            Window = window;

            InitializeComponent();

            var multiBinding = new MultiBinding()
            {
                Converter = new MultiConverter<Visibility>(values =>
                {
                    if ((values[1] as ILayoutControl)?.Root.Equals(this) == true)
                        return Visibility.Visible;

                    return Visibility.Collapsed;
                }),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            multiBinding.Bindings.Add(new Binding("Drag") { Source = DockView });
            multiBinding.Bindings.Add(new Binding("Drag.MouseOver") { Source = DockView });
            Markers.SetBinding(VisibilityProperty, multiBinding);

            foreach (MaskedImage i in SecondaryMarkers.Children)
            {
                if (i.Tag is Docks docks && docks == Docks.Center)
                {
                    i.SetBinding(VisibilityProperty, new Binding()
                    {
                        Converter = new DefaultConverter<ILayoutControl, Visibility>(j =>
                        {
                            if (DockView.Dragging)
                            {
                                if (DockView.Drag.Content?.First() is Document && DockView.Drag.MouseOver is LayoutDocumentGroupControl)
                                    return Visibility.Visible;

                                if (DockView.Drag.Content?.First() is Models.Panel && DockView.Drag.MouseOver is LayoutPanelGroupControl)
                                    return Visibility.Visible;
                            }
                            return Visibility.Collapsed;
                        }, j => null),
                        Mode = BindingMode.OneWay,
                        Path = new PropertyPath($"{nameof(DockView.Drag)}.{nameof(DockView.Drag.MouseOver)}"),
                        Source = DockView,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    break;
                }
            }
        }

        /// ......................................................................................................................

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            DockView.Drag?.End(false);
        }

        /// ......................................................................................................................

        void OnPrimaryMarkerMouseEnter(object sender, MouseEventArgs e)
        {
            if (DockView.Dragging)
            {
                switch ((Docks)(sender as FrameworkElement).Tag)
                {
                    case Docks.Left:
                        Select(0, 0, ActualHeight, ActualWidth / 2);
                        break;
                    case Docks.Top:
                        Select(0, 0, ActualHeight / 2, ActualWidth);
                        break;
                    case Docks.Right:
                        Select(ActualWidth / 2, 0, ActualHeight, ActualWidth / 2);
                        break;
                    case Docks.Bottom:
                        Select(0, ActualHeight / 2, ActualHeight / 2, ActualWidth);
                        break;
                }
            }
        }

        void OnPrimaryMarkerMouseLeave(object sender, MouseEventArgs e) => Selection.Height = Selection.Width = 0;

        void OnPrimaryMarkerMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DockView.Dock(DockView.Drag.MouseOver.Root, (Docks)(sender as FrameworkElement).Tag, null, DockView.Drag.ActualContent);
            DockView.Drag.End(true);
        }

        /// ......................................................................................................................

        void OnSecondaryMarkerMouseEnter(object sender, MouseEventArgs e)
        {
            if (DockView.Dragging)
            {
                switch ((Docks)(sender as FrameworkElement).Tag)
                {
                    case Docks.Left:
                        Select(DockView.Drag.MousePosition.X - (DockView.Drag.MouseOver.ActualWidth / 2), DockView.Drag.MousePosition.Y - (DockView.Drag.MouseOver.ActualHeight / 2), DockView.Drag.MouseOver.ActualHeight, DockView.Drag.MouseOver.ActualWidth / 2);
                        break;
                    case Docks.Top:
                        Select(DockView.Drag.MousePosition.X - (DockView.Drag.MouseOver.ActualWidth / 2), DockView.Drag.MousePosition.Y - (DockView.Drag.MouseOver.ActualHeight / 2), DockView.Drag.MouseOver.ActualHeight / 2, DockView.Drag.MouseOver.ActualWidth);
                        break;
                    case Docks.Right:
                        Select(DockView.Drag.MousePosition.X, DockView.Drag.MousePosition.Y - (DockView.Drag.MouseOver.ActualHeight / 2), DockView.Drag.MouseOver.ActualHeight, DockView.Drag.MouseOver.ActualWidth / 2);
                        break;
                    case Docks.Bottom:
                        Select(DockView.Drag.MousePosition.X - (DockView.Drag.MouseOver.ActualWidth / 2), DockView.Drag.MousePosition.Y, DockView.Drag.MouseOver.ActualHeight / 2, DockView.Drag.MouseOver.ActualWidth);
                        break;
                    case Docks.Center:
                        Select(DockView.Drag.MousePosition.X - (DockView.Drag.MouseOver.ActualWidth / 2), DockView.Drag.MousePosition.Y - (DockView.Drag.MouseOver.ActualHeight / 2), DockView.Drag.MouseOver.ActualHeight, DockView.Drag.MouseOver.ActualWidth);
                        break;
                }
            }
        }

        void OnSecondaryMarkerMouseLeave(object sender, MouseEventArgs e) => Selection.Height = Selection.Width = 0;

        void OnSecondaryMarkerMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool result()
            {
                var docks = (Docks)(sender as FrameworkElement).Tag;
                if (docks == Docks.Center)
                    return DockView.DockCenter(DockView.Drag);

                object b = DockView.Drag.ActualContent;

                var parent = DockView.Drag.MouseOver.GetParent();
                if (parent == null)
                {
                    DockView.Dock(DockView.Drag.MouseOver.Root, docks, null, DockView.Drag.ActualContent);
                    return true;
                }

                var index = DockView.Drag.MouseOver.GetIndex();
                //If a parent exists, the index should always be >= 0; if it isn't, something wrong is happening somewhere...
                if (index == -1)
                    return false;

                DockView.Dock(DockView.Drag.MouseOver.Root, docks, DockView.Drag.MouseOver, DockView.Drag.ActualContent);
                return true;
            }
            DockView.Drag.End(result());
        }

        /// ......................................................................................................................

        public void Activate(Content content)
        {
            ActiveContent = content;
        }

        public void Select(double x, double y, double height, double width)
        {
            Selection.Height = height;
            Selection.Width = width;
            Canvas.SetLeft(Selection, x);
            Canvas.SetTop(Selection, y);
        }
    }
}