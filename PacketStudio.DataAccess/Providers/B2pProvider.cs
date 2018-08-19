using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PacketStudio.DataAccess.SaveData;

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
#pragma warning disable 612 // Disable obsolete warnings
				    case PacketSaveDataV1.MAGIC_WORD:
				    case PacketSaveDataV2.MAGIC_WORD:
#pragma warning restore 612
                    case PacketSaveDataV3.MAGIC_WORD:
						break;
					default:
#pragma warning disable 612 // Disable obsolete warnings
                        throw new Exception($"ERROR: Unkown file format. Support magics: " +
                                            $"({PacketSaveDataV1.MAGIC_WORD}," +
                                            $"{PacketSaveDataV2.MAGIC_WORD}," +
                                            $"{PacketSaveDataV3.MAGIC_WORD})." +
                                            $" Read magic: {magicWordLine}");
#pragma warning restore 612

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
