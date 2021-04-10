using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Haukcode.PcapngUtils.Common;
using Haukcode.PcapngUtils.Pcap;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Providers
{
    public class PcapProviderNG : IPacketsProviderNG
    {

        private string _fileName;
        private Haukcode.PcapngUtils.Pcap.PcapReader pReader;

        public PcapProviderNG(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerator<PacketSaveDataNG> GetEnumerator()
        {

            try
            {
                List<PacketSaveDataNG> blockCol = new List<PacketSaveDataNG>();
                pReader = new PcapReader(_fileName);
                string linkLayer = ((int) pReader.Header.LinkType).ToString();
                pReader.OnReadPacketEvent += delegate(object context, IPacket packet)
                {
                    PacketSaveDataNG psdng = new PacketSaveDataNG(HexStreamType.Raw,packet.Data.ToHex());
                    psdng.Details[PacketSaveDataNGProtoFields.ENCAPS_TYPE] = linkLayer;
                    blockCol.Add(psdng);
                };
                pReader.ReadPackets(CancellationToken.None);

                foreach (var packet in blockCol)
                {
                    yield return packet;
                }
            }
            finally
            {
                pReader?.Dispose();
            }
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            pReader?.Dispose();
        }
    }
}