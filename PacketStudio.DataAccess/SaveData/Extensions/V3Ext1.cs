using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketStudio.DataAccess.SaveData.Extensions
{
    public class V3Ext1
    {
        public const string MAGIC_WORD = "D1GN1TY";
        public string MagicWord => MAGIC_WORD;

        private readonly string _packetName;
        private string _extension;
        public string PacketName => _packetName;
        public string Extension => _extension;

        public V3Ext1(string packetName, string extension)
        {
            _packetName = packetName;
            _extension = extension;
        }
        
    }
}
