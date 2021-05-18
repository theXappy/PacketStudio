using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using PacketStudio.DataAccess;

namespace PacketStudio.Core
{
	public class WiresharkInterop
	{
        public class WiresharkInteropTask
        {
			/// <summary>
			/// Whether wireshark saved new preferences when running. Value only guaranteed to be correct after <see cref="CliTask"/> finished.
			/// </summary>
           public bool PreferencesChanged { get; set; }
           public CommandTask<CommandResult> CliTask { get; set; }
        }

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

		public WiresharkInteropTask ExportToWsAsync(IEnumerable<TempPacketSaveData> packets)
        {
            string savedPcapPath = _packetsSaver.WritePackets(null, packets);

            return RunWireshark(savedPcapPath);
        }

        /// <param name="inputPath">Either a file path or a pipe identifier</param>
        public WiresharkInteropTask RunWireshark(string inputPath)
        {
            // Output. Empty for now, will be updated below
            WiresharkInteropTask outputTask = new WiresharkInteropTask();

            // Setting up preferences monitor
            var wsPrefDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "wireshark");
            FileSystemWatcher fsw = new FileSystemWatcher(wsPrefDir);
            fsw.IncludeSubdirectories = true;

            void FswChanged(object o, FileSystemEventArgs args)
            {
                // In change in the preferences directory is treated the same
                var filteredFile = new List<string> {"recent", "recent_common"};
                if (!filteredFile.Contains(args.Name))
                {
                    outputTask.PreferencesChanged = true;
                }
            }

            fsw.Changed += FswChanged;
            fsw.EnableRaisingEvents = true;

            // Setting command line arguments.
            // By default assuming reading from file so the single argument is the path
            string arguments = inputPath;
            if(inputPath.StartsWith(@"\\.\pipe\"))
            {
                arguments = $"-i {inputPath} -k";
            }

            Debug.WriteLine(" &&& Running Wireshark with these arguments: " + arguments);
            // Running wireshark
            var wsCli = Cli.Wrap(_wiresharkPath)
                .WithArguments(arguments);
            CommandTask<CommandResult> cliTask = wsCli.ExecuteAsync();
            cliTask.Task.ContinueWith(_ =>
            {
                fsw.EnableRaisingEvents = false;
                fsw.Changed -= FswChanged;
                fsw.Dispose();
            });
            outputTask.CliTask = cliTask;

            return outputTask;
        }

        public void ExportToWs(IEnumerable<TempPacketSaveData> packets)
		{
			try
			{
				ExportToWsAsync(packets).CliTask.Task.Wait();
			}
			catch
			{
				// ignored
			}
		}

        public WiresharkInteropTask ExportWithPipe(string pipeName)
        {
            return RunWireshark($@"\\.\pipe\{pipeName}");
        }
    }
}