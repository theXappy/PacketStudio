using System.Windows.Forms;

namespace PacketStudio
{
	public class TabPacketListItem
	{
		private string _prefix;
		public TabPage Page { get; set; }

		public TabPacketListItem(string prefix, TabPage page)
		{
			_prefix = prefix;
			Page = page;
		}
		public override string ToString()
		{
			return $"{_prefix}. {Page.Text}";
		}
	}
}