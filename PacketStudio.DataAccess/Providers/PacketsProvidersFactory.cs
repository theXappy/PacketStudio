using System;
using System.IO;

namespace PacketStudio.DataAccess.Providers
{
    public class PacketsProvidersFactory
    {
        public IPacketsProviderNG Create(string path)
        {
            IPacketsProviderNG provider = null;
            string ext = Path.GetExtension(path);
            switch (ext)
            {
                case "p2s":
                case ".p2s":
                    provider = new P2sFileProviderNG(path);
                    break;
                case "pcap":
                case ".pcap":
                    provider = new PcapProviderNG(path);
                    break;
                case "pcapng":
                case ".pcapng":
                    provider = new PcapNGProviderNG(path);
                    break;
                default:
                    throw new ArgumentException($"Can't create provider for extension '{ext}'");
            }

            return provider;
        }
    }
}