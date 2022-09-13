using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VData;

namespace VProcessWindow
{
    public class ProcessTree<TargT>
    {
        public Action<Node<TargT>, ProcessSearch.Entry> onItem;
        public Node<TargT> root;

        public Node<TargT> construct(IList<ProcessSearch.Entry> list)
        {
            var rt = root;
            var subNodes = rt.subNodes;
            if (subNodes.Count > 0)
            {
                subNodes.Clear();
            }

            foreach (var e in list)
            {
                onItem(rt, e);
            }

            return rt;
        }

    } // end - class ProcessTree
}
