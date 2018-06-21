using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PacketStudio.Controls
{
	public class TreeViewWithArrows : TreeView
	{

		[DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
		private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

		public TreeViewWithArrows()
		{
			SetWindowTheme(this.Handle, "explorer", null);
		}
	}
}