namespace PacketStudio.DataAccess.SaveData
{
	public abstract class PacketSaveData
	{
		public abstract string Text { get; }
		public abstract HexStreamType Type { get; }
        public abstract string LinkLayerType { get; }
        public abstract string StreamID { get; }
        public abstract string PayloadProtoId { get; }
        public abstract string MagicWord { get; }

        public static PacketSaveData Parse(string magicWord, string content)
		{
			switch (magicWord)
			{
#pragma warning disable 612 // Disable 'Obsolete' warning since this is here to handle backward compatibility
                case PacketSaveDataV1.MAGIC_WORD:
                    return PacketSaveDataV1.Parse(content);
				case PacketSaveDataV2.MAGIC_WORD:
					return PacketSaveDataV2.Parse(content);
#pragma warning restore 612
			    case PacketSaveDataV3.MAGIC_WORD:
			        return PacketSaveDataV3.Parse(content);
                default:
					return null;
			}
		}
	}
}