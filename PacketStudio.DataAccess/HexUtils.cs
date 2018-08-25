using System.Linq;

namespace PacketStudio.DataAccess
{
    public static class HexUtils
	{
		public static string ToHex(this byte[] bytes)
		{
			return string.Join(separator: "", values: bytes.Select(b => b.ToString("X2")));
		}
	}
}
