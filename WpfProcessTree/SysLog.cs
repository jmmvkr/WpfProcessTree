using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProcessTree
{
    public class SysLog
    {
        public static void printMsg(Exception ex)
        {
            Console.WriteLine(ex.GetType().ToString());
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
}
