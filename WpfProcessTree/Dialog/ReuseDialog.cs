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

        public static void moveToCenter(Window pRef, Window pWnd)
        {
            Window pOwner = Window.GetWindow(pRef);
            double cx = pOwner.Left + 0.5 * (pOwner.Width - pWnd.Width);
            double cy = pOwner.Top + 0.5 * (pOwner.Height - pWnd.Height);
            pWnd.Left = Math.Max(0.0, cx);
            pWnd.Top = Math.Max(0.0, cy);
        }
    }

}
