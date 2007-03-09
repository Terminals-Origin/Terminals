using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals
{
    class NativeApi
    {
        public struct COPYDATASTRUCT
        {
            public int dwData;
            public int cbData;
            public IntPtr lpData;
        }

        public const short WM_COPYDATA = 74;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

        public static IntPtr PinObject(object obj)
        {
            GCHandle gcHandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            return gcHandle.AddrOfPinnedObject();
        }
    }
}
