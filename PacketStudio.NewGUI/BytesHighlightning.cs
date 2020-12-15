namespace PacketStudio.NewGUI
{
	public class BytesHighlightning
	{
		public int Offset { get; set; }
		public int Length { get; set; }

		public BytesHighlightning(int offset, int length)
		{
			Offset = offset;
			Length = length;
		}

        public override string ToString()
        {
            return $"{nameof(Offset)}: {Offset}, {nameof(Length)}: {Length}";
        }

        protected bool Equals(BytesHighlightning other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Offset == other.Offset && Length == other.Length;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BytesHighlightning) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Offset * 397) ^ Length;
            }
        }

        public static bool operator ==(BytesHighlightning left, BytesHighlightning right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(left, null))
            {
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(BytesHighlightning a, BytesHighlightning b)
        {
            return !(a == b);
        }

        public static BytesHighlightning Empty = new BytesHighlightning(0,0); 
    }
}