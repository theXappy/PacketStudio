using System;
using Syncfusion.Windows.Forms.Tools;

namespace PacketStudio.Controls
{
    public class TabEventArgs : EventArgs
    {
        public TabPageAdv Tab { get; set; }

        public TabEventArgs(TabPageAdv tab)
        {
            this.Tab = tab;
        }
    }
}