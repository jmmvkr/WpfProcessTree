using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace VProcessWindow
{
    public class ProcessSearch
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessId, Name, ExecutablePath FROM Win32_Process");

        public List<Entry> search()
        {
            using (var results = searcher.Get())
            {
                var processes = results.Cast<ManagementObject>().Select(x => new
                {
                    ProcessId = (UInt32)x["ProcessId"],
                    Name = (string)x["Name"],
                    ExecutablePath = (string)x["ExecutablePath"]
                });

                List<Entry> list = new List<Entry>();
                foreach (var p in processes)
                {
                    Entry e;
                    e.ProcessId = Convert.ToInt32(p.ProcessId);
                    e.Name = p.Name;
                    e.ExecutablePath = p.ExecutablePath;
                    list.Add(e);
                }
                return list;
            }
        }

        public struct Entry
        {
            public int ProcessId;
            public string Name;
            public string ExecutablePath;
        }

    } // end - class ProcessSearch
}
