using System;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    public interface IPacketTemplateControl
    {
        event EventHandler Changed;
        bool IsValidWithPayload(byte[] raw);
        (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex);


        string GenerateSaveDataJson();
        void LoadSaveDataJson(string json);

    }
}