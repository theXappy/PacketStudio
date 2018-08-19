using System.ComponentModel;

namespace PacketStudio.DataAccess
{
    public enum HexStreamType
    {
        [Description("Raw")]
        Raw = 0,
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