using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoBroadcastConfig
{
	static class PMain
	{
		public static string path = string.Empty;
		public static bool dispose = false;
		public static bool forced = false;

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmOpen());
		}
	}
}
