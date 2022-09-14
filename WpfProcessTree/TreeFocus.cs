using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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

                // focus on a group when pid is 0
                if (0 == pid)
                {
                    string nm = selNode.val.name;

                    TreePath tPath = new TreePath();
                    foreach (var grp in psList)
                    {
                        if (nm.Equals(grp.__value.name))
                        {
                            tPath.add(grp);
                            break;
                        }
                    }
                    tPath.setTreeFocus(xTree);
                    return;
                }

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

        public static TreeViewItem findTreeUi(TreeView xTree, object v)
        {
            ItemsControl ic = xTree;
            return findTreeUi(ic, v);
        }

        static TreeViewItem findTreeUi(ItemsControl ic, object v)
        {
            var ig = ic.ItemContainerGenerator;

            // find direct child
            foreach (var it in ic.Items)
            {
                if (v == it)
                {
                    return ig.ContainerFromItem(v) as TreeViewItem;
                }
            }
            // find recursively
            foreach (var it in ic.Items)
            {
                var subIc = ig.ContainerFromItem(it) as ItemsControl;
                if (null != subIc && subIc.HasItems)
                {
                    var recFound = findTreeUi(subIc, v);
                    if (null != recFound)
                    {
                        return recFound;
                    }
                }
            }
            return null;
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
                if (iLast == i)
                {
                    FocusManager.SetFocusedElement(xTree, tviPart);
                    break;
                }
                if (tviPart.HasItems)
                {
                    tviPart.IsExpanded = true;
                    tviPart.UpdateLayout();
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
