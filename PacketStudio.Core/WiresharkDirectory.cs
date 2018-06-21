namespace PacketStudio.Core
{
	public class WiresharkDirectory
	{
		public string WiresharkPath { get; set; }
		public string TsharkPath { get; set; }

		public WiresharkDirectory(string wiresharkPath, string tsharkPath)
		{
			WiresharkPath = wiresharkPath;
			TsharkPath = tsharkPath;
		}
	}
}