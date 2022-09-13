using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VProcessWindow
{
    public class ProcessWindow
    {
        delegate bool EnumedWindowProc(IntPtr handleWindow, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumedWindowProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr windowHandle, out int pid);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetWindowTextW(IntPtr windowHandle, StringBuilder sb, int len);

        public static IList<WindowInfo> scanAllWindows()
        {
            throw new NotImplementedException();
        }

        public struct WindowInfo
        {
            public int pid;
            public string title;
        }

    } // end - class ProcessWindow
}
