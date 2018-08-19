namespace PacketStudio.DataAccess
{
    public class TempPacketSaveData
    {
        public byte[] Data { get; private set; }
        public byte LinkLayer { get; private set; }

        public TempPacketSaveData(byte[] data, byte linkLayer)
        {
            Data = data;
            LinkLayer = linkLayer;
        }
    }
}