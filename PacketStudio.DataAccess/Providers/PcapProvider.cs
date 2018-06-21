using System.Collections;
using System.Collections.Generic;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PacketStudio.DataAccess.Providers
{
	public class PcapProvider : IPacketsProvider
	{

		private string _fileName;
		CaptureFileReaderDevice cfrd = null;

		public PcapProvider(string fileName)
		{
			_fileName = fileName;
		}

		public IEnumerator<PacketSaveData> GetEnumerator()
		{
			try
			{
				cfrd = new CaptureFileReaderDevice(_fileName);
				cfrd.Open();
				RawCapture nextPacket;
				while ((nextPacket = cfrd.GetNextPacket()) != null)
				{
					byte[] arr = nextPacket.Data;
					yield return new PacketSaveDataV2(arr.ToHex(),HexStreamType.RawEthernet,"1","1");
				}

			}
			finally
			{
				cfrd?.Close();
			}
		}


		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			cfrd?.Close();
		}
	}


}
