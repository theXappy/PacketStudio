using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Brush = System.Drawing.Brush;

namespace PacketStudio.NewGUI.WpfJokes
{
    [ValueConversion(typeof(string), typeof(Brush))]
    public class TsharkLineToBackgroundColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color lineColor = Colors.White; // Fallback

            string input = value as string;
            if (input != null)
            {
                string ruleName = input[(input.LastIndexOf('\t') + 1)..];

                // I dont want to use TryGetValue because the 'out' modifier on the color might set it to some other default
                // and I already set it to the fallback I want.
                if (WiresharkColorFilters.Instance.BackColors.ContainsKey(ruleName))
                {
                    lineColor = WiresharkColorFilters.Instance.BackColors[ruleName];
                }
            }
            return new SolidColorBrush(lineColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}