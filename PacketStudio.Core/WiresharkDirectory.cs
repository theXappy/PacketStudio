using System.IO;

namespace PacketStudio.Core
{
    public class WiresharkDirectory
    {
        public string WiresharkPath { get; set; }
        public string TsharkPath { get; set; }
        public string CapinfosPath { get; set; }

        public WiresharkDirectory(string wiresharkPath, string tsharkPath, string capinfosPath)
        {
            WiresharkPath = wiresharkPath;
            TsharkPath = tsharkPath;
            CapinfosPath = capinfosPath;
        }

        public WiresharkDirectory(string dir) : this(Path.Combine(dir, "Wireshark.exe"),
                                                Path.Combine(dir, "tshark.exe"),
                                                Path.Combine(dir, "capinfos.exe"))
        {
        }
    }
}