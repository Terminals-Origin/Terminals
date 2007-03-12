// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Runtime.InteropServices;

namespace Crownwood.Magic.Win32
{
    public class User32
    {
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
            
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int newLong);
            
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int bRetValue, uint fWinINI);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool AnimateWindow(IntPtr hWnd, uint dwTime, uint dwFlags);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, uint cursor);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetFocus();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool WaitMessage();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TranslateMessage(ref MSG msg);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DispatchMessage(ref MSG msg);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint SendMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax);
	
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, uint flags);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT pt);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT pt);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENTS tme);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern ushort GetKeyState(int virtKey);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DrawFocusRect(IntPtr hWnd, ref RECT rect);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool HideCaret(IntPtr hWnd);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ShowCaret(IntPtr hWnd);
    }
}