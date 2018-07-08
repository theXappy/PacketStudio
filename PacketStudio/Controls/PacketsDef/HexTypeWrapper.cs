using Humanizer;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{
    public class HexTypeWrapper
    {
        public HexStreamType Type { get; private set; }

        public HexTypeWrapper(HexStreamType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type.Humanize(LetterCasing.Title);
        }

        protected bool Equals(HexTypeWrapper other)
        {
            return Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HexTypeWrapper) obj);
        }

        public override int GetHashCode()
        {
            return (int) Type;
        }

        public static implicit operator HexTypeWrapper(HexStreamType type)
        {
            return new HexTypeWrapper(type);
        }
    }
}