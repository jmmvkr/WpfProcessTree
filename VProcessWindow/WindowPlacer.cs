using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        public void BringToFront()
        {
            SetForegroundWindow(pHandle);
        }

    } // end - class WindowPlacer
}
