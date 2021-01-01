using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PacketStudio.DataAccess.Json;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Providers
{
    public class PssFileProvider : IPacketsProvider
    {
        private string _path;

        public PssFileProvider(string path)
        {
            _path = path;
        }

        public IEnumerator<PacketSaveData> GetEnumerator()
        {
            // TODO: This loads entire file to memory at once, maybe a 'lazy' approach can be made
            // using some sort of lazy json parsing from json.NET?
            var deSerializer = new JsonSerializer<PacketSaveDataV3>();
            string wholeJson = File.ReadAllText(_path);
            PacketSaveDataV3[] packets = deSerializer.Deserialize(wholeJson);
            return packets.Cast<PacketSaveData>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void Dispose()
        {
            // ???
        }
    }
}