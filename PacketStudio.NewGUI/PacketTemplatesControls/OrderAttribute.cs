using System;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class OrderAttribute : Attribute
    {
        public uint Order { get; set; }

        public OrderAttribute(uint order)
        {
            Order = order;
        }
    }
}