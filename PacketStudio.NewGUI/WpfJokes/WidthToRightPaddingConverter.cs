using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ByteArrayToPcap.NewGUI.WpfJokes
{
	[ValueConversion(typeof(int), typeof(Thickness))]
	public class WidthToRightPaddingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double doubleVal = (double) value;
			return new Thickness(0, 0, doubleVal, 0);
	}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
