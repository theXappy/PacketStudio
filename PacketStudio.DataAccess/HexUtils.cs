using System.Linq;

namespace PacketStudio.DataAccess
{
	internal static class HexUtils
	{
		internal static string ToHex(this byte[] bytes)
		{
			return string.Join(separator: "", values: bytes.Select(b => b.ToString("X2")));
		}
	}
}
