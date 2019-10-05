using System;
using Syncfusion.Windows.Forms.Tools;

namespace PacketStudio.Controls
{
    public class TabCancelEventArgs : EventArgs
    {
        public TabPageAdv Tab { get; set; }
        public bool Cancel { get; set; }

        public TabCancelEventArgs(TabPageAdv tab)
        {
            Tab = tab;
        }
    }
}