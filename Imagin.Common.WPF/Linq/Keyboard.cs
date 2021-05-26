using Imagin.Common.Input;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class KeyboardExtensions
    {
        public static bool IsAnyKeyDown(params Key[] Keys)
        {
            if (Keys?.Length > 0)
            {
                foreach (var i in Keys)
                {
                    if (Keyboard.IsKeyDown(i))
                        return true;
                }
            }
            return false;
        }

        public static bool IsKeyModifyingPopupState(KeyEventArgs e) => ((((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) && ((e.SystemKey == Key.Down) || (e.SystemKey == Key.Up))) || (e.Key == Key.F4));

        #region public static char ToChar(this Key key)

        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char ToChar(this Key key)
        {
            var result = '\0';

            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            var scan = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            var builder = new StringBuilder(2);

            var unicode = ToUnicode((uint)virtualKey, scan, keyboardState, builder, builder.Capacity, 0);
            switch (unicode)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    result = builder[0];
                    break;
                default:
                    result = builder[0];
                    break;
            }
            return result;
        }

        #endregion
    }
}