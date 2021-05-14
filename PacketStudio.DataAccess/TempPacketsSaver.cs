using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Haukcode.PcapngUtils.Common;
using Haukcode.PcapngUtils.PcapNG;
using Haukcode.PcapngUtils.PcapNG.BlockTypes;
using Haukcode.PcapngUtils.PcapNG.CommonTypes;
using Haukcode.PcapngUtils.PcapNG.OptionTypes;
using PacketStudio.DataAccess.SmartCapture;

namespace PacketStudio.DataAccess
{
    /// <summary>
	/// Writes packets to temporary files
	/// </summary>
	public class TempPacketsSaver
    {
        public string WritePackets(IEnumerable<TempPacketSaveData> packets) => WritePackets(null, packets);

        /// <summary>
        /// Saves packets to a new temp file
        /// </summary>
        /// <param name="basePcapngFile">Possible back file to copy the packets from. Can be null if no such file exists</param>
        /// <param name="packets">List of temporary packets to write. Might be empty ONLY IF <paramref name="basePcapngFile"/> is not NULL</param>
        /// <returns>Path of the temporary PCAP that was created</returns>
        public string WritePackets(PcapngWeakHandle basePcapngFile, IEnumerable<TempPacketSaveData> packets)
        {
            if (basePcapngFile != null)
            {
                return DoExportBasedOfFile(basePcapngFile, packets);
            }
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


            Haukcode.PcapngUtils.PcapNG.PcapNGWriter ngWriter = new PcapNGWriter(pcapngPath, new List<HeaderWithInterfacesDescriptions>() { hwid });
            foreach (TempPacketSaveData packet in packets)
            {
                int interfaceId = linkLayerToFakeInterfaceId[(ushort)packet.LinkLayer];
                byte[] packetData = packet.Data;
                EnhancedPacketBlock epb = new EnhancedPacketBlock(interfaceId, tsh, packetData.Length, packetData, new EnhancedPacketOption());
                ngWriter.WritePacket(epb);
            }
            ngWriter.Dispose();

            return pcapngPath;
        }

        private string DoExportBasedOfFile(PcapngWeakHandle basePcapngFile, IEnumerable<TempPacketSaveData> packets)
        {
            if (!packets.Any())
            {
                // No packets, just return the pack of the pcapng file
                return basePcapngFile.Path;
            }

            // Some packets defined, need to create modified version of the file
            string tempFile = Path.ChangeExtension(Path.GetTempFileName(), "pcapng");
            File.Copy(basePcapngFile.Path, tempFile);

            PcapngWeakHandle tempHandle = new PcapngWeakHandle(tempFile);
            var offsets = tempHandle.GetPacketsOffsets();
            var ifaces = tempHandle.GetInterfaces();
            foreach (TempPacketSaveData packet in packets)
            {

                long packetOffset = offsets[packet.PacketNumber];
                EnhancedPacketBlock replacedEpb = tempHandle.GetPacketAt(packetOffset);

                InterfaceDescriptionBlock interfaceOfReplacedPacket = ifaces[replacedEpb.InterfaceID];

                // TODO: Sooooo I'm having some issues here because I didn't carry the interface ID into the 'TempPacketSaveData' object
                // nor did I allow the user to set which interface he wants to associate the packet to.
                // I'm resorting to 2 heuristics:
                //  1. If the Link Layer of the exported packet looks like the one of the replaced packet, use the replaced packet's iface ID
                //  2. Otherwise, look for the first interface with the new link layer
                //  3. Otheriwse - NOT IMPLEMENTED, will crash. (TODO: That's where we need to inject new interfaces descirption blocks...)
                int ifacedId = -1;
                if (interfaceOfReplacedPacket.LinkType == (LinkTypes) packet.LinkLayer)
                {
                    ifacedId = replacedEpb.InterfaceID;
                }
                else
                {
                    for (int i = 0; i < ifaces.Count; i++)
                    {
                        if (ifaces[i].LinkType == (LinkTypes) packet.LinkLayer)
                        {
                            ifacedId = i;
                        }
                    }

                    if (ifacedId == -1)
                    {
                        throw new ArgumentException(
                            $"Couldn't find an interface in the PCAPNG for the requested link layer of the exported packet. Link Layer: {packet.LinkLayer}");
                    }
                }

                EnhancedPacketBlock epb = new EnhancedPacketBlock(ifacedId,
                    replacedEpb.Timestamp,
                    packet.Data.Length,
                    packet.Data,
                    new EnhancedPacketOption()
                    );

                tempHandle.ReplacePacket(packetOffset, epb);
            }

            return tempFile;
        }

        /// <returns>Path of the temporary PCAP that was created</returns>
        public string WritePacket(TempPacketSaveData packet)
        {
            return WritePackets(null, new List<TempPacketSaveData>() { packet });
        }

    }
}
