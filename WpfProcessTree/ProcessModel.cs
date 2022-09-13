using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VData;
using VProcessWindow;

namespace WpfProcessTree
{

    internal class ProcessModel
    {
        ProcessSearch psSearcher = new ProcessSearch();
        ProcessTree<ProcessStructure> psTree = new ProcessTree<ProcessStructure>();
        IconCache icons;

        internal ProcessModel()
        {
            Global.init();
            icons = Global.instance.processIcon;
            psTree.root = NodePs.v(ProcessStructure.group("psRoot"));
        }

        internal IList<Node<ProcessStructure>> updateTree()
        {
            var psList = psSearcher.search();

            // organize as buckets
            var bucket = new BucketMap<string, ProcessStructure>();
            var vEmpty = ProcessStructure.empty();
            psTree.onItem = (rt, e) => {
                ProcessStructure st = vEmpty;
                st.pid = e.ProcessId;
                st.name = e.Name;
                st.fullPath = e.ExecutablePath;
                bucket.add(e.Name, st);
            };
            var psRoot = psTree.construct(psList);

            // scan window title
            Dictionary<int, string> mapPidTitle = new Dictionary<int, string>();
            var lstWindowInfo = ProcessWindow.scanAllWindows();
            foreach (var wi in lstWindowInfo)
            {
                int id = wi.pid;
                if (!mapPidTitle.ContainsKey(id))
                {
                    string title = wi.title;
                    switch (title)
                    {
                        case "Default IME": continue;
                        case "MSCTFIME UI": continue;
                    }
                    mapPidTitle[wi.pid] = title;
                }
            }

            // add to node tree
            var keys = bucket.keys.ToArray();
            Array.Sort(keys);
            psRoot.ensureCapacity(keys.Count());
            foreach (var k in keys)
            {
                // prepare a group
                var grp = k;
                if (grp.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase))
                {
                    grp = k.Substring(0, k.Length - 4);
                }
                var groupNode = psRoot.addAsChild(ProcessStructure.group(grp));

                // add processes to group
                bool bFirst = true;
                var lst = bucket[k];
                groupNode.ensureCapacity(lst.Count());
                foreach (var ps in lst)
                {
                    // add child node
                    var node = NodePs.v(ps);
                    if (!updateChildNode(node, ref node.__value, mapPidTitle))
                    {
                        continue;
                    }
                    groupNode.add(node);

                    // use icon from first child node
                    if (bFirst)
                    {
                        bFirst = false;
                        groupNode.__value.iconKey = node.__value.iconKey;
                    }
                }
            }

            return psRoot.subNodes;
        }

        bool updateChildNode(Node<ProcessStructure> node, ref ProcessStructure rValue, Dictionary<int, string> mapPidTitle)
        {
            var fullPath = rValue.fullPath;
            try
            {
                var pid = rValue.pid;
                string title;
                if (mapPidTitle.TryGetValue(pid, out title))
                {
                    rValue.title = title;
                }
                if (icons.loadIcon(fullPath))
                {
                    rValue.iconKey = fullPath;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        internal IEnumerable filter(IList<Node<ProcessStructure>> psList, string strFilter)
        {
            if (String.IsNullOrEmpty(strFilter))
            {
                return psList;
            }
            var result = new System.Collections.ObjectModel.ObservableCollection<Node<ProcessStructure>>();
            foreach (var node in psList)
            {
                var nm = node.__value.name;
                if (nm.IndexOf(strFilter, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    result.Add(node);
                    continue;
                }

                bool bChildMatch = false;
                foreach (var sub in node.subNodes)
                {
                    var title = sub.__value.title;
                    if (null != title && title.IndexOf(strFilter, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        bChildMatch = true;
                        break;
                    }
                }
                if (bChildMatch)
                {
                    result.Add(node);
                    continue;
                }
            }
            return result;
        }

    } // end - class ProcessModel
}
