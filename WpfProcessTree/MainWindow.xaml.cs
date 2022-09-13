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
using VProcessWindow.Example;

namespace WpfProcessTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static ProcessModel psModel;
        IList<Node<ProcessStructure>> psList;


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            psModel = new ProcessModel();
            psList = psModel.updateTree();
            xTree.ItemsSource = psList;
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            xTree.ItemsSource = psModel.filter(psList, txtFilter.Text);
        }

        private void TxtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                (sender as TextBox).Text = "";
            }
        }

        private void XTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
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
                    }
                }
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
