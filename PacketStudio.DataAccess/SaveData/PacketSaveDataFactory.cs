namespace PacketStudio.DataAccess.SaveData
{
    public static class PacketSaveDataFactory
    {
        public static PacketSaveData GetPacketSaveData(string hex, HexStreamType streamType, LinkLayerType linkType, string streamId, string ppId, string ext = "")
        {
            string linkTypeStr = ((byte)linkType).ToString();
            return GetPacketSaveData(hex, streamType, linkTypeStr, streamId, ppId, ext);
        }
        public static PacketSaveData GetPacketSaveData(string hex, HexStreamType streamType, string linkType, string streamId, string ppId, string ext = "")
        {
            return new PacketSaveDataV3(hex, streamType, linkType, streamId, ppId, ext);
        }
    }
}
