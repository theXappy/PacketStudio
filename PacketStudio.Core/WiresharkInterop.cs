﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CliWrap.Models;
using PacketStudio.DataAccess;

namespace PacketStudio.Core
{
	public class WiresharkInterop
	{
		private string _wiresharkPath;
		private TempPacketsSaver _packetsSaver;

	    public string Version { get; private set; }

		public string WiresharkPath
		{
			get => _wiresharkPath;
			set
			{
                FileInfo fi = new FileInfo(value);
				if (!fi.Exists)
				{
					throw new FileNotFoundException("Couldn't find Wireshark at the given location. Path: " + value);
				}
			    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(value);
			    Version = fvi.ProductVersion;
                
				_wiresharkPath = value;
			}
		}

		public WiresharkInterop(string wiresharkPath)
		{
			WiresharkPath = wiresharkPath; // This also checks if the file exists
			_packetsSaver = new TempPacketsSaver();
		}

		public Task<ExecutionOutput> ExportToWsAsync(IEnumerable<byte[]> packets)
		{
			string savedPcapPath = _packetsSaver.WritePackets(packets);
			// Running wireshark
			// Note that when wireshark exists it triggers a live preview update since
			// the user might have enabled/disabled some protocols/preferences so we need to update the current view!
			CliWrap.Cli wsCli = new CliWrap.Cli(WiresharkPath);
			return wsCli.ExecuteAsync(savedPcapPath);
		}
		public void ExportToWs(IEnumerable<byte[]> packets)
		{
			try
			{
				ExportToWsAsync(packets).Wait();
			}
			catch
			{
				// ignored
			}
		}
	}
}