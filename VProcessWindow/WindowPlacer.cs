using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VProcessWindow
{
    public struct WindowPlacer
    {
        IntPtr pHandle;

        public static WindowPlacer fromHandle(IntPtr hWnd)
        {
            WindowPlacer wp;
            wp.pHandle = hWnd;
            return wp;
        }


        const int HWND_BOTTOM = 1;
        const int HWND_NOTOPMOST = -2;
        const int HWND_TOP = 0;
        const int HWND_TOPMOST = -1;
        const int SW_RESTORE = 9;


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // window movement

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hNull, int x, int y, int w, int h, int flags);


        public void bringToFront()
        {
            var wnd = pHandle;
            if (IsIconic(wnd))
            {
                ShowWindow(wnd, SW_RESTORE);
            }
            SetForegroundWindow(wnd);
        }

        public Rect getRect()
        {
            Rect ret = Rect.Parse("0,0,0,0");
            RECT r;
            if (GetWindowRect(pHandle, out r))
            {
                var x = r.Left;
                var y = r.Top;
                var w = r.Right - x;
                var h = r.Bottom - y;
                ret.X = x;
                ret.Y = y;
                ret.Width = w;
                ret.Height = h;
            }
            return ret;
        }

        public void moveTo(double x, double y, double w, double h)
        {
            var hwnd = pHandle;

            int nx = (int)x;
            int ny = (int)y;
            int nw = (int)w;
            int nh = (int)h;

            var zOrder = IntPtr.Zero + HWND_NOTOPMOST;
            MoveWindow(hwnd, 0, 0, nw, nh, true);
            SetWindowPos(hwnd, zOrder, nx, ny, 0, 0, 0x01);
        }

        static Point centerOf(double w, double h, double sw, double sh)
        {
            Point p = Point.Parse("0,0");
            p.X = Math.Max(0.0, 0.5 * (sw - w));
            p.Y = Math.Max(0.0, 0.5 * (sh - h));
            return p;
        }

        public void moveToCenter(double screenWidth, double screenHeight)
        {
            RECT r;
            var hwnd = pHandle;
            if (GetWindowRect(hwnd, out r))
            {
                var w = r.Right - r.Left;
                var h = r.Bottom - r.Top;
                Point center = centerOf(w, h, screenWidth, screenHeight);
                moveTo(center.X, center.Y, w, h);
            }
        }

        public void resizeTo(double w, double h)
        {
            RECT r;
            var hwnd = pHandle;
            if (GetWindowRect(hwnd, out r))
            {
                moveTo(r.Left, r.Top, w, h);
            }
        }

    } // end - class WindowPlacer

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    } // end - struct RECT
}
