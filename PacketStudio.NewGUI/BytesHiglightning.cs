namespace ByteArrayToPcap.NewGUI
{
	public class BytesHiglightning
	{
		public int Offset { get; set; }
		public int Length { get; set; }

		public BytesHiglightning(int offset, int length)
		{
			Offset = offset;
			Length = length;
		}
	}
}