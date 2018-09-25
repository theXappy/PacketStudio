namespace PacketStudio.Core
{
    public class TSharkHeuristicProtocolEntry
    {
        public string ProtoName { get; private set; }
        public string CarryingProto { get; private set; }
        public bool Enabled { get; private set; }
        public string ShortName => ProtoName + "_" + CarryingProto;

        public TSharkHeuristicProtocolEntry(string protoName, string carryingProto, bool enabled)
        {
            ProtoName = protoName;
            CarryingProto = carryingProto;
            Enabled = enabled;
        }
    }
}