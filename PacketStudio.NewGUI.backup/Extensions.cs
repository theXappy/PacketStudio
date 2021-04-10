using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PacketStudio.NewGUI
{
	public static class Extensions
	{
		public static IList<string> SplitIntoChunks(this string text, int chunkSize)
		{
			List<string> chunks = new List<string>();
			int offset = 0;
			while (offset < text.Length)
			{
				int size = Math.Min(chunkSize, text.Length - offset);
				chunks.Add(text.Substring(offset, size));
				offset += size;
			}
			return chunks;
		}

		public static XAttribute GetAttribute(this XElement xelement, string attributeName)
		{
			return xelement.Attribute(XName.Get(attributeName));
		}

		public static bool ValueIs(this XAttribute attribute, string expectedValue)
		{
			if (attribute == null)
				return false;

			return attribute.Value.Equals(expectedValue);
		}

		public static IEnumerable<XElement> ElementNodes(this XElement element)
		{
			return element.Nodes().Where(node => node is XElement).Cast<XElement>();
		}
	}
}