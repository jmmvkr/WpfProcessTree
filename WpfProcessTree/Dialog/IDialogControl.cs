﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProcessTree.Dialog
{
    public interface IDialogControl
    {
        IDialog dlg { get; set; }
    }
}