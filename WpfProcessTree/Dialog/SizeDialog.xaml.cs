using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfProcessTree.Dialog
{
    /// <summary>
    /// Interaction logic for SizeDialog.xaml
    /// </summary>
    public partial class SizeDialog : Window, IDialogControl
    {
        public IDialog dlg { get; set; }

        public Param param;
        public Param result;


        public SizeDialog()
        {
            InitializeComponent();
            this.Loaded += SizeDialog_Loaded;
            this.IsVisibleChanged += Dialog_IsVisibleChanged;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
        }

        private void SizeDialog_Loaded(object sender, RoutedEventArgs e)
        {
            xConfirm.btnOk.Click += BtnOk_Click;
            xConfirm.btnCancel.Click += BtnCancel_Click;
            txtWidth.KeyDown += Txt_KeyDown;
            txtHeight.KeyDown += Txt_KeyDown;
        }

        private void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Return == e.Key) { BtnOk_Click(null, null); }
        }

        private void Dialog_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                ReuseDialog.moveToCenter(Owner, this);
                txtWidth.Text = String.Format("{0:f0}", param.size.Width);
                txtHeight.Text = String.Format("{0:f0}", param.size.Height);
            }
        }

        void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            int w;
            int h;
            if (Int32.TryParse(txtWidth.Text, out w) && Int32.TryParse(txtHeight.Text, out h))
            {
                if (w > 0 && h > 0)
                {
                    result.size.Width = w;
                    result.size.Height = h;
                    dlg.ok();
                }
            }
        }

        void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            dlg.cancel();
        }

        public struct Param
        {
            public Size size;
        }

    } // end - class SizeDialog
}
