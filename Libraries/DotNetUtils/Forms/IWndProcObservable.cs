using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetUtils.Forms
{
    public interface IWndProcObservable
    {
        event WndProcEventHandler WndProcMessage;
    }

    public delegate void WndProcEventHandler(ref Message m);
}
