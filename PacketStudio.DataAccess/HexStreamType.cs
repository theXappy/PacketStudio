using System.ComponentModel;

namespace PacketStudio.DataAccess
{
    public enum HexStreamType
	{
        RawEthernet,
        IpPayload,
        UdpPayload,
        TcpPayload,
        SctpPayload,
    }
}