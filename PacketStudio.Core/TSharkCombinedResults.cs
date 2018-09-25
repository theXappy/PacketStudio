using System;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace PacketStudio.Core
{
	public class TSharkCombinedResults
	{
		public XElement Pdml { get; private set; }
		public JObject JsonRaw { get; private set; }
	    public Exception PdmlException { get; }
	    public Exception JsonException { get; }
	    public bool TotalFailure => PdmlException != null && JsonException != null;

	    public TSharkCombinedResults(XElement pdml, JObject jsonRaw, System.Exception pdmlException, System.Exception jsonException)
		{
			Pdml = pdml;
			JsonRaw = jsonRaw;
		    PdmlException = pdmlException;
		    JsonException = jsonException;
		}
	}
}