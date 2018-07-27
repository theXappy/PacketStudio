using System.Collections;
using System.Collections.Generic;
using PcapngFile;

namespace PacketStudio.DataAccess.Providers
{


	public class PcapNGProvider : IPacketsProvider
	{

		private string _fileName;
		PcapngFile.Reader reader = null;

		public PcapNGProvider(string fileName)
		{
			_fileName = fileName;
		}

		public IEnumerator<PacketSaveData> GetEnumerator()
		{

			try
			{
				reader = new PcapngFile.Reader(_fileName);
				foreach (EnhancedPacketBlock packetBlock in reader.EnhancedPacketBlocks)
				{
					byte[] data = packetBlock.Data;
					yield return new PacketSaveDataV2(data.ToHex(),HexStreamType.RawEthernet,"1","1");
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
