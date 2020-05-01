using System.Collections.Generic;
using SharpPcap;
using SharpPcap.Npcap;
using SharpPcap.WinPcap;
#pragma warning disable 618 // WinPcapDevice is obsolete but i know what i'm doing

namespace PacketStudio.NetAccess
{
    public static class PacketSender
    {
        private static List<CapDeviceToken> _availableDevices = null;
        private static Dictionary<string, ICaptureDevice> _mapping = new Dictionary<string, ICaptureDevice>();
        public static List<CapDeviceToken> AvailableDevices 
        {
            get
            {
                if (_availableDevices == null)
                {
                    _availableDevices = new List<CapDeviceToken>();
                    _mapping = new Dictionary<string, ICaptureDevice>();
                    foreach (ICaptureDevice dev in CaptureDeviceList.Instance)
                    {
                        string id;
                        string name;

                        if (dev is WinPcapDevice windDev)
                        {
                            id = windDev.Interface.Name;
                            name = windDev.Interface.FriendlyName;
                        }
                        else if (dev is NpcapDevice npDev)
                        {
                            id = npDev.Interface.Name;
                            name = npDev.Interface.FriendlyName;
                        }
                        else
                        {
                            continue;
                        }

                        _mapping[id] = dev;

                        CapDeviceToken t = new CapDeviceToken(name, id);
                        _availableDevices.Add(t);
                        
                    }
                }
                return _availableDevices;
            }
        }
        public static void Send(CapDeviceToken t,byte[] packet)
        {
            ICaptureDevice dev = _mapping[t.ID];
            dev.Open();
            dev.SendPacket(packet);
            dev.Close();
        }
    }
}
