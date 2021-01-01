using System.Collections.Generic;
using System.Linq;

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

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, " +
                   $"{nameof(PacketData)}: {PacketData}, " +
                   $"{nameof(Metadata)}: {{{string.Join(",", Metadata.Select(kvp => $"{kvp.Key}={kvp.Value}").ToArray())}}}," +
                   $"{nameof(Details)}: {{{string.Join(",", Details.Select(kvp => $"{kvp.Key}={kvp.Value}").ToArray())}}},";
        }
    }

    public static class PackSaveDataNGMetaFields
    {
        public static string HEADER_FIELD = "header";
    }

    public static class PacketSaveDataNGProtoFields
    {
        public static string ENCAPS_TYPE = "EncapsType";
        public static string STREAM_ID = "StreamId";
    }
}