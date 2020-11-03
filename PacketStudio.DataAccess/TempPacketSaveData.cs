namespace PacketStudio.DataAccess
{
    public class TempPacketSaveData
    {
        public byte[] Data { get; private set; }
        public LinkLayerType LinkLayer { get; private set; }

        public TempPacketSaveData(byte[] data, LinkLayerType linkLayer)
        {
            Data = data;
            LinkLayer = linkLayer;
        }
    }
}