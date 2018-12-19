namespace PacketStudio.Core
{
    public class TSharkHeuristicProtocolEntry
    {
        public string ProtoName { get; set; }
        public string CarryingProto { get;  set; }
        public bool Enabled { get; private set; }
        public string ShortName => _customShortName ?? (ProtoName + "_" + CarryingProto);

        private string _customShortName = null;

        public TSharkHeuristicProtocolEntry(string protoName, string carryingProto, bool enabled)
        {
            ProtoName = protoName;
            CarryingProto = carryingProto;
            Enabled = enabled;
        }

        public void SetCustomShortName(string name)
        {
            _customShortName = name;
        }

        public override string ToString()
        {
            return $"Proto Name: {ProtoName}, Carrying Proto: {CarryingProto}, Abrv: {ShortName}";

        }
    }
}