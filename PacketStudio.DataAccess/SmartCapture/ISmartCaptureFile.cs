using Haukcode.PcapngUtils.PcapNG.BlockTypes;
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

        void AppendPacket(LinkLayerType linkLayer, byte[] data);
        void MovePacket(int oldIndex, int newIndex);
        void ReplacePacket(LinkLayerType type, byte[] data);

        // Returns a .pcapng file containing all the packets
        string GetPcapngFilePath();
    }


    public class SmartPcapngCaptureFile : ISmartCaptureFile
    {
        PcapngWeakHandle _pcapngWeakHandle;
        List<long> _offsetsList = null;

        public PcapngWeakHandle BackingFile => _pcapngWeakHandle;

        public SmartPcapngCaptureFile(string path)
        {
            _pcapngWeakHandle = new PcapngWeakHandle(path);
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
                _offsetsList = _pcapngWeakHandle.GetPacketsOffsets();
            }

            EnhancedPacketBlock pkt = _pcapngWeakHandle.GetPacketAt(_offsetsList[index]);
            var ifaces = _pcapngWeakHandle.GetInterfaces();
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

        public void AppendPacket(LinkLayerType linkLayer, byte[] data)
        {
            // TODO: ??
        }

        public void MovePacket(int oldIndex, int newIndex)
        {
            // TODO: ??
        }

        public void ReplacePacket(int index, LinkLayerType missing_name, byte[] data)
        {
            throw new NotImplementedException();
        }

        public int GetPacketsCount()
        {
            // TODO: Maybe keep versions of 'list' ("retrival time") and the whole smart object
            // so we know when it's stale and needs to be re-retrieved?
            if (_offsetsList == null) {
                _offsetsList = _pcapngWeakHandle.GetPacketsOffsets();
            }

            return _offsetsList.Count;
        }

        public string[] GetPacketsDescriptions()
        {
            // TODO: Maybe keep versions of 'list' ("retrival time") and the whole smart object
            // so we know when it's stale and needs to be re-retrieved?
            if (_offsetsList == null) {
                _offsetsList = _pcapngWeakHandle.GetPacketsOffsets();
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
            return _pcapngWeakHandle.Path;
        }

        public void ReplacePacket(LinkLayerType link, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}