namespace AutoBroadcastConfig
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.bcList = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnbclAdd = new System.Windows.Forms.Button();
			this.btnbclRem = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtName = new System.Windows.Forms.TextBox();
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.lstMsgs = new System.Windows.Forms.ListBox();
			this.lstGrps = new System.Windows.Forms.ListBox();
			this.numInterval = new System.Windows.Forms.NumericUpDown();
			this.txtRed = new System.Windows.Forms.TextBox();
			this.trbRed = new System.Windows.Forms.TrackBar();
			this.trbBlue = new System.Windows.Forms.TrackBar();
			this.txtBlue = new System.Windows.Forms.TextBox();
			this.trbGreen = new System.Windows.Forms.TrackBar();
			this.txtGreen = new System.Windows.Forms.TextBox();
			this.txtbclNew = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.btnHLPName = new System.Windows.Forms.Button();
			this.btnHLPEnabled = new System.Windows.Forms.Button();
			this.btnHLPInterval = new System.Windows.Forms.Button();
			this.btnHLPMessages = new System.Windows.Forms.Button();
			this.btnHLPColor = new System.Windows.Forms.Button();
			this.btnHLPGroups = new System.Windows.Forms.Button();
			this.txtGrpAdd = new System.Windows.Forms.TextBox();
			this.btnGrpRem = new System.Windows.Forms.Button();
			this.btnGrpAdd = new System.Windows.Forms.Button();
			this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
			this.rectangleShape3 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
			this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
			this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
			this.txtMsg = new System.Windows.Forms.TextBox();
			this.btnMsgEdit = new System.Windows.Forms.Button();
			this.pbExample = new System.Windows.Forms.PictureBox();
			this.btnMsgRm = new System.Windows.Forms.Button();
			this.btnMsgAdd = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trbRed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trbBlue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trbGreen)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbExample)).BeginInit();
			this.SuspendLayout();
			// 
			// bcList
			// 
			this.bcList.FormattingEnabled = true;
			this.bcList.Location = new System.Drawing.Point(12, 43);
			this.bcList.Name = "bcList";
			this.bcList.Size = new System.Drawing.Size(120, 173);
			this.bcList.TabIndex = 4;
			this.bcList.SelectedIndexChanged += new System.EventHandler(this.bcList_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 13);
			this.label1.TabIndex = 29;
			this.label1.Text = "Select Broadcast:";
			// 
			// btnbclAdd
			// 
			this.btnbclAdd.Location = new System.Drawing.Point(12, 251);
			this.btnbclAdd.Name = "btnbclAdd";
			this.btnbclAdd.Size = new System.Drawing.Size(120, 23);
			this.btnbclAdd.TabIndex = 2;
			this.btnbclAdd.Text = "Add New:";
			this.btnbclAdd.UseVisualStyleBackColor = true;
			this.btnbclAdd.Click += new System.EventHandler(this.btnbclAdd_Click);
			// 
			// btnbclRem
			// 
			this.btnbclRem.Location = new System.Drawing.Point(12, 222);
			this.btnbclRem.Name = "btnbclRem";
			this.btnbclRem.Size = new System.Drawing.Size(120, 23);
			this.btnbclRem.TabIndex = 3;
			this.btnbclRem.Text = "Remove Selected";
			this.btnbclRem.UseVisualStyleBackColor = true;
			this.btnbclRem.Click += new System.EventHandler(this.btnbclRem_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.MediumSeaGreen;
			this.btnSave.Location = new System.Drawing.Point(25, 347);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(91, 42);
			this.btnSave.TabIndex = 22;
			this.btnSave.Text = "Save Config!";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(232, 55);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(138, 20);
			this.txtName.TabIndex = 5;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.chkEnabled.Location = new System.Drawing.Point(232, 99);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(65, 17);
			this.chkEnabled.TabIndex = 6;
			this.chkEnabled.Text = "Enabled";
			this.chkEnabled.UseVisualStyleBackColor = true;
			this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
			// 
			// lstMsgs
			// 
			this.lstMsgs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstMsgs.FormattingEnabled = true;
			this.lstMsgs.Location = new System.Drawing.Point(174, 251);
			this.lstMsgs.Name = "lstMsgs";
			this.lstMsgs.Size = new System.Drawing.Size(335, 95);
			this.lstMsgs.TabIndex = 11;
			this.lstMsgs.SelectedIndexChanged += new System.EventHandler(this.lstMsgs_SelectedIndexChanged);
			// 
			// lstGrps
			// 
			this.lstGrps.FormattingEnabled = true;
			this.lstGrps.Location = new System.Drawing.Point(547, 302);
			this.lstGrps.Name = "lstGrps";
			this.lstGrps.Size = new System.Drawing.Size(133, 69);
			this.lstGrps.TabIndex = 21;
			// 
			// numInterval
			// 
			this.numInterval.Location = new System.Drawing.Point(232, 161);
			this.numInterval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
			this.numInterval.Name = "numInterval";
			this.numInterval.Size = new System.Drawing.Size(138, 20);
			this.numInterval.TabIndex = 7;
			this.numInterval.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.numInterval.ValueChanged += new System.EventHandler(this.numInterval_ValueChanged);
			// 
			// txtRed
			// 
			this.txtRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtRed.Location = new System.Drawing.Point(728, 69);
			this.txtRed.MaxLength = 3;
			this.txtRed.Name = "txtRed";
			this.txtRed.Size = new System.Drawing.Size(29, 21);
			this.txtRed.TabIndex = 13;
			this.txtRed.Text = "0";
			this.txtRed.TextChanged += new System.EventHandler(this.txtRed_TextChanged);
			// 
			// trbRed
			// 
			this.trbRed.Location = new System.Drawing.Point(547, 59);
			this.trbRed.Maximum = 255;
			this.trbRed.Name = "trbRed";
			this.trbRed.Size = new System.Drawing.Size(175, 45);
			this.trbRed.TabIndex = 12;
			this.trbRed.TickFrequency = 15;
			this.trbRed.Scroll += new System.EventHandler(this.trbRed_Scroll);
			// 
			// trbBlue
			// 
			this.trbBlue.Location = new System.Drawing.Point(547, 161);
			this.trbBlue.Maximum = 255;
			this.trbBlue.Name = "trbBlue";
			this.trbBlue.Size = new System.Drawing.Size(175, 45);
			this.trbBlue.TabIndex = 16;
			this.trbBlue.TickFrequency = 15;
			this.trbBlue.Scroll += new System.EventHandler(this.trbBlue_Scroll);
			// 
			// txtBlue
			// 
			this.txtBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBlue.Location = new System.Drawing.Point(728, 171);
			this.txtBlue.MaxLength = 3;
			this.txtBlue.Name = "txtBlue";
			this.txtBlue.Size = new System.Drawing.Size(29, 21);
			this.txtBlue.TabIndex = 17;
			this.txtBlue.Text = "0";
			this.txtBlue.TextChanged += new System.EventHandler(this.txtBlue_TextChanged);
			// 
			// trbGreen
			// 
			this.trbGreen.Location = new System.Drawing.Point(547, 110);
			this.trbGreen.Maximum = 255;
			this.trbGreen.Name = "trbGreen";
			this.trbGreen.Size = new System.Drawing.Size(175, 45);
			this.trbGreen.TabIndex = 14;
			this.trbGreen.TickFrequency = 15;
			this.trbGreen.Scroll += new System.EventHandler(this.trbGreen_Scroll);
			// 
			// txtGreen
			// 
			this.txtGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtGreen.Location = new System.Drawing.Point(728, 119);
			this.txtGreen.MaxLength = 3;
			this.txtGreen.Name = "txtGreen";
			this.txtGreen.Size = new System.Drawing.Size(29, 21);
			this.txtGreen.TabIndex = 15;
			this.txtGreen.Text = "0";
			this.txtGreen.TextChanged += new System.EventHandler(this.txtGreen_TextChanged);
			// 
			// txtbclNew
			// 
			this.txtbclNew.Location = new System.Drawing.Point(13, 283);
			this.txtbclNew.Name = "txtbclNew";
			this.txtbclNew.Size = new System.Drawing.Size(119, 20);
			this.txtbclNew.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(229, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 30;
			this.label2.Text = "Broadcast Name:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(229, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(145, 13);
			this.label3.TabIndex = 31;
			this.label3.Text = "Broadcast Interval: (seconds)";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(539, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(86, 13);
			this.label4.TabIndex = 33;
			this.label4.Text = "Message Colour:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(511, 69);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(30, 13);
			this.label5.TabIndex = 34;
			this.label5.Text = "Red:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(502, 119);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(39, 13);
			this.label6.TabIndex = 35;
			this.label6.Text = "Green:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(510, 171);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(31, 13);
			this.label7.TabIndex = 36;
			this.label7.Text = "Blue:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(171, 235);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(109, 13);
			this.label8.TabIndex = 32;
			this.label8.Text = "Broadcast Messages:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(544, 286);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(95, 13);
			this.label9.TabIndex = 37;
			this.label9.Text = "Broadcast Groups:";
			// 
			// btnHLPName
			// 
			this.btnHLPName.Location = new System.Drawing.Point(376, 55);
			this.btnHLPName.Name = "btnHLPName";
			this.btnHLPName.Size = new System.Drawing.Size(22, 23);
			this.btnHLPName.TabIndex = 23;
			this.btnHLPName.Text = "?";
			this.btnHLPName.UseVisualStyleBackColor = true;
			this.btnHLPName.Click += new System.EventHandler(this.btnHLPName_Click);
			// 
			// btnHLPEnabled
			// 
			this.btnHLPEnabled.Location = new System.Drawing.Point(303, 95);
			this.btnHLPEnabled.Name = "btnHLPEnabled";
			this.btnHLPEnabled.Size = new System.Drawing.Size(22, 23);
			this.btnHLPEnabled.TabIndex = 24;
			this.btnHLPEnabled.Text = "?";
			this.btnHLPEnabled.UseVisualStyleBackColor = true;
			this.btnHLPEnabled.Click += new System.EventHandler(this.btnHLPEnabled_Click);
			// 
			// btnHLPInterval
			// 
			this.btnHLPInterval.Location = new System.Drawing.Point(376, 158);
			this.btnHLPInterval.Name = "btnHLPInterval";
			this.btnHLPInterval.Size = new System.Drawing.Size(22, 23);
			this.btnHLPInterval.TabIndex = 25;
			this.btnHLPInterval.Text = "?";
			this.btnHLPInterval.UseVisualStyleBackColor = true;
			this.btnHLPInterval.Click += new System.EventHandler(this.btnHLPInterval_Click);
			// 
			// btnHLPMessages
			// 
			this.btnHLPMessages.Location = new System.Drawing.Point(286, 225);
			this.btnHLPMessages.Name = "btnHLPMessages";
			this.btnHLPMessages.Size = new System.Drawing.Size(22, 23);
			this.btnHLPMessages.TabIndex = 26;
			this.btnHLPMessages.Text = "?";
			this.btnHLPMessages.UseVisualStyleBackColor = true;
			this.btnHLPMessages.Click += new System.EventHandler(this.btnHLPMessages_Click);
			// 
			// btnHLPColor
			// 
			this.btnHLPColor.Location = new System.Drawing.Point(631, 25);
			this.btnHLPColor.Name = "btnHLPColor";
			this.btnHLPColor.Size = new System.Drawing.Size(22, 23);
			this.btnHLPColor.TabIndex = 27;
			this.btnHLPColor.Text = "?";
			this.btnHLPColor.UseVisualStyleBackColor = true;
			this.btnHLPColor.Click += new System.EventHandler(this.btnHLPColor_Click);
			// 
			// btnHLPGroups
			// 
			this.btnHLPGroups.Location = new System.Drawing.Point(645, 276);
			this.btnHLPGroups.Name = "btnHLPGroups";
			this.btnHLPGroups.Size = new System.Drawing.Size(20, 23);
			this.btnHLPGroups.TabIndex = 28;
			this.btnHLPGroups.Text = "?";
			this.btnHLPGroups.UseVisualStyleBackColor = true;
			this.btnHLPGroups.Click += new System.EventHandler(this.btnHLPGroups_Click);
			// 
			// txtGrpAdd
			// 
			this.txtGrpAdd.Location = new System.Drawing.Point(687, 347);
			this.txtGrpAdd.Name = "txtGrpAdd";
			this.txtGrpAdd.Size = new System.Drawing.Size(119, 20);
			this.txtGrpAdd.TabIndex = 18;
			// 
			// btnGrpRem
			// 
			this.btnGrpRem.Location = new System.Drawing.Point(686, 286);
			this.btnGrpRem.Name = "btnGrpRem";
			this.btnGrpRem.Size = new System.Drawing.Size(120, 23);
			this.btnGrpRem.TabIndex = 20;
			this.btnGrpRem.Text = "Remove Selected";
			this.btnGrpRem.UseVisualStyleBackColor = true;
			this.btnGrpRem.Click += new System.EventHandler(this.btnGrpRem_Click);
			// 
			// btnGrpAdd
			// 
			this.btnGrpAdd.Location = new System.Drawing.Point(686, 315);
			this.btnGrpAdd.Name = "btnGrpAdd";
			this.btnGrpAdd.Size = new System.Drawing.Size(120, 23);
			this.btnGrpAdd.TabIndex = 19;
			this.btnGrpAdd.Text = "Add New:";
			this.btnGrpAdd.UseVisualStyleBackColor = true;
			this.btnGrpAdd.Click += new System.EventHandler(this.btnGrpAdd_Click);
			// 
			// shapeContainer1
			// 
			this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
			this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
			this.shapeContainer1.Name = "shapeContainer1";
			this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape3,
            this.rectangleShape2,
            this.rectangleShape1});
			this.shapeContainer1.Size = new System.Drawing.Size(834, 416);
			this.shapeContainer1.TabIndex = 0;
			this.shapeContainer1.TabStop = false;
			// 
			// rectangleShape3
			// 
			this.rectangleShape3.Location = new System.Drawing.Point(166, 222);
			this.rectangleShape3.Name = "rectangleShape3";
			this.rectangleShape3.Size = new System.Drawing.Size(349, 182);
			// 
			// rectangleShape2
			// 
			this.rectangleShape2.Location = new System.Drawing.Point(535, 272);
			this.rectangleShape2.Name = "rectangleShape2";
			this.rectangleShape2.Size = new System.Drawing.Size(280, 107);
			// 
			// rectangleShape1
			// 
			this.rectangleShape1.Location = new System.Drawing.Point(7, 30);
			this.rectangleShape1.Name = "rectangleShape1";
			this.rectangleShape1.Size = new System.Drawing.Size(134, 282);
			// 
			// txtMsg
			// 
			this.txtMsg.Location = new System.Drawing.Point(174, 381);
			this.txtMsg.Name = "txtMsg";
			this.txtMsg.Size = new System.Drawing.Size(335, 20);
			this.txtMsg.TabIndex = 8;
			// 
			// btnMsgEdit
			// 
			this.btnMsgEdit.Location = new System.Drawing.Point(285, 352);
			this.btnMsgEdit.Name = "btnMsgEdit";
			this.btnMsgEdit.Size = new System.Drawing.Size(103, 23);
			this.btnMsgEdit.TabIndex = 9;
			this.btnMsgEdit.Text = "Edit Selected:";
			this.btnMsgEdit.UseVisualStyleBackColor = true;
			this.btnMsgEdit.Click += new System.EventHandler(this.btnMsgEdit_Click);
			// 
			// pbExample
			// 
			this.pbExample.Location = new System.Drawing.Point(686, 24);
			this.pbExample.Name = "pbExample";
			this.pbExample.Size = new System.Drawing.Size(36, 28);
			this.pbExample.TabIndex = 39;
			this.pbExample.TabStop = false;
			// 
			// btnMsgRm
			// 
			this.btnMsgRm.Location = new System.Drawing.Point(407, 352);
			this.btnMsgRm.Name = "btnMsgRm";
			this.btnMsgRm.Size = new System.Drawing.Size(102, 23);
			this.btnMsgRm.TabIndex = 10;
			this.btnMsgRm.Text = "Remove Selected";
			this.btnMsgRm.UseVisualStyleBackColor = true;
			this.btnMsgRm.Click += new System.EventHandler(this.btnMsgRm_Click);
			// 
			// btnMsgAdd
			// 
			this.btnMsgAdd.Location = new System.Drawing.Point(174, 352);
			this.btnMsgAdd.Name = "btnMsgAdd";
			this.btnMsgAdd.Size = new System.Drawing.Size(88, 23);
			this.btnMsgAdd.TabIndex = 40;
			this.btnMsgAdd.Text = "Add New:";
			this.btnMsgAdd.UseVisualStyleBackColor = true;
			this.btnMsgAdd.Click += new System.EventHandler(this.btnMsgAdd_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(834, 416);
			this.Controls.Add(this.btnMsgAdd);
			this.Controls.Add(this.btnMsgRm);
			this.Controls.Add(this.pbExample);
			this.Controls.Add(this.txtMsg);
			this.Controls.Add(this.btnMsgEdit);
			this.Controls.Add(this.txtGrpAdd);
			this.Controls.Add(this.btnGrpRem);
			this.Controls.Add(this.btnGrpAdd);
			this.Controls.Add(this.btnHLPGroups);
			this.Controls.Add(this.btnHLPColor);
			this.Controls.Add(this.btnHLPMessages);
			this.Controls.Add(this.btnHLPInterval);
			this.Controls.Add(this.btnHLPEnabled);
			this.Controls.Add(this.btnHLPName);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtbclNew);
			this.Controls.Add(this.trbGreen);
			this.Controls.Add(this.txtGreen);
			this.Controls.Add(this.txtBlue);
			this.Controls.Add(this.trbRed);
			this.Controls.Add(this.txtRed);
			this.Controls.Add(this.numInterval);
			this.Controls.Add(this.lstGrps);
			this.Controls.Add(this.lstMsgs);
			this.Controls.Add(this.chkEnabled);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnbclRem);
			this.Controls.Add(this.btnbclAdd);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bcList);
			this.Controls.Add(this.trbBlue);
			this.Controls.Add(this.shapeContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AutoBroadcast Config Editor";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trbRed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trbBlue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trbGreen)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbExample)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox bcList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnbclAdd;
		private System.Windows.Forms.Button btnbclRem;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.ListBox lstMsgs;
		private System.Windows.Forms.ListBox lstGrps;
		private System.Windows.Forms.NumericUpDown numInterval;
		private System.Windows.Forms.TextBox txtRed;
		private System.Windows.Forms.TrackBar trbRed;
		private System.Windows.Forms.TrackBar trbBlue;
		private System.Windows.Forms.TextBox txtBlue;
		private System.Windows.Forms.TrackBar trbGreen;
		private System.Windows.Forms.TextBox txtGreen;
		private System.Windows.Forms.TextBox txtbclNew;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnHLPName;
		private System.Windows.Forms.Button btnHLPEnabled;
		private System.Windows.Forms.Button btnHLPInterval;
		private System.Windows.Forms.Button btnHLPMessages;
		private System.Windows.Forms.Button btnHLPColor;
		private System.Windows.Forms.Button btnHLPGroups;
		private System.Windows.Forms.TextBox txtGrpAdd;
		private System.Windows.Forms.Button btnGrpRem;
		private System.Windows.Forms.Button btnGrpAdd;
		private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
		private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
		private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape3;
		private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
		private System.Windows.Forms.TextBox txtMsg;
		private System.Windows.Forms.Button btnMsgEdit;
		private System.Windows.Forms.PictureBox pbExample;
		private System.Windows.Forms.Button btnMsgRm;
		private System.Windows.Forms.Button btnMsgAdd;
	}
}