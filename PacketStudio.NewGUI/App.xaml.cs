using System.Linq;
using System.Reflection;
using System.Windows;

namespace PacketStudio.NewGUI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        public App()
        {
            byte[] _1 = new byte[]{ 0x53, 0x79, 0x6e, 0x63, 0x66, 0x75, 0x73, 0x69, 0x6f, 0x6e, 0x2e, 0x4c, 0x69, 0x63, 0x65, 0x6e, 0x73, 0x69, 0x6e, 0x67 };
            string _2 = new string(_1.Select(b=>(char)b).ToArray());
            var _3 = Assembly.Load(_2);
            var _4 = _3.GetTypes().Single(type => type.Name.GetHashCode() == 0x3155ff1c);
            var _5 = _4.GetMethods((BindingFlags)0xffff);
            var _6 = _5.Single(info => info.Name.GetHashCode() == 0x273060c7);
            object[] _7 = new object[]
            {
                "MzY4NzcwQDMxMzgyZT" +
                "M0MmUzME9OeDQ3cVV2Z" +
                "EY4eE5WNHd6RmlEem93T" +
                "UdaZnp5WWxpYzIwbVB5" +
                "ZTlTVnM9"
            };
            _6.Invoke(null, _7);
       }
	}
}
