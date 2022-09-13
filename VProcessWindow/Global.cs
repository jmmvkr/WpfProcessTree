using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VData;

namespace VProcessWindow
{
    public class Global
    {
        public static Global instance;
        public IconCache processIcon;
        public TicketSystem<int, ProcessWindow.WindowScan> ticketSystem;

        public static void init()
        {
            instance = makeOne();
        }

        static Global makeOne()
        {
            var g = new Global();
            g.processIcon = new IconCache();
            g.ticketSystem = new TicketSystem<int, ProcessWindow.WindowScan>() { makeNextTicket = ProcessWindow.makeTicket };
            return g;
        }

    } // end - class Global
}
