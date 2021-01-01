using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PacketStudio.DataAccess.Json;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.DataAccess.Providers
{
    public class P2sFileProviderNG : IPacketsProviderNG
    {
        private string _path;

        public P2sFileProviderNG(string path)
        {
            _path = path;
        }

        public IEnumerator<PacketSaveDataNG> GetEnumerator()
        {
            // TODO: This loads entire file to memory at once, maybe a 'lazy' approach can be made
            // using some sort of lazy json parsing from json.NET?
            var deSerializer = new JsonSerializer<PacketSaveDataNG>();
            string wholeJson = File.ReadAllText(_path);
            PacketSaveDataNG[] packets = deSerializer.Deserialize(wholeJson);
            return packets.Cast<PacketSaveDataNG>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void Dispose()
        {
            // ???
        }
    }
}