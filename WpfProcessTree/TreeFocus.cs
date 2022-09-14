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
                int pid = selNode.val.pid;
                Node<ProcessStructure> newTreeNode = null;
                Node<ProcessStructure> newParentNode = null;

                // find node and parent node
                foreach (var grp in psList)
                {
                    if (null != newTreeNode) break;
                    foreach (var ps in grp.subNodes)
                    {
                        if (pid == ps.__value.pid)
                        {
                            newTreeNode = ps; // find node, in order to set focus of TreeViewItem
                            newParentNode = grp; // find parent node, in order to get parent TreeViewItem
                            break;
                        }
                    }
                }

                // update tree focus by TreePath
                if (null != newTreeNode)
                {
                    TreePath tPath = new TreePath();
                    tPath.add(newTreeNode);
                    tPath.add(newParentNode);
                    tPath.setTreeFocus(xTree);
                }
            }
        }

    } // end - class TreeFocus

    internal class TreePath
    {
        List<object> pathList = new List<object>();

        public void add(object o)
        {
            pathList.Insert(0, o);
        }

        public static void setTreeExpansion(ItemsControl ic, bool bExpand)
        {
            TreeViewItem tvi = ic as TreeViewItem;
            if (null != tvi)
            {
                tvi.IsExpanded = bExpand;
            }

            if (ic.HasItems)
            {
                foreach (var sub in ic.Items)
                {
                    TreeViewItem subTvi = sub as TreeViewItem;
                    if (null != subTvi)
                    {
                        setTreeExpansion(subTvi, bExpand);
                    }
                    else
                    {
                        subTvi = findTreeItem(ic, sub);
                        if ((null == subTvi) && bExpand)
                        {
                            ic.UpdateLayout();
                            subTvi = findTreeItem(ic, sub);
                        }
                        if (null != subTvi)
                        {
                            setTreeExpansion(subTvi, bExpand);
                        }
                    }
                } // end - for
            } // end - if
        }  // function setTreeExpansion()

        public void setTreeFocus(TreeView xTree)
        {
            ItemsControl tvi = xTree;
            var itemOrder = pathList;
            int iLast = itemOrder.Count - 1;
            for (int i = 0; i <= iLast; i++)
            {
                TreeViewItem tviPart = findTreeItem(tvi, itemOrder[i]);
                if (null == tviPart) { break; }
                if (tviPart.HasItems)
                {
                    tviPart.IsExpanded = true;
                    tviPart.UpdateLayout();
                }
                if (iLast == i)
                {
                    tviPart.Focus();
                }
                tvi = tviPart;
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

    } // end - class TreePath
}
