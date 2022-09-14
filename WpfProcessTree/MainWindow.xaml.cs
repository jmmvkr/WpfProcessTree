using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        const string PTH_IGNORE = "ps-ignore.txt";
        const string PTH_SAVE = "ps-list.txt";

        ProcessModel psModel;
        IList<Node<ProcessStructure>> psList;
        Node<ProcessStructure>[] emptyList = { };
        Encoding enc = new UTF8Encoding(true, true);
        ISet<string> psIgnore = new HashSet<string>();
        TreeViewItem tviSelected;
        TextBlock txtSelected;


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            psList = emptyList;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            psModel = new ProcessModel();
            loadIgnoreGroups();
            refreshProcessTree(true);
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var k = e.Key;
            if (Key.F5 == k)
            {
                refreshProcessTree(true);
            }
            if (Key.S == k && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                saveProcessGroups();
            }
        }

        void loadIgnoreGroups()
        {
            string path = PTH_IGNORE;
            psIgnore.Clear();
            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    var trm = line.Trim();
                    if (String.IsNullOrEmpty(trm)) continue;
                    psIgnore.Add(trm);
                }
            }
        }

        void saveProcessGroups()
        {
            string path = PTH_SAVE;
            StringBuilder sb = new StringBuilder();
            foreach (var grp in psList)
            {
                sb.AppendLine(grp.val.name);
            }
            File.WriteAllText(path, sb.ToString(), enc);
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
            if (e.Key == Key.Return)
            {
                TreePath.setTreeExpansion(xTree, true);
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
                psList = psModel.updateTree(psIgnore);
            }
            // apply filter string
            xTree.ItemsSource = psModel.filter(psList, txtFilter.Text);

            // focus on previous selected process
            TreeFocus.focusOnNode(xTree, prevNode, psList);
            if (!bUpdateTree)
            {
                txtFilter.Focus();
            }
        }

        private void XTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = null;
            var sel = e.NewValue;
            if (null != sel)
            {
                tvi = TreeFocus.findTreeUi(xTree, sel);
            }
            if (null != tvi)
            {
                tviSelected = tvi;
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (2 == e.ClickCount)
            {
                bringProcessToFront(sender);
            }
            if (1 == e.ClickCount)
            {
                txtSelected = sender as TextBlock;
            }
        }

        void bringProcessToFront(object sender)
        {
            IntPtr pWindow = IntPtr.Zero;
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

        private void XMenuItem_BrintToFront_Click(object sender, RoutedEventArgs e)
        {
            bringProcessToFront(txtSelected);
        }

    } // end - class MainWindow
}
