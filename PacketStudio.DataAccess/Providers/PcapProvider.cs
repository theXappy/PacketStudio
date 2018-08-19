using System.Collections;
using System.Collections.Generic;
using PacketStudio.DataAccess.SaveData;
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
			    string linkType = ((byte) cfrd.LinkType).ToString();
                while ((nextPacket = cfrd.GetNextPacket()) != null)
				{
					byte[] arr = nextPacket.Data;
					yield return new PacketSaveDataV3(arr.ToHex(),HexStreamType.Raw,linkType,"1","1","");
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
