using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace PacketStudio.Core
{
	public class TSharkCombinedResults
	{
		public XElement Pdml { get; private set; }
		public JObject JsonRaw { get; private set; }

		public TSharkCombinedResults(XElement pdml, JObject jsonRaw)
		{
			Pdml = pdml;
			JsonRaw = jsonRaw;
		}
	}
}