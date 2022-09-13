using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VData;
using VProcessWindow;
using VProcessWindow.Example;

namespace WpfProcessTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProcessModel psModel;
        IList<Node<ProcessStructure>> psList;


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            psModel = new ProcessModel();
            refreshProcessTree(true);
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.F5 == e.Key)
            {
                refreshProcessTree(true);
            }
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            refreshProcessTree(false);
        }

        private void TxtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                (sender as TextBox).Text = "";
            }
        }

        void refreshProcessTree(bool bUpdateTree)
        {
            // store previous node, for id of selected process
            var prevNode = xTree.SelectedItem;

            // refresh tree, if required
            if (bUpdateTree)
            {
                xTree.ItemsSource = null;
                psList = psModel.updateTree();
            }
            // apply filter string
            xTree.ItemsSource = psModel.filter(psList, txtFilter.Text);

            // focus on previous selected process
            if (bUpdateTree)
            {
                TreeFocus.focusOnNode(xTree, prevNode, psList);
            }
        }

        private void XTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IntPtr pWindow = IntPtr.Zero;
            if (2 == e.ClickCount)
            {
                var tx = sender as TextBlock;
                string pidString = Convert.ToString(tx.Tag);
                int pid;

                if (Int32.TryParse(pidString, out pid))
                {
                    Process p;
                    if (tryGetProcess(pid, out p))
                    {
                        //WindowPlacer wp = WindowPlacer.fromHandle(p.MainWindowHandle);
                        //wp.moveToCenter();
                        try
                        {
                            pWindow = p.MainWindowHandle;
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            }
            if (IntPtr.Zero != pWindow)
            {
                WindowPlacer wp = WindowPlacer.fromHandle(pWindow);
                wp.BringToFront();
            }
        }

        bool tryGetProcess(int pid, out Process p)
        {
            Process res = null;
            bool bFound = false;
            try
            {
                res = Process.GetProcessById(pid);
                bFound = true;
            }
            catch (Exception) { }
            p = res;
            return bFound;
        }

    } // end - class MainWindow
}
