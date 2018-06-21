namespace PacketStudio.DataAccess
{
	public abstract class PacketSaveData
	{
		public abstract string Text { get; }
		public abstract HexStreamType Type { get; }
		public abstract string StreamID { get; }

		public static PacketSaveData Parse(string magicWord, string content)
		{
			switch (magicWord)
			{
				case "M4G1C":
					return PacketSaveDataV1.Parse(content);
				case "P0NTI4K":
					return PacketSaveDataV2.Parse(content);
				default:
					return null;
			}
		}
	}
}