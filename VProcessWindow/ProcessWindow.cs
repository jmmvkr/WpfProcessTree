using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VData;

namespace VProcessWindow
{
    public class ProcessWindow
    {
        delegate bool EnumedWindowProc(IntPtr windowHandle, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumedWindowProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr windowHandle, out int pid);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetWindowTextW(IntPtr windowHandle, StringBuilder sb, int len);


        public static IList<WindowInfo> scanAllWindows()
        {
            var tks = Global.instance.ticketSystem;
            var t = tks.next();
            EnumedWindowProc callBackPtr = new EnumedWindowProc(onScanWindow);
            EnumWindows(callBackPtr, t.Id);
            tks.destory(t);
            return t.infoList;
        }

        public static bool onScanWindow(IntPtr hwnd, int lParam)
        {
            var tks = Global.instance.ticketSystem;
            var t = tks.map[lParam];

            int szBuffer = 4096;
            int pid = 0;
            StringBuilder sb = new StringBuilder(szBuffer);

            WindowInfo info;
            int szTitle = GetWindowTextW(hwnd, sb, szBuffer);
            GetWindowThreadProcessId(hwnd, out pid);
            if ((0 != pid) && (szTitle > 0))
            {
                info.pid = pid;
                info.title = sb.ToString();
                t.infoList.Add(info);
            }

            return true;
        }

        internal static WindowScan makeTicket(TicketSystem<int, WindowScan> tks)
        {
            int nNextId = tks.roll(1000);
            WindowScan sc = new WindowScan() {  ticketId = nNextId };
            return sc;
        }

        public class WindowScan : ITicket<int>
        {
            public int ticketId;
            public List<WindowInfo> infoList = new List<WindowInfo>();
            public StringBuilder buffer;

            public int Id { get { return ticketId; } }
        }

        public struct WindowInfo
        {
            public int pid;
            public string title;
        }

    } // end - class ProcessWindow
}
