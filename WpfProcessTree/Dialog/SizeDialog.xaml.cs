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
        }

        private void SizeDialog_Loaded(object sender, RoutedEventArgs e)
        {
            xConfirm.btnOk.Click += BtnOk_Click;
            xConfirm.btnCancel.Click += BtnCancel_Click;
            txtWidth.KeyDown += Txt_KeyDown;
            txtHeight.KeyDown += Txt_KeyDown;

            this.PreviewKeyDown += SizeDialog_PreviewKeyDown;
            xConfirm.btnOk.KeyDown += BtnOk_KeyDown;
        }

        private void BtnOk_KeyDown(object sender, KeyEventArgs e)
        {
            usePresetByKey(e.Key);
        }

        private void usePresetByKey(Key k)
        {
            int idxPreset;
            if (Key.D0 <= k && k <= Key.D9)
            {
                idxPreset = (k - Key.D0);
                callPreset(idxPreset);
            }
        }

        int callPreset(int idxPreset)
        {
            switch (idxPreset)
            {
                case 0: return takeSize(1920, 1080);
                case 1: return takeSize(1634, 934);
                case 2: return takeSize(1280, 720);
                case 3: return takeSize(800, 600);
            }
            return 0;
        }

        int takeSize(double w, double h)
        {
            txtWidth.Text = String.Format("{0:f0}", w);
            txtHeight.Text = String.Format("{0:f0}", h);
            return 0;
        }

        private void SizeDialog_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool bCenterPreset = (Key.C == e.Key);
            if ((Key.P == e.Key) || bCenterPreset)
            {
                cbCenter.IsChecked = (bCenterPreset);
                xConfirm.btnOk.Focus();
            }
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
                takeSize(param.size.Width, param.size.Height);
                cbCenter.IsChecked = false;
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
                    result.moveCenter = (true == cbCenter.IsChecked);
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
            public bool moveCenter;
        }

    } // end - class SizeDialog
}
