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
        IconCache icons = new IconCache();

        internal ProcessModel()
        {
            Global.init();
            Global.instance.processIcon = icons;
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

            // add to node tree
            var keys = bucket.keys.ToArray();
            Array.Sort(keys);
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
                foreach (var ps in lst)
                {
                    var node = NodePs.v(ps);
                    var fullPath = node.__value.fullPath;
                    try
                    {
                        if (icons.loadIcon(fullPath))
                        {
                            node.__value.iconKey = fullPath;
                        }
                        if (bFirst)
                        {
                            groupNode.__value.iconKey = node.__value.iconKey;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                    groupNode.add(node);
                }
            }

            return psRoot.subNodes;
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
                }
            }
            return result;
        }

    } // end - class ProcessModel
}
