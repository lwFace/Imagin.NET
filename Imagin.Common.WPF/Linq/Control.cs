using Imagin.Common.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class ControlExtensions
    {
        #region Properties

        #region IsDraggingOver

        static Control currentDropTarget = null;

        static Control currentControl = null;

        /// <summary>
        /// Indicates whether or not the current item is a possible drop target
        /// </summary>
        static bool isPossibleDropTarget;

        /// <summary>
        /// Indicates whether or not the current item is a possible drop target.
        /// </summary>
        static readonly DependencyPropertyKey IsDraggingOverKey = DependencyProperty.RegisterAttachedReadOnly("IsDraggingOver", typeof(bool), typeof(ControlExtensions), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnIsDraggingOverCoerced)));
        public static readonly DependencyProperty IsDraggingOverProperty = IsDraggingOverKey.DependencyProperty;
        public static bool GetIsDraggingOver(Control i) => (bool)i.GetValue(IsDraggingOverProperty);
        static object OnIsDraggingOverCoerced(DependencyObject i, object value) => i == currentDropTarget && isPossibleDropTarget ? true : false;

        #endregion

        #region IsMouseDirectlyOver

        /// <summary>
        /// Indicates whether or not the mouse is directly over an item.
        /// </summary>
        static readonly DependencyPropertyKey IsMouseDirectlyOverKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseDirectlyOver", typeof(bool), typeof(ControlExtensions), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnIsMouseDirectlyOverCoerced)));
        public static readonly DependencyProperty IsMouseDirectlyOverProperty = IsMouseDirectlyOverKey.DependencyProperty;
        public static bool GetIsMouseDirectlyOver(Control i) => (bool)i.GetValue(IsMouseDirectlyOverProperty);
        static object OnIsMouseDirectlyOverCoerced(DependencyObject i, object value) => i == currentControl;

        #endregion

        #region IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));
        public static void SetIsReadOnly(Control d, bool value)
        {
            d.SetValue(IsReadOnlyProperty, value);
        }
        public static bool GetIsReadOnly(Control d)
        {
            return (bool)d.GetValue(IsReadOnlyProperty);
        }

        #endregion

        /// -----------------------------------------------

        #region FadeIn

        public static readonly DependencyProperty FadeInProperty = DependencyProperty.RegisterAttached("FadeIn", typeof(bool), typeof(ControlExtensions), new UIPropertyMetadata(false, OnFadeInChanged));
        public static bool GetFadeIn(FrameworkElement i) => (bool)i.GetValue(FadeInProperty);
        public static void SetFadeIn(FrameworkElement i, bool value) => i.SetValue(FadeInProperty, value);
        static void OnFadeInChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as FrameworkElement;
            control.Loaded -= FadeIn_OnLoaded;
            if (GetFadeIn(control))
                control.Loaded += FadeIn_OnLoaded;
        }

        static void FadeIn_OnLoaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            FadeIn(frameworkElement);
        }

        static void FadeIn(FrameworkElement control)
        {
            var duration = GetFadeInDuration(control);
            control.FadeIn(duration == default ? new Duration(TimeSpan.FromSeconds(0.5)) : duration);
        }

        #endregion

        #region FadeInDuration

        public static readonly DependencyProperty FadeInDurationProperty = DependencyProperty.RegisterAttached("FadeInDuration", typeof(Duration), typeof(ControlExtensions), new UIPropertyMetadata(default(Duration)));
        public static Duration GetFadeInDuration(FrameworkElement i) => (Duration)i.GetValue(FadeInDurationProperty);
        public static void SetFadeInDuration(FrameworkElement i, Duration value) => i.SetValue(FadeInDurationProperty, value);

        #endregion

        #region FadeOut

        public static readonly DependencyProperty FadeOutProperty = DependencyProperty.RegisterAttached("FadeOut", typeof(bool), typeof(ControlExtensions), new UIPropertyMetadata(false, OnFadesOutChanged));
        public static bool GetFadeOut(FrameworkElement i) => (bool)i.GetValue(FadeOutProperty);
        public static void SetFadeOut(FrameworkElement i, bool value) => i.SetValue(FadeOutProperty, value);
        static void OnFadesOutChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as FrameworkElement;
            if (control != null)
            {
                control.Unloaded -= FadeOut_OnUnloaded;
                if ((bool)e.NewValue)
                    control.Unloaded += FadeOut_OnUnloaded;
            }
        }

        static void FadeOut_OnUnloaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            FadeOut(frameworkElement);
        }

        static void FadeOut(FrameworkElement control)
        {
            var duration = GetFadeOutDuration(control);
            control.FadeOut(duration == default ? new Duration(TimeSpan.FromSeconds(0.5)) : duration);
        }

        #endregion

        #region FadeOutDuration

        public static readonly DependencyProperty FadeOutDurationProperty = DependencyProperty.RegisterAttached("FadeOutDuration", typeof(Duration), typeof(ControlExtensions), new UIPropertyMetadata(default(Duration)));
        public static Duration GetFadeOutDuration(FrameworkElement i) => (Duration)i.GetValue(FadeOutDurationProperty);
        public static void SetFadeOutDuration(FrameworkElement i, Duration value) => i.SetValue(FadeOutDurationProperty, value);

        #endregion

        /// -----------------------------------------------

        #region FontSizeWheel

        public static readonly DependencyProperty FontSizeWheelMaximumProperty = DependencyProperty.RegisterAttached("FontSizeWheelMaximum", typeof(int), typeof(ControlExtensions), new PropertyMetadata(32));
        public static void SetFontSizeWheelMaximum(Control i, int value) => i.SetValue(FontSizeWheelMaximumProperty, value);
        public static int GetFontSizeWheelMaximum(Control i) => (int)i.GetValue(FontSizeWheelMaximumProperty);

        public static readonly DependencyProperty FontSizeWheelMinimumProperty = DependencyProperty.RegisterAttached("FontSizeWheelMinimum", typeof(int), typeof(ControlExtensions), new PropertyMetadata(8));
        public static void SetFontSizeWheelMinimum(Control i, int value) => i.SetValue(FontSizeWheelMinimumProperty, value);
        public static int GetFontSizeWheelMinimum(Control i) => (int)i.GetValue(FontSizeWheelMinimumProperty);

        public static readonly DependencyProperty FontSizeWheelProperty = DependencyProperty.RegisterAttached("FontSizeWheel", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false, OnFontSizeWheelChanged));
        public static void SetFontSizeWheel(Control i, bool value) => i.SetValue(FontSizeWheelProperty, value);
        public static bool GetFontSizeWheel(Control i) => (bool)i.GetValue(FontSizeWheelProperty);
        static void OnFontSizeWheelChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as Control;
            control.PreviewMouseWheel -= FontSizeWheel_OnMouseWheel;
            if ((bool)e.NewValue)
                control.PreviewMouseWheel += FontSizeWheel_OnMouseWheel;
        }

        static void FontSizeWheel_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var control = sender as Control;
            if (ModifierKeys.Control.Pressed())
            {
                if (e.Delta > 0)
                {
                    if (control.FontSize + 1 <= GetFontSizeWheelMaximum(control))
                        control.SetCurrentValue(Control.FontSizeProperty, control.FontSize + 1);
                }
                else
                {
                    if (control.FontSize - 1 >= GetFontSizeWheelMinimum(control))
                        control.SetCurrentValue(Control.FontSizeProperty, control.FontSize - 1);
                }
                e.Handled = true;
            }
        }

        #endregion

        /// -----------------------------------------------

        #region Transform (incomplete...)

        static void SetupTransform(FrameworkElement frameworkElement)
        {
            if (frameworkElement != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
                if (adornerLayer != null)
                {
                    var adorners = adornerLayer.GetAdorners(frameworkElement);
                    adorners?.ForEach(i => adornerLayer.Remove(i));

                    if (GetIsResizable(frameworkElement))
                    {
                        var adorner = new TransformAdorner(frameworkElement, GetResizeSnap(frameworkElement), GetResizeThumbStyle(frameworkElement));
                        adornerLayer.Add(adorner);
                    }
                }
            }
        }

        #region Rotation

        public static readonly DependencyProperty RotationProperty = DependencyProperty.RegisterAttached("Rotation", typeof(double), typeof(ControlExtensions), new UIPropertyMetadata(0.0, OnRotationChanged));
        public static double GetRotation(FrameworkElement d)
        {
            return (double)d.GetValue(RotationProperty);
        }
        public static void SetRotation(FrameworkElement d, double value)
        {
            d.SetValue(RotationProperty, value);
        }
        static void OnRotationChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region Scale

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.RegisterAttached("Scale", typeof(double), typeof(ControlExtensions), new UIPropertyMetadata(1.0, OnScaleChanged));
        public static double GetScale(FrameworkElement d)
        {
            return (double)d.GetValue(ScaleProperty);
        }
        public static void SetScale(FrameworkElement d, double value)
        {
            d.SetValue(ScaleProperty, value);
        }
        static void OnScaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region IsResizable

        public static readonly DependencyProperty IsResizableProperty = DependencyProperty.RegisterAttached("IsResizable", typeof(bool), typeof(ControlExtensions), new UIPropertyMetadata(false, OnIsResizableChanged));
        public static bool GetIsResizable(FrameworkElement d)
        {
            return (bool)d.GetValue(IsResizableProperty);
        }
        public static void SetIsResizable(FrameworkElement d, bool value)
        {
            d.SetValue(IsResizableProperty, value);
        }
        static void OnIsResizableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetupTransform(sender as FrameworkElement);
        }

        #endregion

        #region ResizeSnap

        public static readonly DependencyProperty ResizeSnapProperty = DependencyProperty.RegisterAttached("ResizeSnap", typeof(double), typeof(ControlExtensions), new UIPropertyMetadata(8d, OnResizeSnapChanged));
        public static double GetResizeSnap(FrameworkElement d)
        {
            return (double)d.GetValue(ResizeSnapProperty);
        }
        public static void SetResizeSnap(FrameworkElement d, double value)
        {
            d.SetValue(ResizeSnapProperty, value);
        }
        static void OnResizeSnapChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetupTransform(sender as FrameworkElement);
        }

        #endregion

        #region ResizeThumbStyle

        public static readonly DependencyProperty ResizeThumbStyleProperty = DependencyProperty.RegisterAttached("ResizeThumbStyle", typeof(Style), typeof(ControlExtensions), new UIPropertyMetadata(default(Style), OnResizeThumbStyleChanged));
        public static Style GetResizeThumbStyle(FrameworkElement d)
        {
            return (Style)d.GetValue(ResizeThumbStyleProperty);
        }
        public static void SetResizeThumbStyle(FrameworkElement d, Style value)
        {
            d.SetValue(ResizeThumbStyleProperty, value);
        }
        static void OnResizeThumbStyleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetupTransform(sender as FrameworkElement);
        }

        #endregion

        #region RotateThumbStyle

        public static readonly DependencyProperty RotateThumbStyleProperty = DependencyProperty.RegisterAttached("RotateThumbStyle", typeof(Style), typeof(ControlExtensions), new UIPropertyMetadata(default(Style), OnRotateThumbStyleChanged));
        public static Style GetRotateThumbStyle(FrameworkElement d)
        {
            return (Style)d.GetValue(RotateThumbStyleProperty);
        }
        public static void SetRotateThumbStyle(FrameworkElement d, Style value)
        {
            d.SetValue(RotateThumbStyleProperty, value);
        }
        static void OnRotateThumbStyleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetupTransform(sender as FrameworkElement);
        }

        #endregion

        #endregion

        #endregion

        #region ControlExtensions

        static ControlExtensions()
        {
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDragEnterEvent, new System.Windows.DragEventHandler(OnDragOver), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDragLeaveEvent, new System.Windows.DragEventHandler(OnDragLeave), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDragOverEvent, new System.Windows.DragEventHandler(OnDragOver), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDropEvent, new System.Windows.DragEventHandler(OnDrop), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.MouseEnterEvent, new MouseEventHandler(OnMouseEnterLeave), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.MouseLeaveEvent, new MouseEventHandler(OnMouseEnterLeave), true);
            EventManager.RegisterClassHandler(typeof(Control), UpdateOverItemEvent, new RoutedEventHandler(OnUpdateOverItem));
        }

        static void OnDrop(object sender, DragEventArgs args)
        {
            lock (IsDraggingOverProperty)
            {
                isPossibleDropTarget = false;
                if (currentDropTarget != null)
                    currentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                var Item = sender as Control;
                if (Item != null)
                {
                    currentDropTarget = Item;
                    Item.InvalidateProperty(IsDraggingOverProperty);
                }
            }
        }

        /// <summary>
        /// Called when an item is dragged over the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        static void OnDragOver(object sender, DragEventArgs e)
        {
            lock (IsDraggingOverProperty)
            {
                isPossibleDropTarget = false;
                if (currentDropTarget != null)
                {
                    var OldItem = currentDropTarget;
                    currentDropTarget = null;
                    OldItem.InvalidateProperty(IsDraggingOverProperty);
                }

                if (e.Effects != DragDropEffects.None)
                    isPossibleDropTarget = true;

                var Control = sender as Control;
                if (Control != null)
                {
                    currentDropTarget = Control;
                    currentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                }
            }
        }

        /// <summary>
        /// Called when the drag cursor leaves the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        static void OnDragLeave(object sender, DragEventArgs args)
        {
            lock (IsDraggingOverProperty)
            {
                isPossibleDropTarget = false;
                if (currentDropTarget != null)
                {
                    var OldItem = currentDropTarget;
                    currentDropTarget = null;
                    OldItem.InvalidateProperty(IsDraggingOverProperty);
                }
                var Control = sender as Control;
                if (Control != null)
                {
                    currentDropTarget = Control;
                    currentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                }
            }
        }

        static void OnMouseEnterLeave(object sender, MouseEventArgs args)
        {
            lock (IsMouseDirectlyOverProperty)
            {
                if (currentControl != null)
                {
                    var OldItem = currentControl;
                    currentControl = null;
                    OldItem.InvalidateProperty(IsMouseDirectlyOverProperty);
                }
                //Get the element that is currently under the mouse.
                var CurrentPosition = Mouse.DirectlyOver;

                // See if the mouse is still over something.
                if (CurrentPosition != null)
                {
                    var RoutedEventArgs = new RoutedEventArgs(UpdateOverItemEvent);
                    CurrentPosition.RaiseEvent(RoutedEventArgs);
                }
            }
        }

        static readonly RoutedEvent UpdateOverItemEvent = EventManager.RegisterRoutedEvent("UpdateOverItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlExtensions));
        /// <summary>
        /// This method is a listener for the UpdateOverItemEvent.  
        /// When it is received, it means that the sender is the 
        /// closest item to the mouse (closest logically, not visually).
        /// </summary>
        static void OnUpdateOverItem(object sender, RoutedEventArgs args)
        {
            //Mark this object as the tree view item over which the mouse is currently positioned.
            currentControl = sender as Control;
            //Tell that item to recalculate
            currentControl.InvalidateProperty(IsMouseDirectlyOverProperty);
            //Prevent this event from notifying other items higher in tree
            args.Handled = true;
        }

        #endregion
    }
}