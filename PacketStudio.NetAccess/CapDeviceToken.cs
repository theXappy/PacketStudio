using System;

namespace PacketStudio.NetAccess
{
    public class CapDeviceToken
    {
        public String Name { get; private set; }
        public String ID { get; private set; }

        public CapDeviceToken(string name, string id)
        {
            Name = name;
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}