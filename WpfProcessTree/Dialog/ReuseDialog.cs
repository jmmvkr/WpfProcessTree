using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfProcessTree.Dialog
{
    public class ReuseDialog
    {
        public const bool Ok = true;
        public const bool Cancel = false;

        public static ReuseAdapter<U> fromIns<U>(U u) where U : Window
        {
            ReuseAdapter<U> ra = new ReuseAdapter<U>(u);
            return ra;
        }
    }

}
