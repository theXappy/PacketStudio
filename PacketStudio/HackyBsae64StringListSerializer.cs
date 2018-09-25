using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PacketStudio
{
    /// It's a quick hack but it works
    /// Workaround since List<T> isn't serializeable nor is the suggested
    /// 'StringsCollection' in the Settings editor (WTF microsoft?)
    public static class HackyBsae64StringListSerializer
    {
        public static string Serialize(List<string> list)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream memStream = new MemoryStream())
            {
                bf.Serialize(memStream, list);
                byte[] bytes = memStream.GetBuffer();
                return Convert.ToBase64String(bytes);
            }
        }
        public static List<string> Deserialize(string data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream memStream = new MemoryStream(Convert.FromBase64String(data)))
            {
                if (memStream.Length == 0)
                    return null;
                return bf.Deserialize(memStream) as List<string>;
            }
        }
    }
}