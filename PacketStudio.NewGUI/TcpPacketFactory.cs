using System;
using System.Net;
using System.Net.NetworkInformation;
using PacketDotNet;
using PacketStudio.DataAccess;

namespace PacketStudio.NewGUI
{
    public class TcpPacketFactory
    {
        public TempPacketSaveData GetPacket(byte[] payload, int streamId)
        {
            PhysicalAddress emptyAddress = PhysicalAddress.Parse("000000000000");
            PacketDotNet.EthernetPacket etherPacket = new EthernetPacket(emptyAddress, emptyAddress, EthernetPacketType.IPv4);

            bool flip = streamId < 0;
            streamId = Math.Abs(streamId);
            Random r = new Random(streamId);

            IPAddress sourceIp = new IPAddress(r.Next());
            IPAddress destIp = new IPAddress(r.Next());
            if (flip)
            {
                IPAddress tempAddress = sourceIp;
                sourceIp = destIp;
                destIp = tempAddress;
            }
            IPv4Packet ipPacket = new IPv4Packet(sourceIp, destIp) { NextHeader = IPProtocolType.UDP };
            TcpPacket tcpPacket = new TcpPacket(1,1) { PayloadData = payload };
            ipPacket.PayloadPacket = tcpPacket;
            etherPacket.PayloadPacket = ipPacket;
            return new TempPacketSaveData(etherPacket.Bytes, LinkLayerType.Ethernet);
        }
    }
}