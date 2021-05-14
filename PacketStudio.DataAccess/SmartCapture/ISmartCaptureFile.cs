﻿using Haukcode.PcapngUtils.PcapNG.BlockTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PacketStudio.DataAccess.SmartCapture
{
    public interface ISmartCaptureFile : IDisposable
    {
        int GetPacketsCount();
        string[] GetPacketsDescriptions();
        (LinkLayerType linkLayer, byte[] data) GetPacket(int index);

        // Returns a .pcapng file containing all the packets
        string GetPcapngFilePath();
    }


    public class SmartPcapngCaptureFile : ISmartCaptureFile
    {
        WeakPcapng _weakPcapng;

        List<long> _offsetsList = null;

        public SmartPcapngCaptureFile(string path)
        {
            _weakPcapng = new WeakPcapng(path);
        }

        public void Dispose()
        {
            // TODO: IDK ?
            //throw new NotImplementedException();
        }

        public (LinkLayerType linkLayer, byte[] data) GetPacket(int index)
        {
            if (_offsetsList == null || _offsetsList.Count <= index)
            {
                _offsetsList = _weakPcapng.GetPacketsOffsets();
            }

            EnhancedPacketBlock pkt = _weakPcapng.GetPacketAt(_offsetsList[index]);
            var ifaces = _weakPcapng.GetInterfaces();
            InterfaceDescriptionBlock iface = null;
            try
            {
                iface = ifaces.ElementAt(pkt.AssociatedInterfaceID.Value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Issues when "+ex);
            }

            return ((LinkLayerType)iface.LinkType, pkt.Data);
        }

        public int GetPacketsCount()
        {
            // TODO: Maybe keep versions of 'list' ("retrival time") and the whole smart object
            // so we know when it's stale and needs to be re-retrieved?
            if (_offsetsList == null) {
                _offsetsList = _weakPcapng.GetPacketsOffsets();
            }

            return _offsetsList.Count;
        }

        public string[] GetPacketsDescriptions()
        {
            // TODO: Maybe keep versions of 'list' ("retrival time") and the whole smart object
            // so we know when it's stale and needs to be re-retrieved?
            if (_offsetsList == null) {
                _offsetsList = _weakPcapng.GetPacketsOffsets();
            }

            // TODO: Use TShark for this
            string[] output = new string[_offsetsList.Count];
            for (int i = 0; i < _offsetsList.Count; i++) {
                output[i] = $"{i}. Enhanced Packet Block at offset: {_offsetsList[i]}";
            }
            return output;
        }

        public string GetPcapngFilePath()
        {
            return _weakPcapng.Path;
        }
    }
}