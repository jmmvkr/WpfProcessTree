using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VProcessWindow
{
    public class Global
    {
        public static Global instance;
        public IconCache processIcon;

        public static void init()
        {
            instance = makeOne();
        }

        static Global makeOne()
        {
            return new Global();
        }

    } // end - class Global
}
