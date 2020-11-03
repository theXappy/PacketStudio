using System;
using PacketStudio.DataAccess;

namespace ByteArrayToPcap.NewGUI.PacketTemplatesControls
{
    public interface IPacketTemplateControl
    {
        event EventHandler Changed;
        bool IsValid { get; }
        (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex);
    }
}