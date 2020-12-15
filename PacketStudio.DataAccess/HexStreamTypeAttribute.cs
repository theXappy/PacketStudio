using System;
using System.ComponentModel;

namespace PacketStudio.DataAccess
{
    public class HexStreamTypeAttribute : Attribute
    {
        public HexStreamType StreamType { get; set; }

        public HexStreamTypeAttribute(HexStreamType streamType)
        {
            StreamType = streamType;
        }
    }
}