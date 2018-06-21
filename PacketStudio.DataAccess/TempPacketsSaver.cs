using System.Collections.Generic;
using System.IO;
using SharpPcap.LibPcap;

namespace PacketStudio.DataAccess
{
	/// <summary>
	/// Writes packets to temporary files
	/// </summary>
	public class TempPacketsSaver
	{
		/// <returns>Path of the temporary PCAP that was created</returns>
		public string WritePackets(IEnumerable<byte[]> packets)
		{
			string pcapPath = Path.ChangeExtension(Path.GetTempFileName(), "pcap");
			CaptureFileWriterDevice cfwd = new CaptureFileWriterDevice(pcapPath);
			cfwd.Open();
			foreach (byte[] packet in packets)
			{
				cfwd.Write(packet);
			}
			cfwd.Close();
			return pcapPath;
		}

		/// <returns>Path of the temporary PCAP that was created</returns>
		public string WritePacket(byte[] packet)
		{
			return WritePackets(new List<byte[]>() { packet });
		}

	}
}
