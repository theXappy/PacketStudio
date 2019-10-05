using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Json
{
    public class SaveDataV3JsonSerializer
    {
        public byte[] Serialize(IEnumerable<PacketSaveDataV3> psd)
        {
            PacketSaveDataV3[] psdArray = psd as PacketSaveDataV3[] ?? psd.ToArray();
            JsonSerializer jSerializer = new JsonSerializer();
            jSerializer.Formatting = Formatting.Indented;
            TextWriter tWriter = new StringWriter();
            jSerializer.Serialize(tWriter, psdArray);
            return Encoding.UTF8.GetBytes(tWriter.ToString());
        }

        public PacketSaveDataV3[] Deserialize(byte[] utf8Json)
        {
            return Deserialize(Encoding.UTF8.GetString(utf8Json));
        }

        public PacketSaveDataV3[] Deserialize(string json)
        {
            JsonSerializer jSerializer = new JsonSerializer();
            TextReader tReader = new StringReader(json);
            JsonReader jReader = new JsonTextReader(tReader);
            return jSerializer.Deserialize<PacketSaveDataV3[]>(jReader);
        }

    }
}
