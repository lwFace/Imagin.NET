using Imagin.Common.Native;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Imagin.Common.Linq
{
    public static class WindowExtensions
    {
        #region Properties

        #region MinimizeCommand

        public static readonly RoutedUICommand MinimizeCommand = new RoutedUICommand(nameof(MinimizeCommand), nameof(MinimizeCommand), typeof(WindowExtensions));
        static void OnMinimize(object sender, ExecutedRoutedEventArgs e) => SystemCommands.MinimizeWindow(sender as Window);
        static void OnCanMinimize(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        #endregion

        #region MaximizeCommand

        public static readonly RoutedUICommand MaximizeCommand = new RoutedUICommand(nameof(MaximizeCommand), nameof(MaximizeCommand), typeof(WindowExtensions));
        static void OnMaximize(object sender, ExecutedRoutedEventArgs e) => SystemCommands.MaximizeWindow(sender as Window);
        static void OnCanMaximize(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanMaximize(sender as Window);

        #endregion

        #region RestoreCommand

        public static readonly RoutedUICommand RestoreCommand = new RoutedUICommand(nameof(RestoreCommand), nameof(RestoreCommand), typeof(WindowExtensions));
        static void OnRestore(object sender, ExecutedRoutedEventArgs e) => SystemCommands.RestoreWindow(sender as Window);
        static void OnCanRestore(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanRestore(sender as Window);

        #endregion

        #region CloseCommand

        public static readonly RoutedUICommand CloseCommand = new RoutedUICommand(nameof(CloseCommand), nameof(CloseCommand), typeof(WindowExtensions));
        static void OnClose(object sender, ExecutedRoutedEventArgs e) => SystemCommands.CloseWindow(sender as Window);
        static void CanClose(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        #endregion

        /// ------------------------------------------------------------------------------

        #region Always On Bottom

        #region Bottom

        public class Bottom
        {
            #region Properties

            const UInt32 SWP_NOSIZE = 0x0001;

            const UInt32 SWP_NOMOVE = 0x0002;

            const UInt32 SWP_NOACTIVATE = 0x0010;

            const UInt32 SWP_NOZORDER = 0x0004;

            const int WM_ACTIVATEAPP = 0x001C;

            const int WM_ACTIVATE = 0x0006;

            const int WM_SETFOCUS = 0x0007;

            const int WM_WINDOWPOSCHANGING = 0x0046;

            static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

            Window Window = null;

            #endregion

            #region WindowSinker

            public Bottom(Window Window)
            {
                this.Window = Window;
            }

            #endregion

            #region Methods

            [DllImport("user32.dll")]
            static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

            [DllImport("user32.dll")]
            static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

            [DllImport("user32.dll")]
            static extern IntPtr BeginDeferWindowPos(int nNumWindows);

            [DllImport("user32.dll")]
            static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

            void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                var Handle = (new WindowInteropHelper(Window)).Handle;

                var Source = HwndSource.FromHwnd(Handle);
                Source.RemoveHook(new HwndSourceHook(WndProc));
            }

            void OnLoaded(object sender, RoutedEventArgs e)
            {
                var Hwnd = new WindowInteropHelper(Window).Handle;
                SetWindowPos(Hwnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);

                var Handle = (new WindowInteropHelper(Window)).Handle;

                var Source = HwndSource.FromHwnd(Handle);
                Source.AddHook(new HwndSourceHook(WndProc));
            }

            IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                if (msg == WM_SETFOCUS)
                {
                    hWnd = new WindowInteropHelper(Window).Handle;
                    SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
                    handled = true;
                }
                return IntPtr.Zero;
            }

            public void Sink()
            {
                Window.Loaded += OnLoaded;
                Window.Closing += OnClosing;
            }

            public void Unsink()
            {
                Window.Loaded -= OnLoaded;
                Window.Closing -= OnClosing;
            }

            #endregion
        }

        #endregion

        public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottommost", typeof(Bottom), typeof(WindowExtensions), new UIPropertyMetadata(null));
        public static Bottom GetBottom(DependencyObject d)
            => (Bottom)d.GetValue(BottomProperty);
        public static void SetBottom(DependencyObject d, Bottom value)
            => d.SetValue(BottomProperty, value);

        public static readonly DependencyProperty AlwaysOnBottomProperty = DependencyProperty.RegisterAttached("AlwaysOnBottom", typeof(bool), typeof(WindowExtensions), new UIPropertyMetadata(false, OnAlwaysOnBottomChanged));
        public static bool GetAlwaysOnBottom(DependencyObject d)
            => (bool)d.GetValue(AlwaysOnBottomProperty);
        public static void SetAlwaysOnBottom(DependencyObject d, bool value)
            => d.SetValue(AlwaysOnBottomProperty, value);
        static void OnAlwaysOnBottomChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window i)
            {
                if ((bool)e.NewValue)
                {
                    var bottom = new Bottom(i);
                    bottom.Sink();
                    SetBottom(i, bottom);
                }
                else
                {
                    var bottom = GetBottom(i);
                    bottom?.Unsink();
                    SetBottom(i, null);
                }
            }
        }

        #endregion

        #region AutoCenter

        /// <summary>
        /// Gets or sets value indicating whether window should position itself relative to center of the screen.
        /// </summary>
        public static readonly DependencyProperty AutoCenter = DependencyProperty.RegisterAttached("AutoCenter", typeof(bool), typeof(WindowExtensions), new UIPropertyMetadata(false, OnAutoCenterChanged));
        public static bool GetAutoCenter(DependencyObject d)
            => (bool)d.GetValue(AutoCenter);
        public static void SetAutoCenter(DependencyObject d, bool value)
            => d.SetValue(AutoCenter, value);
        static void OnAutoCenterChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window i && (bool)e.NewValue)
            {
                i.SizeChanged += (a, b) =>
                {
                    if (b.HeightChanged)
                        i.Top += (b.PreviousSize.Height - b.NewSize.Height) / 2;
                    if (b.WidthChanged)
                        i.Left += (b.PreviousSize.Width - b.NewSize.Width) / 2;
                };

            }
        }

        #endregion

        #region Immovable

        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        public static readonly DependencyProperty ImmovableProperty = DependencyProperty.RegisterAttached("Immovable", typeof(bool), typeof(WindowExtensions), new PropertyMetadata(false, OnImmovableChanged));
        public static bool GetImmovable(Window d)
            => (bool)d.GetValue(ImmovableProperty);
        public static void SetImmovable(Window d, bool value)
            => d.SetValue(ImmovableProperty, value);
        static void OnImmovableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window i)
            {
                i.SourceInitialized -= Immovable_OnWindowSourceInitialized;
                if ((bool)e.OldValue)
                    i.SourceInitialized += Immovable_OnWindowSourceInitialized;
            }
        }

        static void Immovable_OnWindowSourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper((Window)sender);
            HwndSource source = HwndSource.FromHwnd(helper.Handle);
            source.AddHook(WndProc);
        }

        static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion

        #region IsChild

        public static readonly DependencyProperty IsChildProperty = DependencyProperty.RegisterAttached("IsChild", typeof(bool), typeof(WindowExtensions), new PropertyMetadata(false, OnIsChildChanged));
        public static bool GetIsChild(DependencyObject d)
            => (bool)d.GetValue(IsChildProperty);
        public static void SetIsChild(DependencyObject d, bool value)
            => d.SetValue(IsChildProperty, value);
        static void OnIsChildChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window i)
                i.Owner = (bool)e.NewValue ? Application.Current.MainWindow : null;
        }

        #endregion

        #region IsDragMoveEnabled

        /// <summary>
        /// Gets or sets value indicating whether window should allow drag moving.
        /// </summary>
        public static readonly DependencyProperty IsDragMoveEnabled = DependencyProperty.RegisterAttached("IsDragMoveEnabled", typeof(bool), typeof(WindowExtensions), new UIPropertyMetadata(true, OnIsDragMoveEnabledChanged));
        public static bool GetIsDragMoveEnabled(DependencyObject d)
            => (bool)d.GetValue(IsDragMoveEnabled);
        public static void SetIsDragMoveEnabled(DependencyObject d, bool value)
            => d.SetValue(IsDragMoveEnabled, value);
        static void OnIsDragMoveEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Window i && !((bool)e.NewValue))
            {
                i.SourceInitialized += (a, b) =>
                {
                    WindowInteropHelper helper = new WindowInteropHelper(i);
                    HwndSource Source = HwndSource.FromHwnd(helper.Handle);
                    Source.AddHook(WndProc);
                };
            }
        }

        #endregion

        /// ------------------------------------------------------------------------------

        #region Extends

        /// <summary>
        /// Gets or sets a value indicating whether window should implement default behavior; this is only applicable when window
        /// a) overrides default style,
        /// b) allows transparency, and
        /// c) style is set to 'None'.
        /// </summary>
        public static readonly DependencyProperty ExtendsProperty = DependencyProperty.RegisterAttached("Extends", typeof(bool), typeof(WindowExtensions), new PropertyMetadata(false, OnExtendsChanged));
        public static bool GetExtends(Window d)
            => (bool)d.GetValue(ExtendsProperty);
        public static void SetExtends(Window d, bool value)
            => d.SetValue(ExtendsProperty, value);
        static void OnExtendsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            window.SourceInitialized -= Extends_OnSourceInitialized;

            if ((bool)e.NewValue)
            {
                window.SourceInitialized += Extends_OnSourceInitialized;
            }
        }

        static void Extends_OnSourceInitialized(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.SourceInitialized -= Extends_OnSourceInitialized;

            var handle = (new WindowInteropHelper(window)).Handle;
            HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));

            window.CommandBindings.Add(new CommandBinding(MinimizeCommand, OnMinimize, OnCanMinimize));
            window.CommandBindings.Add(new CommandBinding(MaximizeCommand, OnMaximize, OnCanMaximize));
            window.CommandBindings.Add(new CommandBinding(RestoreCommand, OnRestore, OnCanRestore));
            window.CommandBindings.Add(new CommandBinding(CloseCommand, OnClose, CanClose));
        }

        #endregion

        #endregion

        #region Methods

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        /// ------------------------------------------------------------------------------

        static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            var MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {

                var monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);

                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = System.Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = System.Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = System.Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = System.Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        /// ------------------------------------------------------------------------------

        public static double ActualLeft(this Window input)
        {
            if (input.WindowState == WindowState.Maximized)
            {
                var field = typeof(Window).GetField("_actualLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double)field.GetValue(input);
            }
            else return input.Left;
        }

        public static double ActualTop(this Window input)
        {
            if (input.WindowState == WindowState.Maximized)
            {
                var field = typeof(Window).GetField("_actualTop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double)field.GetValue(input);
            }
            else return input.Top;
        }

        public static bool CanMaximize(this Window window) => window != null && window.WindowStyle != WindowStyle.ToolWindow && window.WindowState != WindowState.Maximized && window.ResizeMode != ResizeMode.NoResize;

        public static void Center(this Window input)
        {
            input.Left = (SystemParameters.PrimaryScreenWidth / 2.0) - (input.Width / 2.0);
            input.Top = (SystemParameters.PrimaryScreenHeight / 2.0) - (input.Height / 2.0);
        }

        public static bool CanRestore(this Window window) => window != null && window.WindowStyle != WindowStyle.ToolWindow && window.WindowState != WindowState.Normal && window.ResizeMode != ResizeMode.NoResize;

        #endregion
    }
}