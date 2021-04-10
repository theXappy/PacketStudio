using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using PacketStudio.Core;

namespace PacketStudio.NewGUI.WpfJokes
{
    public class WiresharkDirListToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<WiresharkDirectory> list = value as List<WiresharkDirectory>;
            return list.Select(dir => dir.WiresharkPath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class WiresharkDirToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            WiresharkDirectory dir = value as WiresharkDirectory;
            return dir?.WiresharkPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string dirPath = value as string;
            SharksFinder.TryGetByPath(dirPath, out WiresharkDirectory wd);
            return wd;
        }
    }
}