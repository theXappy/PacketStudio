using System;
using System.Linq;
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
