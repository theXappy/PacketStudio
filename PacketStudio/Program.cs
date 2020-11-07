using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketStudio.NetAccess;

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
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ4MDQ2QDMxMzgyZTMzMmUzMEVsVFV6N2tPS0tJMitVR05LQXRDNTN1bWJtWHlUZUg3dzlFbytuYlUrSkE9;MzQ4MDQ3QDMxMzgyZTMzMmUzMENaazhtZmRYVEVHV1M3MDBhaWZueU83WnRmR0dhUEtBK0x5bWhxSHZHNzA9");

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
