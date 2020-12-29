using System.Collections;
using System.Collections.Generic;
using PacketStudio.DataAccess.SaveData;
using PcapngFile;

namespace PacketStudio.DataAccess.Providers
{


	public class PcapNGProviderNG : IPacketsProviderNG
	{

		private string _fileName;
		PcapngFile.Reader reader = null;

		public PcapNGProviderNG(string fileName)
		{
			_fileName = fileName;
		}

		public IEnumerator<PacketSaveDataNG> GetEnumerator()
		{

			try
			{
				reader = new PcapngFile.Reader(_fileName);
			    int interfaceId = 0;
			    Dictionary<int, LinkType> interfaceIdToLinkLayer = new Dictionary<int, LinkType>();
                foreach (InterfaceDescriptionBlock readerInterfaceDescriptionBlock in reader.InterfaceDescriptionBlocks)
                {
                    interfaceIdToLinkLayer.Add(interfaceId,readerInterfaceDescriptionBlock.LinkType);
                }
				foreach (EnhancedPacketBlock packetBlock in reader.EnhancedPacketBlocks)
				{
				    LinkType linkType = interfaceIdToLinkLayer[packetBlock.InterfaceID];
				    string linkTypeStr = ((byte) linkType).ToString();
					byte[] data = packetBlock.Data;
                    PacketSaveDataNG psdng = new PacketSaveDataNG(HexStreamType.Raw, data.ToHex());
                    psdng.Details[PacketSaveDataNGProtoFields.ENCAPS_TYPE] = linkTypeStr;
                    yield return psdng;
				}
			}
			finally
			{
				reader?.Dispose();
			}
		}


		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			reader?.Dispose();
		}
	}


}
