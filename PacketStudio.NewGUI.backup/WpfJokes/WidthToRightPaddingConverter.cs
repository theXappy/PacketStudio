using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PacketStudio.NewGUI.WpfJokes
{
	[ValueConversion(typeof(Encoding), typeof(string))]
	public class EncodingToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IEnumerable<Encoding> doubleVal = value as IEnumerable<Encoding>;
            return doubleVal?.Select(enc=>enc.EncodingName);
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = value as string;
            return Encoding.GetEncoding(name);
		}
	}
}
