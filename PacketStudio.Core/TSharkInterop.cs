using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CliWrap;
using CliWrap.Buffered;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PacketStudio.DataAccess;

namespace PacketStudio.Core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TSharkInterop
    {

        public class TSharkInteropTask
        {
            public ObservableCollection<string> StdOutLines { get; set; }
            public CommandTask<CommandResult> CliTask { get; set; }
        }

        private string _tsharkPath;
        private readonly TempPacketsSaver _packetsSaver;
        private string _version;

        public string Version
        {
            get => _version;
            private set
            {
                _version = value;

                VersionMajor = 0;
                VersionMinor = 0;
                string[] splitted = Version?.Split('.');
                if (splitted != null && splitted.Length >= 2)
                {
                    int.TryParse(splitted[0], out var major);
                    int.TryParse(splitted[1], out var minor);
                    VersionMajor = major;
                    VersionMinor = minor;
                }
            }
        }

        private int VersionMajor { get; set; }
        private int VersionMinor { get; set; }

        public string TsharkPath
        {
            get => _tsharkPath;
            set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException("Couldn't find TShark at the given location. Path: " + value);
                }
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(value);
                Version = fvi.ProductVersion;


                _tsharkPath = value;
            }
        }

        public TSharkInterop(string tsharkPath)
        {
            TsharkPath = tsharkPath; // This also checks if the file exists
            _packetsSaver = new TempPacketsSaver();
        }

        public async Task<XElement> GetPdmlAsync(TempPacketSaveData packetSaveData)
        {
            return await GetPdmlAsync(packetSaveData, CancellationToken.None);
        }

        public async Task<XElement> GetPdmlAsync(IEnumerable<TempPacketSaveData> packets, int packetIndex)
        {
            return await GetPdmlAsync(packets, packetIndex, CancellationToken.None);
        }


        public async Task<XElement> GetPdmlAsync(TempPacketSaveData packetSaveData, CancellationToken token)
        {
            return await GetPdmlAsync(new List<TempPacketSaveData>() { packetSaveData }, 0, token);
        }

        public Task<XElement> GetPdmlAsync2(IEnumerable<TempPacketSaveData> packets, int packetIndex,
            CancellationToken token, List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            StringBuilder sb = new StringBuilder();

            // line below is a local func
            void dataReceivedEventHandler(object sender, DataReceivedEventArgs eventArgs) => sb.Append(eventArgs.Data);

            string pcapPath = _packetsSaver.WritePackets(packets);
            string args = GetPdmlArgs(pcapPath, toBeEnabledHeurs, toBeDisabledHeurs);
            Stopwatch spw = new Stopwatch();
            spw.Start();
            Process pProcess = new Process();
            pProcess.StartInfo.FileName = _tsharkPath;
            pProcess.StartInfo.Arguments = args;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.EnableRaisingEvents = true;
            pProcess.OutputDataReceived += dataReceivedEventHandler;
            pProcess.Start();
            pProcess.BeginOutputReadLine();
            pProcess.WaitForExit();
            pProcess.OutputDataReceived -= dataReceivedEventHandler;
            spw.Stop();
            Debug.WriteLine("Completed in : " +
                              spw.ElapsedMilliseconds.ToString()
                              + "ms");
            Debug.WriteLine("Process obj elapsed time : " + pProcess.TotalProcessorTime);
            pProcess.Close();

            string xml = sb.ToString();

            return ParsePdmlAsync(xml, packetIndex, token);
        }

        public Task<XElement> GetPdmlAsync(IEnumerable<TempPacketSaveData> packets, int packetIndex, CancellationToken token, List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            return Task.Run((() =>
            {
                string pcapPath = _packetsSaver.WritePackets(packets);
                token.ThrowIfCancellationRequested();

                string args = GetPdmlArgs(pcapPath, toBeEnabledHeurs, toBeDisabledHeurs);
                Debug.WriteLine(" @@@ [TShark] GetPdml Args: " + args);
                Debug.WriteLine(" @@@ [TShark] GetPdml First Packet: " + packets.First());
                ProcessStartInfo psi = new ProcessStartInfo(_tsharkPath, args)
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Minimized
                };
                // TODO CliWrap this
                Process p = Process.Start(psi);

                StreamReader errorStream = p.StandardError;
                StreamReader standardStream = p.StandardOutput;
                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();
                while (!p.WaitForExit(1000))
                {
                    output.Append(standardStream.ReadToEnd());
                    error.Append(errorStream.ReadToEnd());
                }
                output.Append(standardStream.ReadToEnd());
                error.Append(errorStream.ReadToEnd());

                if (p.ExitCode != 0)
                {
                    if (output.Length > 0)
                    {
                        // Exit code isn't 0 but we have output so dismissing for now...
                    }
                    else
                    {
                        // No output, show exit code and error
                        throw new Exception($"TShark returned with exit code: {p.ExitCode}\r\n{error}");
                    }

                }

                string xml = output.ToString();

                XElement element = ParsePdmlAsync(xml, packetIndex, token).Result;
                xml = null; //Hoping GC will get the que
                token.ThrowIfCancellationRequested();

                return element;
            }), token);
        }

        public Task GetTextOutputAsync(IEnumerable<TempPacketSaveData> packets, CancellationToken token, Action<string> handleStdoutLine, List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            // 2 Tasks:
            //      1. Same packets to file and get temporary pcap(ng) file path
            //      2. Feed GetTextOutputAsync overload which recieves a file path with all the received arguments
            var pcapPathTask = Task.Run(() => _packetsSaver.WritePackets(packets), token);
            return GetTextOutputAsync(pcapPathTask.Result, token, handleStdoutLine, toBeEnabledHeurs, toBeDisabledHeurs);
        }

        /// <param name="inputPath">Either a file path or a pipe identifier</param>
        public Task GetTextOutputAsync(string inputPath, CancellationToken token, Action<string> handleStdoutLine,
             List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            token.ThrowIfCancellationRequested();

            string args = GetTextOutputArgs(inputPath, toBeEnabledHeurs, toBeDisabledHeurs);
            Debug.WriteLine("GetText Args: " + args);

            var cli = Cli.Wrap(_tsharkPath)
                .WithArguments(args)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(handleStdoutLine, Encoding.UTF8))
                .ExecuteAsync(token);

            token.ThrowIfCancellationRequested();

            Debug.WriteLine(" @@@ Tshark started!");

            return cli;
        }

        private string GetPdmlArgs(string pcapPath, List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            string oldVersionArgs = $"-r {pcapPath} -T pdml";
            string newVersionArgs = $"-r {pcapPath} -2 -T pdml";

            string selected = oldVersionArgs;
            if (IsNewVersion())
            {
                selected = newVersionArgs;
            }

            if (toBeEnabledHeurs != null)
            {
                foreach (string enabledHeur in toBeEnabledHeurs)
                {
                    selected += " --enable-heuristic " + enabledHeur;
                }
            }
            if (toBeDisabledHeurs != null)
            {
                foreach (string disabledHeur in toBeDisabledHeurs)
                {
                    selected += " --disable-heuristic " + disabledHeur;
                }
            }
            return selected;
        }

        private bool IsNewVersion()
        {
            if (VersionMajor == 2)
            {
                if (VersionMinor > 4)
                {
                    return true;
                }
            }

            if (VersionMajor == 3)
                return true;
            return false;
        }

        private string GetTextOutputArgs(string inputPath, List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            string oldVersionArgs = $"-r {inputPath} -T tabs";
            string newVersionArgs = $"-r {inputPath} -T tabs -2";

            string selected = oldVersionArgs;
            if (IsNewVersion())
            {
                selected = newVersionArgs;
            }

            if (inputPath.Contains(@"\\.\pipe\"))
            {
                selected = Regex.Replace(selected, "^-r ", "-i ");
                // TODO: "Live capturing does not support two pass analysis..."
                selected = Regex.Replace(selected, "-2$", string.Empty);
            }


            if (toBeEnabledHeurs != null)
            {
                foreach (string enabledHeur in toBeEnabledHeurs)
                {
                    selected += " --enable-heuristic " + enabledHeur;
                }
            }
            if (toBeDisabledHeurs != null)
            {
                foreach (string disabledHeur in toBeDisabledHeurs)
                {
                    selected += " --disable-heuristic " + disabledHeur;
                }
            }

            // Force Wireshark's default columns in output format 
            selected +=
                " -o \"gui.column.format:" +
                "\"No\",\"%m\","
                +
                "\"Time\",\"%t\"," +
                "\"Source\",\"%s\"," +
                "\"Destination\",\"%d\"," +
                "\"Protocol\",\"%p\"," +
                "\"Length\",\"%L\"," +
                "\"Info\",\"%i\"" +
                "\""
                ;

            return selected;
        }


        public async Task<JObject> GetJsonRawAsync(TempPacketSaveData packetBytes)
        {
            return await GetJsonRawAsync(packetBytes, CancellationToken.None);
        }

        public async Task<JObject> GetJsonRawAsync(IEnumerable<TempPacketSaveData> packets, int packetIndex)
        {
            return await GetJsonRawAsync(packets, packetIndex, CancellationToken.None);
        }


        public async Task<JObject> GetJsonRawAsync(TempPacketSaveData packetBytes, CancellationToken token)
        {
            return await GetJsonRawAsync(new List<TempPacketSaveData>() { packetBytes }, 0, token);
        }

        public Task<JObject> GetJsonRawAsync(IEnumerable<TempPacketSaveData> packets, int packetIndex, CancellationToken token, List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            if (!IsNewVersion())
            {
                return Task.FromResult((JObject)null);
            }

            return Task.Run((() =>
            {
                string pcapPath = _packetsSaver.WritePackets(packets);
                token.ThrowIfCancellationRequested();

                string args = GetJsonArgs(pcapPath, toBeEnabledHeurs, toBeDisabledHeurs);
                var cli = Cli.Wrap(_tsharkPath)
                    .WithArguments(args)
                    .ExecuteBufferedAsync(token);

                bool timedOut = !cli.Task.Wait(15_000);

                if (timedOut)
                    throw new TaskCanceledException();

                if (cli.Task.IsCanceled) // task was canceled
                    throw new TaskCanceledException();


                var res = cli.Task.Result;
                if (res.ExitCode != 0) // TShark returned an error exit code
                {
                    // If we are cancelled, we don't actually care about the exit code
                    token.ThrowIfCancellationRequested();

                    // Show the exit code + errors
                    throw new Exception($"TShark returned with exit code: {res.ExitCode}\r\n{res.StandardError}");
                }

                string jsonArray = res.StandardOutput.ToString();

                JObject jsonPacket = ParseJsonAsync(jsonArray, packetIndex, token).Result;

                jsonArray = null; //Hoping GC will get the que
                token.ThrowIfCancellationRequested();

                return jsonPacket;
            }));
        }

        private string GetJsonArgs(string pcapPath, List<string> toBeEnabledHeurs, List<string> toBeDisabledHeurs)
        {
            string oldVersionArgs = $"-r {pcapPath} -T jsonraw";
            string newVersionArgs = $"-r {pcapPath} -T jsonraw -2  --no-duplicate-keys -o nameres.use_external_name_resolver:FALSE";

            string selected = oldVersionArgs;
            if (IsNewVersion())
            {
                selected = newVersionArgs;
            }

            if (toBeEnabledHeurs != null)
            {
                foreach (string enabledHeur in toBeEnabledHeurs)
                {
                    selected += " --enable-heuristic " + enabledHeur;
                }
            }
            if (toBeDisabledHeurs != null)
            {
                foreach (string disabledHeur in toBeDisabledHeurs)
                {
                    selected += " --disable-heuristic " + disabledHeur;
                }
            }
            return selected;
        }

        private Task<XElement> ParsePdmlAsync(string xml, int packetIndex, CancellationToken token)
        {
            return Task.Run((() =>
            {
                token.ThrowIfCancellationRequested();

                // This is most likely the heavy operation in this method. So checking the token right before and after
                XDocument xdoc = XDocument.Parse(xml);

                token.ThrowIfCancellationRequested();

                // Get the right packet according to the given index (parameter)

                // New PDML Style
                XContainer skippedHeaderNode = (XContainer)xdoc.FirstNode?.NextNode?.NextNode;
                if (skippedHeaderNode == null)
                {
                    // Old PDML Style
                    skippedHeaderNode = ((XContainer)xdoc.FirstNode);
                }

                XElement packetElement =
                    skippedHeaderNode.Elements("packet").ElementAt(packetIndex); // Getting the x-th 'packet' node

                string singlePacketXml = packetElement.ToString();
                XElement elem = XElement.Parse(singlePacketXml);

                // Hoping GC will get the cue
                singlePacketXml = null;
                packetElement = null;
                xdoc = null;


                return elem;
            }), token);
        }
        private Task<JObject> ParseJsonAsync(string json, int packetIndex, CancellationToken token)
        {
            return Task.Run((() =>
            {
                token.ThrowIfCancellationRequested();

                // This is most likely the heavy operation in this method. So checking the token right before and after
                JsonSerializer serializer = JsonSerializer.CreateDefault();
                JsonReader reader = new JsonTextReader(new StringReader(json));
                object deserialized = serializer.Deserialize(reader);
                JArray asArray = deserialized as JArray;


                token.ThrowIfCancellationRequested();

                // Get the right packet according to the given index (parameter)
                JObject packet = asArray.ElementAt(packetIndex) as JObject;

                // Hoping GC will get the cue
                reader.Close();
                reader = null;
                asArray = null;


                return packet;
            }), token);
        }

        public Task<TSharkCombinedResults> GetPdmlAndJsonAsync(IEnumerable<TempPacketSaveData> packets, int packetIndex,
            CancellationToken token, List<string> toBeEnabledHeurs = null, List<string> toBeDisabledHeurs = null)
        {
            Task<XElement> pdmlTask = GetPdmlAsync2(packets, packetIndex, token, toBeEnabledHeurs, toBeDisabledHeurs);
            Task<JObject> jsonTask = GetJsonRawAsync(packets, packetIndex, token, toBeEnabledHeurs, toBeDisabledHeurs);

            return Task.WhenAll(new Task[] { pdmlTask, jsonTask }).ContinueWith((task) =>
              {
                  XElement pdml = null;
                  Exception pdmlException = null;
                  JObject json = null;
                  Exception jsonException = null;

                  try
                  {
                      pdml = pdmlTask.Result;
                  }
                  catch (Exception ex)
                  {
                      pdmlException = ex;
                  }
                  try
                  {
                      json = jsonTask.Result;
                  }
                  catch (Exception ex)
                  {
                      jsonException = ex;
                  }
                  return new TSharkCombinedResults(pdml, json, pdmlException, jsonException);
              }, token);
        }

        public Task<List<TSharkHeuristicProtocolEntry>> GetHeurDissectors()
        {
            return Task.Factory.StartNew(() =>
            {
                string args = "-G heuristic-decodes";
                ProcessStartInfo psi = new ProcessStartInfo(_tsharkPath, args)
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Minimized
                };
                Process p = Process.Start(psi);

                StreamReader errorStream = p.StandardError;
                StreamReader standardStream = p.StandardOutput;
                Stream rawStdStream = standardStream.BaseStream;
                StreamReader unicodeReader = new StreamReader(rawStdStream, Encoding.UTF8);

                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();
                while (!p.WaitForExit(1000))
                {
                    output.Append(unicodeReader.ReadToEnd());
                    error.Append(errorStream.ReadToEnd());
                }

                output.Append(unicodeReader.ReadToEnd());
                error.Append(errorStream.ReadToEnd());

                if (p.ExitCode != 0)
                {
                    if (output.Length > 0)
                    {
                        // Exit code isn't 0 but we have output so dismissing for now...
                    }
                    else
                    {
                        // No output, show exit code and error
                        throw new Exception($"TShark returned with exit code: {p.ExitCode}\r\n{error}");
                    }

                }

                string rawStdOut = output.ToString();
                var lines = rawStdOut.Split('\n');
                var heuristDissectorsEntries = (from line in lines
                                                let parts = line.Split('\t')
                                                where parts.Length >= 3
                                                let carryingProto = parts[0]
                                                let targetProto = parts[1]
                                                let enabled = (parts[2].Trim() == "T")
                                                select new TSharkHeuristicProtocolEntry(targetProto, carryingProto, enabled)).ToList();

                // Code below tries to find the actual heuristic dissector names since 
                // the way TShark provide them ('carried protocol name' and 'carrying protocol name') is sometimes
                // mis-aligned with the registered name
                var misalignedEntires = from entry in heuristDissectorsEntries
                                        let guessedName = entry.ShortName
                                        where (WiresharkHeuristics.List.Contains(guessedName) == false)
                                        select entry;

                List<TSharkHeuristicProtocolEntry>
                    entriedToRemove = new List<TSharkHeuristicProtocolEntry>(); // Entried which couldn't be saved
                foreach (TSharkHeuristicProtocolEntry misalignedEntry in misalignedEntires)
                {
                    string newName = WiresharkHeuristics.GetActualName(misalignedEntry);
                    if (newName != null)
                    {
                        misalignedEntry.SetCustomShortName(newName);
                    }
                    else
                    {
                        entriedToRemove.Add(misalignedEntry);
                    }
                }

                var finalList = heuristDissectorsEntries.ToList();

                foreach (TSharkHeuristicProtocolEntry entry in entriedToRemove)
                {
                    finalList.Remove(entry);
                }

                return finalList;
            });
        }

    }
}
