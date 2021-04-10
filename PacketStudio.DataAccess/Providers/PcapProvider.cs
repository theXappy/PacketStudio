using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Haukcode.PcapngUtils.Common;
using Haukcode.PcapngUtils.Pcap;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Providers
{
    public class PcapProvider : IPacketsProvider
    {

        private string _fileName;
        private Haukcode.PcapngUtils.Pcap.PcapReader pReader;

        public PcapProvider(string fileName)
        {
            _fileName = fileName;
        }

        public IEnumerator<PacketSaveData> GetEnumerator()
        {

            try
            {
                List<PacketSaveData> blockCol = new List<PacketSaveData>();
                 pReader = new PcapReader(_fileName);
                string linkLayer = ((int) pReader.Header.LinkType).ToString();
                pReader.OnReadPacketEvent += delegate(object context, IPacket packet)
                {
                    PacketSaveData psd = new PacketSaveDataV3(packet.Data.ToHex(),HexStreamType.Raw,linkLayer, "1", "1", "");
                    blockCol.Add(psd);
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