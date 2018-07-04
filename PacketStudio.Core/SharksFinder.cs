using System.Collections.Generic;
using System.IO;

namespace PacketStudio.Core
{
	public static class SharksFinder
	{
		private static string _defaultWsPath = @"C:\Program Files\Wireshark\";
		private static string _defaultx86wsPath = @"C:\Program Files (x86)\Wireshark\";
		private static string _defaultDevWsPath = @"C:\Program Files\Wireshark\";

		public static List<WiresharkDirectory> GetDirectories()
		{
			List<WiresharkDirectory> output = new List<WiresharkDirectory>();
			WiresharkDirectory next;
			if (TryGetDirectories(_defaultWsPath, out next))
			{
				output.Add(next);
			}
			if (TryGetDirectories(_defaultx86wsPath, out next))
			{
				output.Add(next);
			}
			if (TryGetDirectories(_defaultDevWsPath, out next))
			{
				output.Add(next);
			}
			return output;
		}

		public static bool TryGetDirectories(string dirPath, out WiresharkDirectory output)
		{
			string wsPath = dirPath + "wireshark.exe";
			string tsPath = dirPath + "tshark.exe";
			if (File.Exists(wsPath) && File.Exists(tsPath))
			{
				output = new WiresharkDirectory(wsPath,tsPath);
				return true;
			}
			output = null;
			return false;
		}


		public static bool TryGetByPath(string dirPath, out WiresharkDirectory output)
		{
			if(!dirPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				dirPath += Path.DirectorySeparatorChar;
			}

			if (Directory.Exists(dirPath))
			{
				string wsPath = dirPath + "wireshark.exe";
				string tsPath = dirPath + "tshark.exe";

				output = new WiresharkDirectory(wsPath, tsPath);
				return true;
			}
			output = null;
			return false;
		}
	}
}
