using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Imagin.Common.Configuration
{
    public static class Computer
    {
        #region Methods

        #region Imports

        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        [DllImport("user32")]
        public static extern void LockWorkStation();

        #endregion

        #region Static 

        public static void Hibernate()
        {
            SetSuspendState(true, true, true);
        }

        public static void Lock()
        {
            LockWorkStation();
        }

        public static void Restart()
        {
            System.Diagnostics.Process.Start("shutdown", "/r /t 0");
        }

        public static void SignOff()
        {
            ExitWindowsEx(0, 0);
        }

        public static void Shutdown()
        {
            System.Diagnostics.Process.Start("shutdown", "/s /t 0");
        }

        public static void Sleep()
        {
            SetSuspendState(false, true, true);
        }

        #endregion

        #endregion

        #region Screen

        public static class Screen
        {
            #region Methods

            #region Imports

            [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll")]
            private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

            #endregion

            #region Static

            public static Bitmap CaptureDesktop()
            {
                return CaptureWindow(GetDesktopWindow());
            }

            public static Bitmap CaptureForegroundWindow()
            {
                return CaptureWindow(GetForegroundWindow());
            }

            public static Bitmap CaptureWindow(IntPtr handle)
            {
                try
                {
                    var rect = new Rect();
                    GetWindowRect(handle, ref rect);
                    var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                    var result = new Bitmap(bounds.Width, bounds.Height);

                    using (var graphics = Graphics.FromImage(result))
                    {
                        graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
                    }

                    return result;
                }
                catch
                {
                    return null;
                }
            }

            public static IntPtr GetProcessHandle()
            {
                System.Diagnostics.Process[] Processes = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process p in Processes)
                {
                    IntPtr windowHandle = p.MainWindowHandle;
                    // do something with windowHandle
                }
                return new IntPtr();
            }

            #endregion

            #endregion

            #region Structs

            [StructLayout(LayoutKind.Sequential)]
            private struct Rect
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            #endregion
        }

        #endregion
    }
}