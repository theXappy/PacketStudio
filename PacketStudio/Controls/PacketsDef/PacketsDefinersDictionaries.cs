using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{
    public static class PacketsDefinersDictionaries
    {
        public static Dictionary<HexStreamType, int> StreamTypeToFirstOffset { get; set; }
        public static Dictionary<HexStreamType, Func<Control>> StreamTypeToPacketDefineControlFactory { get; set; }

        static PacketsDefinersDictionaries()
        {
            var packetDefiners = typeof(PacketsDefinersDictionaries).Assembly.GetTypes()
                .Where(classType => classType.IsSubclassOf(typeof(UserControl)))
                .Where(classType => classType.GetInterface(nameof(IPacketDefiner)) != null);


            StreamTypeToFirstOffset = new Dictionary<HexStreamType, int>();
            StreamTypeToPacketDefineControlFactory = new Dictionary<HexStreamType, Func<Control>>();
            foreach (Type packetDefiner in packetDefiners)
            {
                Func<Control> creationFunc = () => Activator.CreateInstance(packetDefiner) as Control;
                IPacketDefiner prototype = Activator.CreateInstance(packetDefiner) as IPacketDefiner;
                HexStreamType hexType = prototype.StreamType;
                int headersLen = prototype.HeadersLength;
                StreamTypeToFirstOffset[hexType] = headersLen;
                StreamTypeToPacketDefineControlFactory[hexType] = creationFunc;
            }
        }
    }
}