using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace PacketStudio.NewGUI.WpfJokes
{
    public class WiresharkColorFilters
    {
        private static WiresharkColorFilters _instance;
        public static WiresharkColorFilters Instance => _instance ??= new();

        public Dictionary<string, Color> BackColors = new();
        public Dictionary<string, Color> ForeColors = new();
        private WiresharkColorFilters()
        {
            string path =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                    , @"Wireshark\colorfilters");
            foreach (string ruleLine in File.ReadLines(path))
            {
                int firstAtIndex = ruleLine.IndexOf('@');
                if (firstAtIndex == -1)
                {
                    continue;
                }

                string ruleName = ruleLine[(firstAtIndex+1) .. ruleLine.IndexOf('@', firstAtIndex + 1)];
                string encodedColors = ruleLine[(ruleLine.LastIndexOf('@')+1) ..];
                string encodedBackColor = encodedColors.Split(']')[0].Trim('[',']');
                string encodedForeColor = encodedColors.Split(']')[1].Trim('[',']');

                Color DecodeColor(string threePartsString)
                {
                    string[] parts = threePartsString.Split(',');
                    int r = int.Parse(parts[0]);
                    int g = int.Parse(parts[1]);
                    int b = int.Parse(parts[2]);
                    byte rByte = (byte) (r / 257);
                    byte gByte = (byte) (g / 257);
                    byte bByte = (byte) (b / 257);
                    return Color.FromRgb(rByte, gByte, bByte);
                }

                if (!BackColors.ContainsKey(ruleLine))
                {
                    Color background = DecodeColor(encodedBackColor);
                    BackColors[ruleName] = background;
                }

                if (!ForeColors.ContainsKey(ruleName))
                {
                    Color foreground = DecodeColor(encodedForeColor);
                    ForeColors[ruleName] = foreground;
                }
            }
        }

    }
}