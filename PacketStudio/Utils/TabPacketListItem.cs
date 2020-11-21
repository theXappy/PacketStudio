using System.Windows.Forms;

namespace PacketStudio.Utils
{
	public class TabPacketListItem
	{
		private readonly string _prefix; // Usually the packet's number
		public TabPage Page { get; set; }

		public TabPacketListItem(string prefix, TabPage page)
		{
			_prefix = prefix;
			Page = page;
		}
		public override string ToString()
		{
			return $"{_prefix} [Tab Name: {Page.Text}]";
		}
	}
}