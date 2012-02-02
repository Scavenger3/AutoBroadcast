using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AutoBroadcastConfig
{
    public delegate void ColorValueChangedEventHandler(ColorRGB color, int value);
    public delegate void ChangePanelColorEventHandler(System.Drawing.Color c);
    public enum ColorRGB
    {
        Red = 0,
        Green = 1,
        Blue = 2,
    }
    public partial class frmMain : Form
    {
        private System.Drawing.Color _tmpColor;
        private int _tmpRed;
        private int _tmpGreen;
        private int _tmpBlue;
        public event ColorValueChangedEventHandler ColorValueChangedEvent;
        public event ChangePanelColorEventHandler ChangePanelColorEvent;
        public static aBList aBroadcasts;
        public static String savepath = "";
        public int selitm;
        public int selmsgind;
        
        public frmMain()
        {
            InitializeComponent();
            ColorValueChangedEvent += new ColorValueChangedEventHandler(frmMain_ColorValueChangedEvent);
            ChangePanelColorEvent += new ChangePanelColorEventHandler(frmMain_ChangePanelColorEvent);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMain_FormClosing);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            ConfR reader = new ConfR();
            reader.saveFile(savepath, aBroadcasts);
            PMain.dispos = true;
            e.Cancel = false;
            
        }

        void frmMain_ChangePanelColorEvent(System.Drawing.Color c)
        {
            pbExample.BackColor = c;
        }

        void frmMain_ColorValueChangedEvent(ColorRGB Color, int Value)
        {
            switch (Color)
            {
                case ColorRGB.Red:
                    _tmpRed = Value;
                    break;
                case ColorRGB.Green:
                    _tmpGreen = Value;
                    break;
                case ColorRGB.Blue:
                    _tmpBlue = Value;
                    break;
                default:
                    break;
            }
            _tmpColor = System.Drawing.Color.FromArgb(_tmpRed, _tmpGreen, _tmpBlue);
            ChangePanelColorEvent(_tmpColor);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            selitm = 0;
            savepath = PMain.path;

            ConfR reader = new ConfR();
            if (File.Exists(savepath))
            {
                aBroadcasts = reader.readFile(savepath);
                MessageBox.Show(aBroadcasts.AutoBroadcast.Count + " broadcasts have been loaded.");
            }
            else
            {
                aBroadcasts = reader.writeFile(savepath);
                MessageBox.Show("Empty Config file being created!");
            }

            foreach (aBc bc in aBroadcasts.AutoBroadcast)
            {
                bcList.Items.Add(bc.Name);
            }

            //Set values
            txtName.Text = aBroadcasts.AutoBroadcast[0].Name;

            chkEnabled.Checked = aBroadcasts.AutoBroadcast[0].Enabled;

            if (aBroadcasts.AutoBroadcast[0].Interval > 3600)
                numInterval.Value = 3600;
            else
                numInterval.Value = aBroadcasts.AutoBroadcast[0].Interval;

            txtRed.Text = aBroadcasts.AutoBroadcast[0].ColorR.ToString();
            txtGreen.Text = aBroadcasts.AutoBroadcast[0].ColorG.ToString();
            txtBlue.Text = aBroadcasts.AutoBroadcast[0].ColorB.ToString();

            foreach (string msg in aBroadcasts.AutoBroadcast[0].Messages)
            {
                lstMsgs.Items.Add(msg);
            }

            foreach (string grp in aBroadcasts.AutoBroadcast[0].Groups)
            {
                lstGrps.Items.Add(grp);
            }

            bcList.SetSelected(0, true);
            lstMsgs.SetSelected(0, true);
            if (lstGrps.Items.Count != 0)
                lstGrps.SetSelected(0, true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfR reader = new ConfR();
            reader.saveFile(savepath, aBroadcasts);
            bcList.Items.Clear();
            foreach (aBc bc in aBroadcasts.AutoBroadcast)
            {
                bcList.Items.Add(bc.Name);
            }
            bcList.SetSelected(selitm, true);
        }

        private void btnbclAdd_Click(object sender, EventArgs e)
        {
            if (txtbclNew.Text == "")
            {
                MessageBox.Show("Please enter a broadcast name!");
            }
            else
            {
                bcList.Items.Add(txtbclNew.Text);

                List<string> defMessages = new List<string> { "", "", "", "", "", "", "" };
                List<string> defGroups = new List<string> { };
                aBc newBC = new aBc(txtbclNew.Text, false, defMessages, 255, 255, 255, 300, defGroups);
                aBroadcasts.AutoBroadcast.Add(newBC);
                txtbclNew.Text = "";
            }
        }

        private void btnHLPName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This name is not important to broadcasts, It is only used in the config.");
        }

        private void btnHLPEnabled_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Check this box to Enable the broadcast.");
        }

        private void btnHLPInterval_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Interval (In Seconds) Between broadcasts of this message.");
        }

        private void btnHLPMessages_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is the actual Broadcast Text. Only 7 lines can be showed at once.");
        }

        private void btnHLPColor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The colour the broadcast is displayed in, For a list of RGB Colours, Visit:" + Environment.NewLine + "http://www.tayloredmktg.com/rgb/");
        }

        private void btnHLPGroups_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This broadcast will only be shown to people in these groups, dont add any groups to broadcast to everyone!");
        }

        private void txtRed_TextChanged(object sender, EventArgs e)
        {
            if (txtRed.Text != "")
            {
                try
                {
                    int red = int.Parse(txtRed.Text);
                    if (red > 255)
                    {
                        MessageBox.Show("This can only be an Interger from 0-255");
                        txtRed.Text = "255";
                        return;
                    }
                    trbRed.Value = red;
                    ColorValueChangedEvent(ColorRGB.Red, trbRed.Value);
                }
                catch (Exception)
                {
                    MessageBox.Show("This can only be an Interger from 0-255");
                    string newtxt = "";
                    List<char> ints = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};
                    foreach (char ch in txtRed.Text.ToCharArray())
                    {
                        if (ints.Contains(ch))
                        {
                            newtxt += ch;
                        }
                    }
                    txtRed.Text = newtxt;
                    return;
                }
            }
        }

        private void trbRed_Scroll(object sender, EventArgs e)
        {
            this.txtRed.Text = this.trbRed.Value.ToString();
            aBroadcasts.AutoBroadcast[selitm].ColorR = trbRed.Value;
            ColorValueChangedEvent(ColorRGB.Red, trbRed.Value);
        }

        private void trbGreen_Scroll(object sender, EventArgs e)
        {
            this.txtGreen.Text = this.trbGreen.Value.ToString();
            aBroadcasts.AutoBroadcast[selitm].ColorG = trbGreen.Value;
            ColorValueChangedEvent(ColorRGB.Green, trbGreen.Value);
        }

        private void trbBlue_Scroll(object sender, EventArgs e)
        {
            this.txtBlue.Text = this.trbBlue.Value.ToString();
            aBroadcasts.AutoBroadcast[selitm].ColorB = trbBlue.Value;
            ColorValueChangedEvent(ColorRGB.Blue, trbBlue.Value);
        }

        private void txtGreen_TextChanged(object sender, EventArgs e)
        {
            if (txtGreen.Text != "")
            {
                try
                {
                    int Green = int.Parse(txtGreen.Text);
                    if (Green > 255)
                    {
                        MessageBox.Show("This can only be an Interger from 0-255");
                        txtGreen.Text = "255";
                        return;
                    }
                    trbGreen.Value = Green;
                    ColorValueChangedEvent(ColorRGB.Green, trbGreen.Value);
                }
                catch (Exception)
                {
                    MessageBox.Show("This can only be an Interger from 0-255");
                    string newtxt = "";
                    List<char> ints = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                    foreach (char ch in txtGreen.Text.ToCharArray())
                    {
                        if (ints.Contains(ch))
                        {
                            newtxt += ch;
                        }
                    }
                    txtGreen.Text = newtxt;
                    return;
                }
            }
        }

        private void txtBlue_TextChanged(object sender, EventArgs e)
        {
            if (txtBlue.Text != "")
            {
                try
                {
                    int Blue = int.Parse(txtBlue.Text);
                    if (Blue > 255)
                    {
                        MessageBox.Show("This can only be an Interger from 0-255");
                        txtBlue.Text = "255";
                        return;
                    }
                    trbBlue.Value = Blue;
                    ColorValueChangedEvent(ColorRGB.Blue, trbBlue.Value);
                }
                catch (Exception)
                {
                    MessageBox.Show("This can only be an Interger from 0-255");
                    string newtxt = "";
                    List<char> ints = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
                    foreach (char ch in txtBlue.Text.ToCharArray())
                    {
                        if (ints.Contains(ch))
                        {
                            newtxt += ch;
                        }
                    }
                    txtBlue.Text = newtxt;
                    return;
                }
            }
        }

        private void btnbclRem_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Are you sure you want to delete Broadcast: " + aBroadcasts.AutoBroadcast[selitm].Name, "", MessageBoxButtons.YesNo)) == DialogResult.Yes)
            {
                bcList.Items.RemoveAt(selitm);
                aBroadcasts.AutoBroadcast.RemoveAt(selitm);
            }
        }

        private void lstMsgs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selmsgind != lstMsgs.SelectedIndex)
            {
                selmsgind = lstMsgs.SelectedIndex;
                txtMsg.Text = lstMsgs.Text;
                if (selmsgind != -1)
                    lstMsgs.SetSelected(selmsgind, true);
            }
        }

        private void btnMsgEdit_Click(object sender, EventArgs e)
        {
            lstMsgs.Items[selmsgind] = txtMsg.Text;
            aBroadcasts.AutoBroadcast[selitm].Messages[selmsgind] = txtMsg.Text;
        }

        private void bcList_SelectedIndexChanged(object sender, EventArgs e)
        {

            ConfR reader = new ConfR();
            reader.saveFile(savepath, aBroadcasts);
            if (bcList.SelectedIndex != selitm)
            {
                selitm = bcList.SelectedIndex;
                bcList.Items.Clear();
                foreach (aBc bc in aBroadcasts.AutoBroadcast)
                {
                    bcList.Items.Add(bc.Name);
                }
                bcList.SetSelected(selitm, true);
            }

            if (bcList.SelectedIndex == -1)
            {
                bcList.SetSelected(0, true);
                selitm = 0;
            }
            else
                selitm = bcList.SelectedIndex;

            txtName.Text = aBroadcasts.AutoBroadcast[selitm].Name;

            chkEnabled.Checked = aBroadcasts.AutoBroadcast[selitm].Enabled;

            if (aBroadcasts.AutoBroadcast[selitm].Interval > 3600)
                numInterval.Value = 3600;
            else
                numInterval.Value = aBroadcasts.AutoBroadcast[selitm].Interval;

            txtRed.Text = aBroadcasts.AutoBroadcast[selitm].ColorR.ToString();
            txtGreen.Text = aBroadcasts.AutoBroadcast[selitm].ColorG.ToString();
            txtBlue.Text = aBroadcasts.AutoBroadcast[selitm].ColorB.ToString();

            lstMsgs.Items.Clear();
            foreach (string msg in aBroadcasts.AutoBroadcast[selitm].Messages)
            {
                lstMsgs.Items.Add(msg);
            }

            lstGrps.Items.Clear();
            foreach (string grp in aBroadcasts.AutoBroadcast[selitm].Groups)
            {
                lstGrps.Items.Add(grp);
            }

            lstMsgs.SetSelected(0, true);
            if (lstGrps.Items.Count != 0)
                lstGrps.SetSelected(0, true);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            aBroadcasts.AutoBroadcast[selitm].Name = txtName.Text;
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            aBroadcasts.AutoBroadcast[selitm].Enabled = chkEnabled.Checked;
        }

        private void numInterval_ValueChanged(object sender, EventArgs e)
        {
            aBroadcasts.AutoBroadcast[selitm].Interval = (int)numInterval.Value;
        }

        private void btnMsgClr_Click(object sender, EventArgs e)
        {
            lstMsgs.Items[selmsgind] = "";
            aBroadcasts.AutoBroadcast[selitm].Messages[selmsgind] = "";
        }

        private void btnGrpRem_Click(object sender, EventArgs e)
        {
            int sel = lstGrps.SelectedIndex;
            lstGrps.Items.RemoveAt(sel);
            aBroadcasts.AutoBroadcast[selitm].Groups.RemoveAt(sel);
        }

        private void btnGrpAdd_Click(object sender, EventArgs e)
        {
            lstGrps.Items.Add(txtGrpAdd.Text);
            aBroadcasts.AutoBroadcast[selitm].Groups.Add(txtGrpAdd.Text);
            txtGrpAdd.Text = "";
        }



    }
}
