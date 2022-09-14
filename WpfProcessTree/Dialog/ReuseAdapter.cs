using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfProcessTree.Dialog
{
    public class ReuseAdapter<T> : IDialog where T : Window
    {
        T _ins;
        bool bDestorying = false;
        bool bOk = false;
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
            if (e.Key == Key.Escape)
            {
                _ins.Close();
                e.Handled = true;
            }
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
            var w = _ins;

            IDialogControl ctrl = w as IDialogControl;
            if (null != ctrl)
            {
                ctrl.dlg = this;
            }

            bOk = false;
            if (null == wParent)
            {
                w.ShowDialog();
            }
            else
            {
                w.Owner = wParent;
                w.ShowDialog();
                w.Owner = null;
            }
            return bOk;
        }

        public void cancel()
        {
            bOk = false;
            _ins.Close();
        }

        public void ok()
        {
            bOk = true;
            _ins.Close();
        }

    } // end - class ReuseAdapter

}
