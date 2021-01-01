using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Json
{
    public class JsonSerializer<T>
    {
        public byte[] Serialize(IEnumerable<T> psd)
        {
            T[] psdArray = psd as T[] ?? psd.ToArray();
            Newtonsoft.Json.JsonSerializer jSerializer = new Newtonsoft.Json.JsonSerializer();
            jSerializer.Formatting = Formatting.Indented;
            TextWriter tWriter = new StringWriter();
            jSerializer.Serialize(tWriter, psdArray);
            return Encoding.UTF8.GetBytes(tWriter.ToString());
        }

        public T[] Deserialize(byte[] utf8Json)
        {
            return Deserialize(Encoding.UTF8.GetString(utf8Json));
        }

        public T[] Deserialize(string json)
        {
            Newtonsoft.Json.JsonSerializer jSerializer = new Newtonsoft.Json.JsonSerializer();
            TextReader tReader = new StringReader(json);
            JsonReader jReader = new JsonTextReader(tReader);
            return jSerializer.Deserialize<T[]>(jReader);
        }

    }
}
