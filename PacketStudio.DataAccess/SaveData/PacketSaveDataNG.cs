using System;
using System.Collections.Generic;
using System.Linq;

namespace PacketStudio.DataAccess.SaveData
{
    public class PacketSaveDataNG : ICloneable
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

        public object Clone()
        {
            return new PacketSaveDataNG(this.Type, this.PacketData)
            {
                Metadata = new Dictionary<string, string>(this.Metadata),
                Details = new Dictionary<string, string>(this.Details),
            };
        }

        protected bool Equals(PacketSaveDataNG other)
        {
            return Type == other.Type &&
                   PacketData == other.PacketData && 
                   Metadata.SequenceEqual(other.Metadata) && 
                   Details.SequenceEqual(other.Details);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PacketSaveDataNG) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Type, PacketData, Metadata, Details);
        }
    }

    public static class PackSaveDataNGMetaFields
    {
        public static string HEADER_FIELD = "header";
        public static string CARET_POS_FIELD = "caret_pos";
    }

    public static class PacketSaveDataNGProtoFields
    {
        public static string ENCAPS_TYPE = "EncapsType";
        public static string STREAM_ID = "StreamId";
        public static string PPID_ID = "PayloadProtoId";
    }
}