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

        public override string ToString()
        {
            string ToHex(byte[] bs)
            {
                string outt = "";
                foreach (byte b in bs)
                    outt += b.ToString("X2");
                return outt;
            }
            return $"{nameof(LinkLayer)}: {LinkLayer}, {nameof(Data)}: {ToHex(Data)}";
        }
    }
}