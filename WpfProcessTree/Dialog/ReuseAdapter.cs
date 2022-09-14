using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfProcessTree.Dialog
{
    public class ReuseAdapter<T> where T : Window
    {
        T _ins;
        bool bDestorying = false;
        public T ins { get { return _ins; } }

        public ReuseAdapter(T pIns)
        {
            _ins = pIns;
            pIns.PreviewKeyDown += PIns_PreviewKeyDown;
            pIns.Closing += PIns_Closing;
        }

        void PIns_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var w = sender as Window;
            if (w != _ins) return;

            if (!bDestorying)
            {
                w.Visibility = Visibility.Hidden;
                e.Cancel = true;
            }
        }

        void PIns_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            _ins.Close();
        }

        public void destory()
        {
            bDestorying = true;
            _ins.Close();
        }

        public bool showDialog()
        {
            return showDialog(null);
        }

        public bool showDialog(Window wParent)
        {
            bool bOk;
            var w = _ins;
            if (null == wParent)
            {
                bOk = (true == w.ShowDialog());
            }
            else
            {
                w.Owner = wParent;
                bOk = (true == w.ShowDialog());
                w.Owner = null;
            }
            return bOk;
        }
    }

}
