using System;
using System.IO;
using System.Threading.Tasks;
using CliWrap.Models;

namespace PacketStudio.Core
{
    public class CapInfosInterop
    {
        private string _capinfosPath;

        public string CapinfosPath
        {
            get => _capinfosPath;
            set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException("Couldn't find Capinfos at the given location. Path: " + value);
                }
                _capinfosPath = value;
            }
        }

        public CapInfosInterop(string capinfosPath)
        {
            CapinfosPath = capinfosPath; // This also checks if the file exists
        }

        public int GetPacketsCount(string captureFile)
        {
            CliWrap.Cli cli = new CliWrap.Cli(_capinfosPath);
            Task<ExecutionOutput> capinfosTask = cli.ExecuteAsync($"{captureFile} -c");
            bool timedout = !capinfosTask.Wait(2_000); // Waiting 2 seconds max

            if (timedout)
            {
                throw new Exception("It took capinfos too much time to execute.");
            }

            if (capinfosTask.IsCanceled) // task was canceled
                throw new TaskCanceledException();

            ExecutionOutput res = capinfosTask.Result;
            if (res.ExitCode != 0) // Capinfos returned an error exit code
            {
                // Show the exit code + errors
                throw new Exception($"Capinfos returned with exit code: {res.ExitCode}\r\n{capinfosTask.Result.StandardError}");
            }

            string output = res.StandardOutput;
            string numberOfPacketsString = "Number of packets:";
            int numPacketsIndex = output.IndexOf(numberOfPacketsString) + numberOfPacketsString.Length;
            string packetsCountStr = output.Substring(numPacketsIndex).Trim();

            if (packetsCountStr.Contains(" k"))
            {
                packetsCountStr.Replace(" k", "000");
            }

            int parsedCount;
            if (!int.TryParse(packetsCountStr, out parsedCount))
            {
                throw new Exception("Failed to parse the result of capinfos.\r\n\r\nRaw output:\r\n"+output);
            }
            return parsedCount;
        }
    }
}