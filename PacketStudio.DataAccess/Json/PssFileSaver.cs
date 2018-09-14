using System.Collections.Generic;
using System.IO;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Json
{
    public class PssFileSaver
    {
        public void Save(string path, IEnumerable<PacketSaveDataV3> packets)
        {
            SaveDataJsonSerializer serializer = new SaveDataJsonSerializer();
            byte[] json = serializer.Serialize(packets);
            File.WriteAllBytes(path, json);
        }
    }
}