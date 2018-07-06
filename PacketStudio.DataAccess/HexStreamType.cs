using System.ComponentModel;

namespace PacketStudio.DataAccess
{
    public enum HexStreamType
    {
        [Description("Raw Ethernet")]
        RawEthernet,
        [Description("IP Payload")]
        IpPayload,
        [Description("UDP Payload")]
        UdpPayload,
        [Description("TCP Payload")]
        TcpPayload,
        [Description("SCTP Payload")]
        SctpPayload,
    }
}