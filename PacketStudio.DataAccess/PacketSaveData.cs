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
#pragma warning disable 612 // Disable 'Obsolete' warning since this is here to handle backward compatibility
                    return PacketSaveDataV1.Parse(content);
#pragma warning restore 612
				case "P0NTI4K":
					return PacketSaveDataV2.Parse(content);
				default:
					return null;
			}
		}
	}
}