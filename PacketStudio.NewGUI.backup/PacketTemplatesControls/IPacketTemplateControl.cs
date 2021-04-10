using System;
using System.Collections.Generic;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    public interface IPacketTemplateControl
    {
        event EventHandler Changed;
        bool IsValidWithPayload(byte[] raw);
        (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex);


        Dictionary<string,string> GenerateSaveDetails();
        void LoadSaveDetails(Dictionary<string,string> data);

    }
}