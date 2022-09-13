using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VData;
using VProcessWindow.Example;

namespace WpfProcessTree
{
    public class TreeFocus
    {
        public static void focusOnNode(TreeView xTree, object sel, IList<Node<ProcessStructure>> psList)
        {
            var selNode = sel as Node<ProcessStructure>;
            //var selNode = psList[0].subNodes[0];
            if (null != selNode)
            {
                TreeViewItem tviFound = null;
                TreeViewItem tviSubFound = null;

                int pid = selNode.val.pid;
                Node<ProcessStructure> newTreeNode = null;
                Node<ProcessStructure> newParentNode = null;

                // find node and parent node
                foreach (var one in psList)
                {
                    if (null != newTreeNode) break;
                    foreach (var ps in one.subNodes)
                    {
                        if (pid == ps.__value.pid)
                        {
                            newTreeNode = ps; // find node, in order to set focus of TreeViewItem
                            newParentNode = one; // find parent node, in order to get parent TreeViewItem
                            break;
                        }
                    }
                }

                if (null != newParentNode)
                {
                    // find parent TreeViewItem
                    tviFound = findTreeItem(xTree, newParentNode);
                    if (null != tviFound && tviFound.HasItems)
                    {
                        tviFound.IsExpanded = true;
                        tviFound.UpdateLayout();

                        // find node TreeViewItem
                        tviSubFound = findTreeItem(tviFound, newTreeNode);
                    }
                }
                if (null != tviSubFound)
                {
                    tviSubFound.Focus();
                }
            }
        }

        static TreeViewItem findTreeItem(ItemsControl ic, object v)
        {
            TreeViewItem tvi = v as TreeViewItem;
            if (null != tvi)
            {
                return tvi;
            }

            if (ic.HasItems)
            {
                var ig = ic.ItemContainerGenerator;
                foreach (var sub in ic.Items)
                {
                    if (v == sub)
                    {
                        var subTvi = ig.ContainerFromItem(v) as TreeViewItem;
                        return subTvi;
                    }
                }
            }
            return null;
        }

    } // end - class TreeFocus
}
