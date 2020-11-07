using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PacketStudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            byte[] _1 = new byte[]{ 0x53, 0x79, 0x6e, 0x63, 0x66, 0x75, 0x73, 0x69, 0x6f, 0x6e, 0x2e, 0x4c, 0x69, 0x63, 0x65, 0x6e, 0x73, 0x69, 0x6e, 0x67 };
            string _2 = new string(_1.Select(b=>(char)b).ToArray());
            var _3 = Assembly.Load(_2);
            var _4 = _3.GetTypes().Single(type => type.Name.GetHashCode() == 0x3155ff1c);
            var _5 = _4.GetMethods((BindingFlags)0xffff);
            var _6 = _5.Single(info => info.Name.GetHashCode() == 0x273060c7);
            object[] _7 = new object[]
            {
                "MzQ4MDQ2QDMxMzgyZTMzMmUzMEVsVFV6N2tPS0tJMitVR05L" +
                "QXRDNTN1bWJtWHlUZUg3dzlFbytuYlUrSkE9;MzQ4MDQ3QDM" +
                "xMzgyZTMzMmUzMENaazhtZmRYVEVHV1M3MDBhaWZueU83WnR" +
                "mR0dhUEtBK0x5bWhxSHZHNzA9"
            };
            _6.Invoke(null, _7);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm a = new MainForm();
            if (args.Any())
            {
                a.LoadFile(args.First());
            }
            Application.Run(a);
        }
    }
}
