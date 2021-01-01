using System.IO;
using System.Text;
using Newtonsoft.Json;
using PacketStudio.DataAccess.SaveData.Extensions;

namespace PacketStudio.DataAccess.Json
{
    public class V3Ext1JsonSerializer
    {
        public byte[] Serialize(V3Ext1 ext)
        {
            return Encoding.UTF8.GetBytes(SerializeToString(ext));
        }
        /// <summary>
        /// Serializes to UTF-8 string containing a json
        /// </summary>
        public string SerializeToString(V3Ext1 ext)
        {
            Newtonsoft.Json.JsonSerializer jSerializer = new Newtonsoft.Json.JsonSerializer();
            jSerializer.Formatting = Formatting.Indented;
            TextWriter tWriter = new StringWriter();
            jSerializer.Serialize(tWriter, ext);
            return tWriter.ToString();
        }

        public V3Ext1 Deserialize(byte[] utf8Json)
        {
            return Deserialize(Encoding.UTF8.GetString(utf8Json));
        }

        public V3Ext1 Deserialize(string json)
        {
            Newtonsoft.Json.JsonSerializer jSerializer = new Newtonsoft.Json.JsonSerializer();
            TextReader tReader = new StringReader(json);
            JsonReader jReader = new JsonTextReader(tReader);
            return jSerializer.Deserialize<V3Ext1>(jReader);
        }

    }
}