namespace PacketStudio.DataAccess
{
    public class TempPacketSaveData
    {
        public int PacketNumber { get; set; }
        public byte[] Data { get; private set; }
        public LinkLayerType LinkLayer { get; private set; }

        public TempPacketSaveData(byte[] data, LinkLayerType linkLayer)
        {
            Data = data;
            LinkLayer = linkLayer;
        }

        public override string ToString()
        {
            return $"{nameof(PacketNumber)}:{PacketNumber}, {nameof(LinkLayer)}: {LinkLayer}, {nameof(Data)}: {Data.ToHex()}";
        }
    }
}