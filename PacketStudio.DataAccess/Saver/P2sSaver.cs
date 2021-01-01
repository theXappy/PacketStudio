using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketStudio.DataAccess.Json;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Saver
{
    public class P2sSaver
    {
        public void Save(string path, IEnumerable<PacketSaveDataNG> packets)
        {
            var serializer = new JsonSerializer<PacketSaveDataNG>();
            byte[] json = serializer.Serialize(packets);
            File.WriteAllBytes(path, json);
        }
    }
}
