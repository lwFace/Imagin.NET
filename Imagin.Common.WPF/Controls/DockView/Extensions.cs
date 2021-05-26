using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public static class DockViewExtensions
    {
        static void Each<T>(ILayoutControl i, Action<T> action) where T : Content
        {
            if (i == null)
                return;

            if (typeof(T).Equals(typeof(Document)))
            {
                if (i is LayoutDocumentGroupControl a)
                {
                    foreach (T j in a.Source)
                        action(j);
                }
            }
            if (typeof(T).Equals(typeof(Models.Panel)))
            {
                if (i is LayoutPanelGroupControl b)
                {
                    foreach (T j in b.Source)
                        action(j);
                }
            }
            if (i is LayoutGroupControl c)
                Each(i, action, c);
        }

        public static void Each<T>(this ILayoutControl input, Action<T> action, LayoutGroupControl parent = null) where T : Content
        {
            if (parent == null)
            {
                Each(input, action);
                return;
            }

            foreach (var i in parent.Children)
                Each(i as ILayoutControl, action);
        }

        static void EachControl<T>(ILayoutControl i, Action<T> action) where T : LayoutContentGroupControl
        {
            if (i == null)
                return;

            if (typeof(T).Equals(typeof(LayoutDocumentGroupControl)))
            {
                if (i is LayoutDocumentGroupControl a)
                    action(a as T);
            }
            if (typeof(T).Equals(typeof(LayoutPanelGroupControl)))
            {
                if (i is LayoutPanelGroupControl b)
                    action(b as T);
            }
            if (i is LayoutGroupControl c)
                EachControl(i, action, c);
        }

        public static void EachControl<T>(this ILayoutControl input, Action<T> action, LayoutGroupControl parent = null) where T : LayoutContentGroupControl
        {
            if (parent == null)
            {
                EachControl(input, action);
                return;
            }

            foreach (var i in parent.Children)
                EachControl(i as ILayoutControl, action);
        }

        /// ......................................................................................................................

        static LayoutGroupControl GetParent(this ILayoutControl input, LayoutGroupControl parent = null)
        {
            if (parent == null)
            {
                if (ReferenceEquals(input.Root.Child, input))
                    return null;

                if (input.Root.Child is LayoutGroupControl i)
                    return input.GetParent(i);
            }

            foreach (var i in parent.Children)
            {
                if (ReferenceEquals(i, input))
                    return parent;

                if (i is LayoutGroupControl j)
                {
                    var result = input.GetParent(j);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public static LayoutGroupControl GetParent(this ILayoutControl input) => input.GetParent(null);

        /// ......................................................................................................................

        public static Point GetPosition(this ILayoutControl input)
        {
            var x = input.ActualWidth / 2;
            var y = input.ActualHeight / 2;

            UIElement last = input as UIElement;
            while (true)
            {
                var parent = last.GetParent<LayoutGroupControl>();
                if (parent != null)
                {
                    foreach (FrameworkElement i in parent.Children)
                    {
                        if (i.Equals(last))
                            break;

                        if (parent.Orientation == Orientation.Horizontal)
                            x += i.ActualWidth;

                        if (parent.Orientation == Orientation.Vertical)
                            y += i.ActualHeight;
                    }
                    last = parent;
                }
                else break;
            }
            return new Point(x, y);
        }

        /// ......................................................................................................................

        public static int GetIndex(this ILayoutControl input) => input.GetParent()?.Children.IndexOf(input as UIElement) ?? -1;

        /// ......................................................................................................................

        static void Unsubscribe(ILayoutControl i)
        {
            if (i is LayoutDocumentGroupControl a)
                Unsubscribe(a);

            if (i is LayoutPanelGroupControl b)
                Unsubscribe(b);
        }

        static void Unsubscribe(LayoutDocumentGroupControl i)
        {
            i.DockView.DocumentGroups.Remove(i);
        }

        static void Unsubscribe(LayoutPanelGroupControl input)
        {
            input.DockView.Unsubscribe(input);
            for (var i = input.DockView.Hidden.Count - 1; i >= 0; i--)
            {
                var j = input.DockView.Hidden.ElementAt(i);
                if (j.Value.Equals(input))
                    input.DockView.Hidden.Remove(j.Key);
            }
            input.DockView.PanelGroups.Remove(input);
        }

        /// ......................................................................................................................

        public static void Remove(this ILayoutControl input)
        {
            var parent = input.GetParent();
            if (parent == null)
            {
                Unsubscribe(input);
                input.Root.Child = null;
                return;
            }

            var index = input.GetIndex();
            if (index == -1)
                throw new IndexOutOfRangeException();

            Unsubscribe(input);
            parent.Children.RemoveAt(index);

            //Remove the column or row definition
            if (parent.Orientation == Orientation.Horizontal)
                parent.ColumnDefinitions.RemoveAt(index);

            if (parent.Orientation == Orientation.Vertical)
                parent.RowDefinitions.RemoveAt(index);

            if (parent.Children.Count > 0)
            {
                if (index >= parent.Children.Count)
                    index = parent.Children.Count - 1;

                //Remove the grid splitter
                if (index < parent.Children.Count)
                {
                    if (parent.Children[index] is GridSplitter)
                    {
                        parent.Children.RemoveAt(index);
                        //Remove the column or row definition again
                        if (parent.Orientation == Orientation.Horizontal)
                            parent.ColumnDefinitions.RemoveAt(index);

                        if (parent.Orientation == Orientation.Vertical)
                            parent.RowDefinitions.RemoveAt(index);
                    }
                }
            }

            //Reassign all column or row values
            for (var i = 0; i < parent.Children.Count; i++)
            {
                if (parent.Orientation == Orientation.Horizontal)
                    Grid.SetColumn(parent.Children[i], i);

                if (parent.Orientation == Orientation.Vertical)
                    Grid.SetRow(parent.Children[i], i);
            }

            //Remove the parent from it's own parent to avoid empty LayoutGroupControls
            if (parent.Children.Count == 0)
                parent.Remove();
        }
    }
}