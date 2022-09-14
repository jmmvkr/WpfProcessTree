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

        public SizeDialog()
        {
            InitializeComponent();
            this.Loaded += SizeDialog_Loaded;
            this.IsVisibleChanged += Dialog_IsVisibleChanged;
        }

        private void SizeDialog_Loaded(object sender, RoutedEventArgs e)
        {
            xConfirm.btnOk.Click += BtnOk_Click;
            xConfirm.btnCancel.Click += BtnCancel_Click;
        }

        private void Dialog_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {

            }
        }

        void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            dlg.ok();
        }

        void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            dlg.cancel();
        }

    } // end - class SizeDialog
}
