using System.Collections.Generic;
using System.IO;
using System.Linq;
using PcapngUtils.Common;
using PcapngUtils.PcapNG;
using PcapngUtils.PcapNG.BlockTypes;
using PcapngUtils.PcapNG.CommonTypes;
using PcapngUtils.PcapNG.OptionTypes;

namespace PacketStudio.DataAccess
{
    /// <summary>
	/// Writes packets to temporary files
	/// </summary>
	public class TempPacketsSaver
    {
        /// <returns>Path of the temporary PCAP that was created</returns>
        public string WritePackets(IEnumerable<TempPacketSaveData> packets)
        {
            TimestampHelper tsh = new TimestampHelper(0, 0);

            string pcapngPath = Path.ChangeExtension(Path.GetTempFileName(), "pcapng");
            IEnumerable<LinkLayerType> allLinkLayers = packets.Select(packetData => packetData.LinkLayer).Distinct();

            // A local "Link layer to Interface ID" dictionary
            Dictionary<ushort, int> linkLayerToFakeInterfaceId = new Dictionary<ushort, int>();
            int nextInterfaceId = 0;
            // Collection of face interfaces we need to add
            List<InterfaceDescriptionBlock> ifaceDescBlock = new List<InterfaceDescriptionBlock>();
            foreach (LinkLayerType linkLayer in allLinkLayers)
            {
                InterfaceDescriptionBlock ifdb = new InterfaceDescriptionBlock((LinkTypes)linkLayer, ushort.MaxValue,
                    new InterfaceDescriptionOption(Comment: null, Name: "Fake interface " + nextInterfaceId));
                ifaceDescBlock.Add(ifdb);
                linkLayerToFakeInterfaceId.Add((ushort)linkLayer, nextInterfaceId);

                nextInterfaceId++;
            }

            // Place all interfaaces in a header
            HeaderWithInterfacesDescriptions hwid =
                new HeaderWithInterfacesDescriptions(SectionHeaderBlock.GetEmptyHeader(false), ifaceDescBlock);


            PcapngUtils.PcapNG.PcapNGWriter ngWriter = new PcapNGWriter(pcapngPath, new List<HeaderWithInterfacesDescriptions>() { hwid });
            foreach (TempPacketSaveData packet in packets)
            {
                int interfaceId = linkLayerToFakeInterfaceId[(ushort)packet.LinkLayer];
                byte[] packetData = packet.Data;
                EnchantedPacketBlock epb = new EnchantedPacketBlock(interfaceId, tsh, packetData.Length, packetData, new EnchantedPacketOption());
                ngWriter.WritePacket(epb);
            }
            ngWriter.Dispose();

            return pcapngPath;
        }

        /// <returns>Path of the temporary PCAP that was created</returns>
        public string WritePacket(TempPacketSaveData packet)
        {
            return WritePackets(new List<TempPacketSaveData>() { packet });
        }

    }
}
