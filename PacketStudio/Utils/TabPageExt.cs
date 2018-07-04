using System.Windows.Forms;

namespace PacketStudio.Utils
{
    internal static class TabPageExt
    {
        public static bool IsPlusTab(this TabPage page) => page?.Name == "plusTab";
    }
}