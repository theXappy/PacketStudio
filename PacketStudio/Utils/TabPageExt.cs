﻿using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;

namespace PacketStudio.Utils
{
    internal static class TabPageExt
    {
        public static bool IsPlusTab(this TabPageAdv page) => page?.Name == "plusTab";
    }
}