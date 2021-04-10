using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using PacketDotNet;
using PacketDotNet.Sctp.Chunks;
using PacketDotNet.Utils;
using PacketStudio.DataAccess;

namespace PacketStudio.NewGUI.ProtocolFactories
{
    public class SctpPacketFactory
    {
        public TempPacketSaveData GetPacket(byte[] payload, int sctpStreamId, int ppid)
        {
            PacketDotNet.Sctp.Chunks.SctpPayloadProtocol proto = (SctpPayloadProtocol)ppid;

            return GetPacket(payload, sctpStreamId, proto);
        }

        public TempPacketSaveData GetPacket(byte[] payload, int sctpStreamId, SctpPayloadProtocol proto)
        {
            PhysicalAddress emptyAddress = PhysicalAddress.Parse("000000000000");
            PacketDotNet.EthernetPacket etherPacket = new EthernetPacket(emptyAddress, emptyAddress, EthernetType.IPv4);

            bool flip = sctpStreamId < 0;
            sctpStreamId = Math.Abs(sctpStreamId);
            Random r = new Random(sctpStreamId);

            IPAddress sourceIp = new IPAddress(r.Next());
            IPAddress destIp = new IPAddress(r.Next());
            if (flip)
            {
                IPAddress tempAddress = sourceIp;
                sourceIp = destIp;
                destIp = tempAddress;
            }

            var IPProtocolType_Sctp = 132;
            IPv4Packet ipPacket = new IPv4Packet(sourceIp, destIp) {Protocol = (ProtocolType)IPProtocolType_Sctp};
            SctpPacket sctpPacket = new SctpPacket(1, 1, 0, 0);
            SctpDataChunk dataChunk = new PacketDotNet.Sctp.Chunks.SctpDataChunk(new ByteArraySegment(
                new byte[]
                {
                    0x00, 0x03, 0x00, 0x14, 0x79, 0x46, 0x08, 0xb7, 0x00, 0x00, 0x00, 0x17, 0x00, 0x00, 0x00, 0x19, 0x00, 0x00,
                    0x00, 0x00
                }), sctpPacket)
            {
                PayloadData = payload,
                PayloadProtocol = proto
            };
            dataChunk.Length = (ushort) (16 + payload.Length);
            byte[] ipPayload = sctpPacket.Bytes.Concat(dataChunk.Bytes).ToArray();
            ipPacket.PayloadData = ipPayload;
            etherPacket.PayloadPacket = ipPacket;
            return new TempPacketSaveData(etherPacket.Bytes, LinkLayerType.Ethernet);
        }
    }
}