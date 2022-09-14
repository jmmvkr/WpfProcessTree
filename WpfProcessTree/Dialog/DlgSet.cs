using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfProcessTree.Dialog
{
    public class DlgSet : IDisposable
    {
        public ReuseAdapter<SizeDialog> dlgSize = ReuseDialog.fromIns(new SizeDialog());

        static DlgSet _sharedIns;

        public static void init()
        {
            _sharedIns = new DlgSet();
        }

        public static DlgSet ins()
        {
            return _sharedIns;
        }

        public void Dispose()
        {
            releaseWindow(dlgSize);
        }

        void releaseWindow<W>(ReuseAdapter<W> dlg) where W : Window
        {
            dlg.destory();
        }
    }

}
