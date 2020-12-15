using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PacketStudio.DataAccess.Json
{
    public class DictJsonSerializer
    {
        public string Serialize(Dictionary<string,object> obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return json;
        }


        public Dictionary<string,object> Deserialize(string json)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            return dict;

        }

        
    }
}