using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProcessTree.Dialog
{
    public interface IDialog
    {
        IDialogControl dlg { get; set; }
    }
}
