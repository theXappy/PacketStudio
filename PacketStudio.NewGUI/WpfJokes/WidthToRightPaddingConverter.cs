using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using PacketStudio.DataAccess;

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

	[ValueConversion(typeof(int), typeof(HexStreamType))]
    public sealed class ListIndexToHexStreamType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HexStreamType)
            {
                return (int) ((HexStreamType) value);
            }

            return 0;
        }

        /// <summary>
        /// Convert Visibility to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return HexStreamType.Raw;

            return (HexStreamType)((int) value);
        }
    }
}
