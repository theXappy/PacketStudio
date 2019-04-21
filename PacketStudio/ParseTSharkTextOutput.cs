using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PacketStudio
{
    public class ParseTSharkTextOutput
    {
        public class ParsedTsharkTextPacket
        {
            public string Number { get; set; }
            public string Time { get; set; }
            public string Source { get; set; }
            public string Dest { get; set; }
            public string Proto { get; set; }
            public string Length { get; set; }
            public string Info { get; set; }

        }

        // "Linked" to a tab page
        public class LinkedParsedPacket
        {
            public string Number { get; set; }
            public string Time { get; set; }
            public string Source { get; set; }
            public string Dest { get; set; }
            public string Proto { get; set; }
            public string Length { get; set; }
            public string Info { get; set; }
            private TabPage _tabPage;

            public LinkedParsedPacket(TabPage tabPage)
            {
                _tabPage = tabPage;
            }

            public TabPage GetLinkedPage() => _tabPage;

            public static LinkedParsedPacket FromUnlinked(ParsedTsharkTextPacket packet, TabPage page)
            {
                return new LinkedParsedPacket(page)
                {
                    Dest = packet.Dest,
                    Info = packet.Info,
                    Number = packet.Number,
                    Proto = packet.Proto,
                    Source = packet.Source,
                    Time = packet.Time,
                    Length = packet.Length
                };
            }
        }

        public List<ParsedTsharkTextPacket> Packets { get; private set; }

        public ParseTSharkTextOutput(List<ParsedTsharkTextPacket> packets)
        {
            Packets = packets;
        }

        public static ParseTSharkTextOutput Parse(string[] packetsTextLines)
        {
            Regex lessTabs = new Regex("\t+");

            List<ParsedTsharkTextPacket> packets = new List<ParsedTsharkTextPacket>();

            foreach (string packetsTextLine in packetsTextLines)
            {
                string newLine = lessTabs.Replace(packetsTextLine, "\t");
                string[] fields = newLine.Split('\t');
                fields = fields.Select(fld => " " + fld.Trim() + " ").ToArray();
                if (fields.Length < 8)
                {
                    fields = fields.Concat(new string[8 - fields.Length]).ToArray();
                }
                ParsedTsharkTextPacket packet = new ParsedTsharkTextPacket()
                {
                    Number = fields[0],
                    Time = fields[1],
                    Source = fields[2],
                    Dest = fields[4],
                    Proto = fields[5],
                    Length = fields[6],
                    Info = fields[7]
                };

                packets.Add(packet);
            }

            return new ParseTSharkTextOutput(packets);
        }
    }
}