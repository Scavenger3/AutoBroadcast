using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace AutoBroadcastConfig
{
	public partial class frmOpen : Form
	{
		public frmOpen()
		{
			InitializeComponent();
			lblWarning.Text = "WARNING:\nDO NOT modify the config WHILE the server is running!";
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog OFD = new OpenFileDialog();
			OFD.Title = "Open AutoBroadcast Config";
			OFD.Filter = " | AutoBroadcastConfig.json";
			OFD.FileName = "AutoBroadcastConfig.json";

			if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				PMain.path = OFD.FileName;
				frmMain next = new frmMain();
				this.Hide();
				next.ShowDialog();
				if (PMain.dispose == true)
					Dispose();
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			SaveFileDialog SFD = new SaveFileDialog();
			SFD.Title = "Open AutoBroadcast Config";
			SFD.Filter = "AutoBroadcastConfig.json | AutoBroadcastConfig.json";
			SFD.FileName = "AutoBroadcastConfig.json";

			if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				PMain.path = SFD.FileName;
				if (File.Exists(SFD.FileName))
				{
					File.Delete(SFD.FileName);
				}
				frmMain next = new frmMain();
				this.Hide();
				next.ShowDialog();
				if (PMain.dispose == true)
					Dispose();
			}
		}
	}
}