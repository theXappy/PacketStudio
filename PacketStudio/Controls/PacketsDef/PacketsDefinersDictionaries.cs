using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{
    public static class PacketsDefinersDictionaries
    {
        public static IEnumerable<HexStreamType> SupportedTypes => StreamTypeToFirstOffset.Keys;
        public static IReadOnlyDictionary<HexStreamType, int> StreamTypeToFirstOffset { get; set; }
        public static IReadOnlyDictionary<HexStreamType, Func<Control>> StreamTypeToPacketDefineControlFactory { get; set; }

        static PacketsDefinersDictionaries()
        {
            var packetDefiners = typeof(PacketsDefinersDictionaries).Assembly.GetTypes()
                .Where(classType => classType.IsSubclassOf(typeof(UserControl)))
                .Where(classType => classType.GetInterface(nameof(IPacketDefiner)) != null);


            Dictionary<HexStreamType, int> StreamTypeToFirstOffsetTemp = new Dictionary<HexStreamType, int>();
            StreamTypeToFirstOffset = StreamTypeToFirstOffsetTemp;
            Dictionary<HexStreamType, Func<Control>> StreamTypeToPacketDefineControlFactoryTemp = new Dictionary<HexStreamType, Func<Control>>();
            StreamTypeToPacketDefineControlFactory = StreamTypeToPacketDefineControlFactoryTemp;
            foreach (Type packetDefiner in packetDefiners)
            {
                Func<Control> creationFunc = () => Activator.CreateInstance(packetDefiner) as Control;
                IPacketDefiner prototype = Activator.CreateInstance(packetDefiner) as IPacketDefiner;
                HexStreamType hexType = prototype.StreamType;
                int headersLen = prototype.HeadersLength;
                StreamTypeToFirstOffsetTemp[hexType] = headersLen;
                StreamTypeToPacketDefineControlFactoryTemp[hexType] = creationFunc;
            }
        }
    }
}