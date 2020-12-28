using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PacketStudio.Core
{
    public class HexDeserializer
    {
		Regex hexRegex = new Regex("^([A-Fa-f0-9]{2})+$");

		public bool IsHexStream(string txt)
		{
			return hexRegex.IsMatch(txt);
		}

		/// <summary>
		/// Tries to deserialzie a hex string. Throws exceptions if invalid.
		/// </summary>
		/// <param name="rawHex">string to deserialize</param>
		/// <returns>Parsed byte arrray</returns>
		public byte[] Deserialize(string rawHex)
        {
            if (rawHex.StartsWith("0000   ")) // this is a wireshark hex dump string
            {
                rawHex = HandleWiresharkDump(rawHex);
            }
            if (IsWiresharkEscapedString(rawHex))
            {
                rawHex = HandleWiresharkEscapedString(rawHex);
            }
            rawHex = rawHex.Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\t", string.Empty)
                .Replace(" ", string.Empty)
                .Replace("0x", string.Empty)
                .Replace(",", string.Empty);

            if (string.IsNullOrEmpty(rawHex))
            {
                throw new Exception("Byte array was empty after removing sapces,commas and '0x's");
            }

            // since the hexRegex could return false for IsMatch for two reason - if the length is odd or invalid characters are present
            // So first, I'm checking if the length is odd so if the IsMatch function returns false I KNOW it's invalid characters

            if (rawHex.Length % 2 == 1)
            {
                throw new FormatException("Byte array contains odd number of nibbles.");
            }

            if (!hexRegex.IsMatch(rawHex))
            {
                // Finding the bad characters in the string to show the user
                IEnumerable<string> invalidChars = rawHex.Where(c => !Uri.IsHexDigit(c)).Select(invalidChar=> "\'" + invalidChar + '\'').Distinct();
                string exampleInvalidChars = string.Join(" , ", invalidChars);
                throw new Exception("Byte array contains invalid characters.\r\nRemove these characters: " +
                                exampleInvalidChars);
            }

            return StringToByteArray(rawHex);
        }


		/// <summary>
		/// Tries to deserialzie a hex string. Returns false if invalid.
		/// </summary>
		/// <param name="rawHex">string to deserialize</param>
		/// <param name="output">Deserialized byte array or null if failed to deserialize</param>
		public bool TryDeserialize(string rawHex, out byte[] output)
		{
            // Removing comments
            var lines = rawHex.Split('\n');
            string noCommentsHex = "";
            foreach (var line in lines)
            {
                // ReSharper disable once StringIndexOfIsCultureSpecific.1
                int commentStart = line.IndexOf(@"//");
                if (commentStart != -1)
                {
                    noCommentsHex += line.Substring(0, commentStart);
                }
                else
                {
                    noCommentsHex += line;
                }
                noCommentsHex += "\n";
            }

			output = null;
		    if (noCommentsHex.StartsWith("0000   ")) // this is a wireshark hex dump string
		    {
			    noCommentsHex = HandleWiresharkDump(noCommentsHex);
		    }
		    if (IsWiresharkEscapedString(noCommentsHex))
		    {
			    noCommentsHex = HandleWiresharkEscapedString(noCommentsHex);
		    }
		    noCommentsHex = noCommentsHex.Replace("\r", string.Empty)
			    .Replace("\n", string.Empty)
			    .Replace("\t", string.Empty)
			    .Replace(" ", string.Empty)
			    .Replace("0x", string.Empty)
			    .Replace(",", string.Empty);

		    if (string.IsNullOrEmpty(noCommentsHex))
		    {
			    return false;
		    }

		    // since the hexRegex could return false for IsMatch for two reason - if the length is odd or invalid characters are present
		    // So first, I'm checking if the length is odd so if the IsMatch function returns false I KNOW it's invalid characters

		    if (noCommentsHex.Length % 2 == 1)
		    {
			    return false;
		    }

		    if (!hexRegex.IsMatch(noCommentsHex))
		    {
			    return false;
		    }

		    output = StringToByteArray(noCommentsHex);
		    return true;
	    }


		private bool IsWiresharkEscapedString(string rawHex)
        {
            return (rawHex.Contains("\" \\") && rawHex.StartsWith("\"\\x"));
        }

        private string HandleWiresharkEscapedString(string rawHex)
        {
            return rawHex.Replace("\"\\x", string.Empty)
                        .Replace("\"\\\\x", string.Empty)
                        .Replace("\" \\", string.Empty)
                        .Replace("\"\\", string.Empty)
                        .Replace("\\x", string.Empty)
                        .Replace("\"", string.Empty);
        }

        Regex WiresharpDumpHexExtractor = new Regex(@"(?:[0-9a-fA-F]{4}  )((?: [0-9a-fA-F]{2})*)(?:  |\r?\n|$)");
        private string HandleWiresharkDump(string rawHex)
        {
            // This is a wireshark dump. Needs to figure out if hex + ascii or just hex.
            MatchCollection matches = WiresharpDumpHexExtractor.Matches(rawHex);
            if (matches == null || matches.Count == 0)
            {
                throw new Exception("Found a Wireshark dump but failed to extract packets from it...");
            }
            StringBuilder output = new StringBuilder(matches.Count*30);
            foreach (Match match in matches)
            {
                output.Append(match.Groups[1].ToString());
            }
            return output.ToString();
        }


        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}