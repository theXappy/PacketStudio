using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PacketStudio.Core
{
	public static class SharksFinder
	{
		private static string _defaultWsPath = @"C:\Program Files\Wireshark\";
		private static string _defaultx86wsPath = @"C:\Program Files (x86)\Wireshark\";
		private static string _defaultDevWsPath = @"C:\Development\wsbuild64\run\RelwithDebInfo\";

		public static List<WiresharkDirectory> GetDirectories()
		{
			HashSet<WiresharkDirectory> output = new HashSet<WiresharkDirectory>();
			WiresharkDirectory next;
			if (TryGetByDirectoryPath(_defaultWsPath, out next))
			{
				output.Add(next);
			}
			if (TryGetByDirectoryPath(_defaultx86wsPath, out next))
			{
				output.Add(next);
			}
			if (TryGetByDirectoryPath(_defaultDevWsPath, out next))
			{
				output.Add(next);
			}
			return output.ToList();
		}

		public static bool TryGetByDirectoryPath(string dirPath, out WiresharkDirectory output)
		{
			string wsPath = dirPath + "wireshark.exe";
			string tsPath = dirPath + "tshark.exe";
			string ciPath = dirPath + "capinfos.exe";
			if (File.Exists(wsPath) && File.Exists(tsPath) && File.Exists(ciPath))
			{
				output = new WiresharkDirectory(wsPath,tsPath,ciPath);
				return true;
			}
			output = null;
			return false;
		}


		public static bool TryGetByPath(string path, out WiresharkDirectory output)
        {
            output = null;
            if (path == null)
                return false;

            if (path.ToUpper().EndsWith(".EXE"))
            {
                path = Path.GetDirectoryName(path);
            }

			if(!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				path += Path.DirectorySeparatorChar;
			}

            return TryGetByDirectoryPath(path, out output);
        }
	}
}
