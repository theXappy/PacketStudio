using System.Collections.Generic;

namespace PacketStudio.DataAccess.SaveData
{
    public class PacketSaveDataNG
    {
        public HexStreamType Type { get; set; }
        public string PacketData { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public Dictionary<string, string> Details { get; set; }

        public PacketSaveDataNG(HexStreamType type, string packetData)
        {
            Type = type;
            PacketData = packetData;
            Metadata = new Dictionary<string, string>();
            Details = new Dictionary<string, string>();
        }
    }

    public static class PackSaveDataNGMetaFields
    {
        public static string HEADER_FIELD = "header";
    }
}