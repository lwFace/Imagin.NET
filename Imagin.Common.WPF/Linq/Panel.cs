using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    public static class PanelExtensions
    {
        #region HorizontalContentAlignment

        /// <summary>
        /// Applies <see cref="HorizontalAlignment"/> to all children except those that define <see cref="FrameworkElementExtensions.HorizontalAlignmentProperty"/>.
        /// </summary>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(PanelExtensions), new FrameworkPropertyMetadata(HorizontalAlignment.Left, OnHorizontalContentAlignmentChanged));
        public static void SetHorizontalContentAlignment(Panel d, HorizontalAlignment value)
            => d.SetValue(HorizontalContentAlignmentProperty, value);
        public static HorizontalAlignment GetHorizontalContentAlignment(Panel d)
            => (HorizontalAlignment)d.GetValue(HorizontalContentAlignmentProperty);
        static void OnHorizontalContentAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            panel.SizeChanged -= OnHorizontalContentAlignmentUpdated;
            panel.SizeChanged += OnHorizontalContentAlignmentUpdated;
            OnHorizontalContentAlignmentUpdated(panel, null);
        }
        static void OnHorizontalContentAlignmentUpdated(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Panel;
            var value = GetHorizontalContentAlignment(panel);
            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var frameworkElement = panel.Children[i].As<FrameworkElement>();
                if (FrameworkElementExtensions.GetHorizontalAlignment(frameworkElement) == null)
                    frameworkElement.HorizontalAlignment = value;
            }
        }

        #endregion

        #region Spacing

        /// <summary>
        /// Applies <see cref="Thickness"/> to all children except those that define <see cref="FrameworkElementExtensions.MarginProperty"/>.
        /// </summary>
        public static readonly DependencyProperty SpacingProperty = DependencyProperty.RegisterAttached("Spacing", typeof(Thickness), typeof(PanelExtensions), new FrameworkPropertyMetadata(default(Thickness), OnSpacingChanged));
        public static void SetSpacing(Panel d, Thickness value)
            => d.SetValue(SpacingProperty, value);
        public static Thickness GetSpacing(Panel d)
            => (Thickness)d.GetValue(SpacingProperty);
        static void OnSpacingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            panel.SizeChanged -= OnSpacingUpdated;
            panel.SizeChanged += OnSpacingUpdated;
            OnSpacingUpdated(panel, null);
        }
        static void OnSpacingUpdated(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Panel;
            var value = GetSpacing(panel);

            var except = GetSpacingExcept(panel);

            var tf = except.HasFlag(SpacingExceptions.First);
            var tl = except.HasFlag(SpacingExceptions.Last);

            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var frameworkElement = panel.Children[i].As<FrameworkElement>();
                if (FrameworkElementExtensions.GetMargin(frameworkElement) == null)
                {
                    frameworkElement.Margin
                        = (i == 0 && tf) || (i == (Count - 1) && tl)
                        ? new Thickness(0)
                        : frameworkElement.Margin = value;
                }
            }
        }

        #endregion

        #region SpacingExcept

        public static readonly DependencyProperty SpacingExceptProperty = DependencyProperty.RegisterAttached("SpacingExcept", typeof(SpacingExceptions), typeof(PanelExtensions), new FrameworkPropertyMetadata(SpacingExceptions.None, OnSpacingChanged));
        public static void SetSpacingExcept(Panel d, SpacingExceptions value) => d.SetValue(SpacingExceptProperty, value);
        public static SpacingExceptions GetSpacingExcept(Panel d) => (SpacingExceptions)d.GetValue(SpacingExceptProperty);

        #endregion

        #region SpacingExceptions

        [Flags]
        public enum SpacingExceptions
        {
            None = 0,
            First = 1,
            Last = 2
        }

        #endregion

        #region VerticalContentAlignment

        /// <summary>
        /// Applies <see cref="VerticalAlignment"/> to all children except those that define <see cref="FrameworkElementExtensions.VerticalAlignmentProperty"/>.
        /// </summary>
        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.RegisterAttached("VerticalContentAlignment", typeof(VerticalAlignment), typeof(PanelExtensions), new FrameworkPropertyMetadata(VerticalAlignment.Top, OnVerticalContentAlignmentChanged));
        public static void SetVerticalContentAlignment(Panel d, VerticalAlignment value)
            => d.SetValue(VerticalContentAlignmentProperty, value);
        public static VerticalAlignment GetVerticalContentAlignment(Panel d)
            => (VerticalAlignment)d.GetValue(VerticalContentAlignmentProperty);
        static void OnVerticalContentAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            panel.SizeChanged -= OnVerticalContentAlignmentUpdated;
            panel.SizeChanged += OnVerticalContentAlignmentUpdated;
            OnVerticalContentAlignmentUpdated(panel, null);
        }
        static void OnVerticalContentAlignmentUpdated(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Panel;
            var value = GetVerticalContentAlignment(panel);
            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var frameworkElement = panel.Children[i].As<FrameworkElement>();
                if (FrameworkElementExtensions.GetVerticalAlignment(frameworkElement) == null)
                    frameworkElement.VerticalAlignment = value;
            }
        }

        #endregion
    }
}