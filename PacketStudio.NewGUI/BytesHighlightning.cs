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
	}
}