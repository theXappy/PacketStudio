using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PacketStudio.DataAccess.Providers
{
	public class B2pProvider : IPacketsProvider
	{
		FileStream outputFile;
		StreamReader sr;

		public B2pProvider(string path)
		{
			outputFile = File.OpenRead(path);
		}

		public IEnumerator<PacketSaveData> GetEnumerator()
		{
			sr = new StreamReader(outputFile);

			string magicWordLine;
			try
			{
				if (sr.EndOfStream)
				{
					throw new Exception("ERROR: File is empty.");
				}
				magicWordLine = sr.ReadLine();
				switch (magicWordLine)
				{
					case "M4G1C":
					case "P0NTI4K":
						break;
					default:
						throw new Exception($"ERROR: Unkown file format. Support magics: M4G1C, P0NTI4K. Read magic: {magicWordLine}");

				}
				if (sr.EndOfStream)
				{
					throw new Exception("ERROR: File is empty.");
				}

				while (!sr.EndOfStream)
				{
					string nextLine = sr.ReadLine();
					PacketSaveData psd = PacketSaveData.Parse(magicWordLine,nextLine);
					yield return psd;
				}
			}
			finally
			{
				sr?.Dispose();
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

		public void Dispose()
		{
			outputFile?.Dispose();
			sr?.Dispose();
		}
	}


}
