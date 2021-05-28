using System;
using System.Globalization;
using System.Windows.Data;
using PacketStudio.DataAccess;

namespace PacketStudio.NewGUI.WpfJokes
{
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