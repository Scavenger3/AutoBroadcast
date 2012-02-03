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
        #region Vars
        //Colour Events
        private System.Drawing.Color _tmpColor;
        private int _tmpRed;
        private int _tmpGreen;
        private int _tmpBlue;
        public event ColorValueChangedEventHandler ColorValueChangedEvent;
        public event ChangePanelColorEventHandler ChangePanelColorEvent;

        //Config
        public static aBList aBroadcasts;
        public static String savepath = "";

        //BCList
        public List<string> BCList = new List<string>();
        public int BCSel = 0;
        public bool BCLrl = false;
        public bool MSLrl = false;
        #endregion

        public frmMain()
        {
            InitializeComponent();
            ColorValueChangedEvent += new ColorValueChangedEventHandler(frmMain_ColorValueChangedEvent);
            ChangePanelColorEvent += new ChangePanelColorEventHandler(frmMain_ChangePanelColorEvent);
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMain_FormClosing);
        }


        #region Colour Events
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
        #endregion

        #region Colour Change + Set
        private void trbRed_Scroll(object sender, EventArgs e)
        {
            this.txtRed.Text = this.trbRed.Value.ToString();
            aBroadcasts.AutoBroadcast[BCSel].ColorR = trbRed.Value;
            ColorValueChangedEvent(ColorRGB.Red, trbRed.Value);
        }

        private void trbGreen_Scroll(object sender, EventArgs e)
        {
            this.txtGreen.Text = this.trbGreen.Value.ToString();
            aBroadcasts.AutoBroadcast[BCSel].ColorG = trbGreen.Value;
            ColorValueChangedEvent(ColorRGB.Green, trbGreen.Value);
        }

        private void trbBlue_Scroll(object sender, EventArgs e)
        {
            this.txtBlue.Text = this.trbBlue.Value.ToString();
            aBroadcasts.AutoBroadcast[BCSel].ColorB = trbBlue.Value;
            ColorValueChangedEvent(ColorRGB.Blue, trbBlue.Value);
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
                    List<char> ints = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
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
        #endregion

        #region Help Msgs
        private void btnHLPName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This name is not important to Broadcasts, It is used in the Config Only!", "Broadcast Name");
        }

        private void btnHLPEnabled_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Checking this box will enable this broadcast.", "Enable");
        }

        private void btnHLPInterval_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is the Interval (In Seconds) Between broadcasts of this message.", "Interval");
        }

        private void btnHLPMessages_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is the actual message text that will be broadcasted. Terraria can only show 7 Lines at once, so the maximum is 7.", "Messages");
        }

        private void btnHLPColor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is the colour the message will broadcast in (The preview is not accurate compared to what it will look like in-game)" + Environment.NewLine + "The format for colour is RGB, For a list of examples, please visit: http://www.tayloredmktg.com/rgb/", "Colour");
        }

        private void btnHLPGroups_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This broadcast will only be shown to people in these groups. If you want to broadcast to everyone, Remove all groups!", "Groups");
        }
        #endregion

        #region Load, Save and Close
        private void frmMain_Load(object sender, EventArgs e)
        {
            savepath = PMain.path;

            ConfR reader = new ConfR();
            if (File.Exists(savepath))
            {
                aBroadcasts = reader.readFile(savepath);
                if (aBroadcasts.AutoBroadcast.Count < 1)
                {
                    MessageBox.Show("You must have at least 1 Broadcast, Please select Create New And ovewrite your current one!", "Error:");
                    PMain.forced = true;
                    PMain.dispos = true;
                    this.Dispose();
                    this.Close();
                    return;
                }
                else
                    MessageBox.Show(aBroadcasts.AutoBroadcast.Count + " broadcasts have been loaded.");
            }
            else
            {
                aBroadcasts = reader.writeFile(savepath);
                MessageBox.Show("Empty Config file being created!");
            }

            //Load Values! BCList:
            foreach (aBc bc in aBroadcasts.AutoBroadcast)
            {
                BCList.Add(bc.Name);
            }
            BCLrl = false;
            bcList.DataSource = null;
            bcList.DataSource = BCList;
            BCLrl = true;

            //Name:
            txtName.Text = aBroadcasts.AutoBroadcast[BCSel].Name;

            //Enabled:
            chkEnabled.Checked = aBroadcasts.AutoBroadcast[BCSel].Enabled;

            //Interval:
            numInterval.Value = aBroadcasts.AutoBroadcast[BCSel].Interval;

            //Colours:
            txtRed.Text = aBroadcasts.AutoBroadcast[BCSel].ColorR.ToString();
            txtGreen.Text = aBroadcasts.AutoBroadcast[BCSel].ColorG.ToString();
            txtBlue.Text = aBroadcasts.AutoBroadcast[BCSel].ColorB.ToString();

            //Messages:
            MSLrl = false;
            lstMsgs.DataSource = aBroadcasts.AutoBroadcast[BCSel].Messages;
            int MsgSel = lstMsgs.SelectedIndex;
            if (MsgSel != -1 || MsgSel < 8)
                txtMsg.Text = aBroadcasts.AutoBroadcast[BCSel].Messages[MsgSel];
            MSLrl = true;

            //Groups:
            lstGrps.DataSource = aBroadcasts.AutoBroadcast[BCSel].Groups;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfR reader = new ConfR();
            reader.saveFile(savepath, aBroadcasts);

            //ReLoad Values!
            BCList.Clear();
            foreach (aBc bc in aBroadcasts.AutoBroadcast)
            {
                BCList.Add(bc.Name);
            }
            BCLrl = false;
            bcList.DataSource = null;
            bcList.DataSource = BCList;
            BCLrl = true;

            MessageBox.Show("Config saved sucessfuly!", "Save Config");
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!PMain.forced)
            {
                if ((MessageBox.Show("Warning, If you have unsaved changes, they will be lost!" + Environment.NewLine + "Are you sure you want to exit?", "Warning!", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                {
                    PMain.dispos = true;
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }
        #endregion

        #region BC List
        private void btnbclAdd_Click(object sender, EventArgs e)
        {
            if (txtbclNew.Text == "")
            {
                MessageBox.Show("Please enter a broadcast name!");
            }
            else
            {
                List<string> defMessages = new List<string> { "", "", "", "", "", "", "" };
                List<string> defGroups = new List<string> { };
                aBc newBC = new aBc(txtbclNew.Text, false, defMessages, 255, 255, 255, 300, defGroups);

                aBroadcasts.AutoBroadcast.Add(newBC);
                BCList.Clear();
                foreach (aBc bc in aBroadcasts.AutoBroadcast)
                {
                    BCList.Add(bc.Name);
                }
                BCLrl = false;
                bcList.DataSource = null;
                bcList.DataSource = BCList;
                BCLrl = true;

                txtbclNew.Text = "";
            }
        }

        private void btnbclRem_Click(object sender, EventArgs e)
        {
            BCSel = bcList.SelectedIndex;
            if (BCSel == -1 || BCSel >= BCList.Count)
            {
                MessageBox.Show("Selected Broadcast is invalid!", "Error:");
            }
            else if (aBroadcasts.AutoBroadcast.Count == 1)
            {
                MessageBox.Show("You must have at least 1 Broadcast!", "Error:");
            }
            else
            {
                if ((MessageBox.Show("Are you sure you want to delete the Broadacast: " + aBroadcasts.AutoBroadcast[BCSel].Name + "?", "Warning!", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                {
                    aBroadcasts.AutoBroadcast.RemoveAt(BCSel);
                    BCList.Clear();
                    foreach (aBc bc in aBroadcasts.AutoBroadcast)
                    {
                        BCList.Add(bc.Name);
                    }
                    BCLrl = false;
                    bcList.DataSource = null;
                    bcList.DataSource = BCList;
                    BCLrl = true;
                }
            }
        }

        private void bcList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BCLrl)
            {
                BCSel = bcList.SelectedIndex;
                BCList.Clear();
                foreach (aBc bc in aBroadcasts.AutoBroadcast)
                {
                    BCList.Add(bc.Name);
                }
                BCLrl = false;
                bcList.DataSource = null;
                bcList.DataSource = BCList;
                //bcList.SetSelected(BCSel);
                BCLrl = true;

                //Name:
                txtName.Text = aBroadcasts.AutoBroadcast[BCSel].Name;

                //Enabled:
                chkEnabled.Checked = aBroadcasts.AutoBroadcast[BCSel].Enabled;

                //Interval:
                numInterval.Value = aBroadcasts.AutoBroadcast[BCSel].Interval;

                //Colours:
                txtRed.Text = aBroadcasts.AutoBroadcast[BCSel].ColorR.ToString();
                txtGreen.Text = aBroadcasts.AutoBroadcast[BCSel].ColorG.ToString();
                txtBlue.Text = aBroadcasts.AutoBroadcast[BCSel].ColorB.ToString();

                //Messages:
                MSLrl = false;
                lstMsgs.DataSource = aBroadcasts.AutoBroadcast[BCSel].Messages;
                int MsgSel = lstMsgs.SelectedIndex;
                if (MsgSel != -1 || MsgSel < 8)
                    txtMsg.Text = aBroadcasts.AutoBroadcast[BCSel].Messages[MsgSel];
                MSLrl = true;

                //Groups:
                lstGrps.DataSource = aBroadcasts.AutoBroadcast[BCSel].Groups;
            }
        }
        #endregion

        #region Name, Enabled, Colour Set!
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            aBroadcasts.AutoBroadcast[BCSel].Name = txtName.Text;
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            aBroadcasts.AutoBroadcast[BCSel].Enabled = chkEnabled.Checked;
        }

        private void numInterval_ValueChanged(object sender, EventArgs e)
        {
            aBroadcasts.AutoBroadcast[BCSel].Interval = (int)numInterval.Value;
        }
        #endregion

        #region Change Group
        private void btnGrpAdd_Click(object sender, EventArgs e)
        {
            if (txtGrpAdd.Text == "")
            {
                MessageBox.Show("Please enter a group name!");
            }
            else
            {
                aBroadcasts.AutoBroadcast[BCSel].Groups.Add(txtGrpAdd.Text);
                lstGrps.DataSource = null;
                lstGrps.DataSource = aBroadcasts.AutoBroadcast[BCSel].Groups;

                txtGrpAdd.Text = "";
            }
        }

        private void btnGrpRem_Click(object sender, EventArgs e)
        {
            int GrpSel = lstGrps.SelectedIndex;
            if (BCSel == -1 || BCSel >= BCList.Count)
            {
                MessageBox.Show("Selected Broadcast is invalid!", "Error:");
                return;
            }
            else if (GrpSel == -1 || GrpSel >= aBroadcasts.AutoBroadcast[BCSel].Groups.Count)
            {
                MessageBox.Show("Selected Group is invalid!", "Error:");
                return;
            }
            else
            {
                if ((MessageBox.Show("Are you sure you want to remove the Group: " + aBroadcasts.AutoBroadcast[BCSel].Groups[GrpSel] + "?", "Warning!", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                {
                    aBroadcasts.AutoBroadcast[BCSel].Groups.RemoveAt(GrpSel);

                    lstGrps.DataSource = null;
                    lstGrps.DataSource = aBroadcasts.AutoBroadcast[BCSel].Groups;
                }
            }
        }
        #endregion

        #region Change Messages
        private void lstMsgs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MSLrl)
            {
                int MsgSel = lstMsgs.SelectedIndex;
                if (BCSel == -1 || BCSel >= BCList.Count)
                {
                    MessageBox.Show("Selected Broadcast is invalid!", "Error:");
                    return;
                }
                else if (MsgSel == -1 || MsgSel > 7)
                {
                    MessageBox.Show("Selected Message is invalid!", "Error:");
                    return;
                }
                else
                {
                    txtMsg.Text = aBroadcasts.AutoBroadcast[BCSel].Messages[MsgSel];
                }
            }
        }

        private void btnMsgClr_Click(object sender, EventArgs e)
        {
            int MsgSel = lstMsgs.SelectedIndex;
            if (BCSel == -1 || BCSel >= BCList.Count)
            {
                MessageBox.Show("Selected Broadcast is invalid!", "Error:");
                return;
            }
            else if (MsgSel == -1 || MsgSel > 7)
            {
                MessageBox.Show("Selected Message is invalid!", "Error:");
                return;
            }
            else
            {
                aBroadcasts.AutoBroadcast[BCSel].Messages[MsgSel] = "";
                MSLrl = false;
                lstMsgs.DataSource = null;
                lstMsgs.DataSource = aBroadcasts.AutoBroadcast[BCSel].Messages;
                MSLrl = true;
            }
        }

        private void btnMsgEdit_Click(object sender, EventArgs e)
        {
            int MsgSel = lstMsgs.SelectedIndex;
            if (BCSel == -1 || BCSel >= BCList.Count)
            {
                MessageBox.Show("Selected Broadcast is invalid!", "Error:");
                return;
            }
            else if (MsgSel == -1 || MsgSel > 7)
            {
                MessageBox.Show("Selected Message is invalid!", "Error:");
                return;
            }
            else
            {
                aBroadcasts.AutoBroadcast[BCSel].Messages[MsgSel] = txtMsg.Text;
                txtMsg.Text = "";
                MSLrl = false;
                lstMsgs.DataSource = null;
                lstMsgs.DataSource = aBroadcasts.AutoBroadcast[BCSel].Messages;
                MSLrl = true;
            }
        }
        #endregion

    }
}
