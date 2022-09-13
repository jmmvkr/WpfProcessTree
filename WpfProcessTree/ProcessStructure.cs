using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VData;
using VProcessWindow;

namespace WpfProcessTree
{

    internal struct ProcessStructure
    {
        public int pid;
        public string name;
        public string fullPath;
        public string iconKey;
        public string title;

        public static ProcessStructure group(string nm)
        {
            ProcessStructure st = empty();
            st.name = nm;
            return st;
        }

        public static ProcessStructure empty()
        {
            ProcessStructure st;
            st.pid = 0;
            st.name = null;
            st.fullPath = null;
            st.iconKey = null;
            st.title = null;
            return st;
        }

        public string getName()
        {
            var title = this.title;
            if (null != title)
            {
                return title;
            }
            return name;
        }

        public string Name { get { return getName(); } }
        public string Tooltip => fullPath;
        public int Pid => pid;
        public System.Windows.Media.Imaging.BitmapImage Icon
        {
            get
            {
                var iconCache = Global.instance.processIcon;
                if (null != iconKey)
                {
                    var icon = iconCache.findIcon(iconKey);
                    if (null != icon)
                    {
                        return icon;
                    }
                }
                return null;
            }
        }
    }

    internal class NodePs : Node<ProcessStructure> { }

}
