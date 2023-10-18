namespace MortgageTransfer
{
    partial class AutoTransfer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoTransfer));
            this.label1 = new System.Windows.Forms.Label();
            this.txtRunTime = new System.Windows.Forms.MaskedTextBox();
            this.timeCheck = new System.Windows.Forms.Timer(this.components);
            this.cbFromPeriod = new System.Windows.Forms.ComboBox();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.btnTo = new System.Windows.Forms.Button();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSendNotifications = new System.Windows.Forms.CheckBox();
            this.btnBeginNow = new System.Windows.Forms.Button();
            this.lblErrorFiles = new System.Windows.Forms.Label();
            this.lstErrorFiles = new System.Windows.Forms.ListBox();
            this.lblDone = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblErrors = new System.Windows.Forms.Label();
            this.txtNowDownloading = new System.Windows.Forms.Label();
            this.btnNAS = new System.Windows.Forms.Button();
            this.txtNAS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.chkClient = new System.Windows.Forms.CheckBox();
            this.chkGetAll = new System.Windows.Forms.CheckBox();
            this.lblLastSucceed = new System.Windows.Forms.Label();
            this.txtBackup = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBackup = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.btnFrom = new System.Windows.Forms.Button();
            this.btnSpecific = new System.Windows.Forms.Button();
            this.txtSpecificFolder = new System.Windows.Forms.TextBox();
            this.chkSpecificFolder = new System.Windows.Forms.CheckBox();
            this.chkSFTP = new System.Windows.Forms.CheckBox();
            this.btnSFTP = new System.Windows.Forms.Button();
            this.txtSFTP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Run every";
            // 
            // txtRunTime
            // 
            this.txtRunTime.Location = new System.Drawing.Point(86, 13);
            this.txtRunTime.Mask = "00";
            this.txtRunTime.Name = "txtRunTime";
            this.txtRunTime.Size = new System.Drawing.Size(27, 20);
            this.txtRunTime.TabIndex = 1;
            this.txtRunTime.Text = "1";
            this.txtRunTime.TextChanged += new System.EventHandler(this.TxtRunTimeLeave);
            this.txtRunTime.Leave += new System.EventHandler(this.TxtRunTimeLeave);
            // 
            // timeCheck
            // 
            this.timeCheck.Tick += new System.EventHandler(this.TimerTick);
            // 
            // cbFromPeriod
            // 
            this.cbFromPeriod.FormattingEnabled = true;
            this.cbFromPeriod.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days"});
            this.cbFromPeriod.Location = new System.Drawing.Point(120, 11);
            this.cbFromPeriod.Name = "cbFromPeriod";
            this.cbFromPeriod.Size = new System.Drawing.Size(80, 21);
            this.cbFromPeriod.TabIndex = 2;
            this.cbFromPeriod.Text = "Hours";
            this.cbFromPeriod.SelectedIndexChanged += new System.EventHandler(this.CbFromPeriodLeave);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(248, 13);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(114, 13);
            this.lblCurrentTime.TabIndex = 3;
            this.lblCurrentTime.Text = "Next Run: 8:00 PM";
            // 
            // btnTo
            // 
            this.btnTo.Location = new System.Drawing.Point(461, 79);
            this.btnTo.Name = "btnTo";
            this.btnTo.Size = new System.Drawing.Size(75, 23);
            this.btnTo.TabIndex = 31;
            this.btnTo.Text = "Browse";
            this.btnTo.UseVisualStyleBackColor = true;
            this.btnTo.Click += new System.EventHandler(this.BtnFromClick);
            // 
            // txtTo
            // 
            this.txtTo.BackColor = System.Drawing.Color.White;
            this.txtTo.Location = new System.Drawing.Point(144, 82);
            this.txtTo.Name = "txtTo";
            this.txtTo.ReadOnly = true;
            this.txtTo.Size = new System.Drawing.Size(311, 20);
            this.txtTo.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "ARCH Location";
            // 
            // chkSendNotifications
            // 
            this.chkSendNotifications.AutoSize = true;
            this.chkSendNotifications.Checked = true;
            this.chkSendNotifications.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSendNotifications.Location = new System.Drawing.Point(8, 163);
            this.chkSendNotifications.Name = "chkSendNotifications";
            this.chkSendNotifications.Size = new System.Drawing.Size(128, 17);
            this.chkSendNotifications.TabIndex = 32;
            this.chkSendNotifications.Text = "Send Notifications";
            this.chkSendNotifications.UseVisualStyleBackColor = true;
            // 
            // btnBeginNow
            // 
            this.btnBeginNow.Location = new System.Drawing.Point(8, 283);
            this.btnBeginNow.Name = "btnBeginNow";
            this.btnBeginNow.Size = new System.Drawing.Size(105, 23);
            this.btnBeginNow.TabIndex = 34;
            this.btnBeginNow.Text = "Run Now";
            this.btnBeginNow.UseVisualStyleBackColor = true;
            this.btnBeginNow.Click += new System.EventHandler(this.BtnBeginNowClick);
            // 
            // lblErrorFiles
            // 
            this.lblErrorFiles.AutoSize = true;
            this.lblErrorFiles.Location = new System.Drawing.Point(4, 368);
            this.lblErrorFiles.Name = "lblErrorFiles";
            this.lblErrorFiles.Size = new System.Drawing.Size(65, 13);
            this.lblErrorFiles.TabIndex = 38;
            this.lblErrorFiles.Text = "Error Files";
            // 
            // lstErrorFiles
            // 
            this.lstErrorFiles.FormattingEnabled = true;
            this.lstErrorFiles.Location = new System.Drawing.Point(8, 384);
            this.lstErrorFiles.Name = "lstErrorFiles";
            this.lstErrorFiles.Size = new System.Drawing.Size(542, 95);
            this.lstErrorFiles.TabIndex = 37;
            // 
            // lblDone
            // 
            this.lblDone.AutoSize = true;
            this.lblDone.ForeColor = System.Drawing.Color.Crimson;
            this.lblDone.Location = new System.Drawing.Point(5, 354);
            this.lblDone.Name = "lblDone";
            this.lblDone.Size = new System.Drawing.Size(44, 13);
            this.lblDone.TabIndex = 36;
            this.lblDone.Text = "DONE!";
            this.lblDone.Visible = false;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(143, 160);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(311, 20);
            this.txtEmail.TabIndex = 41;
            this.txtEmail.Leave += new System.EventHandler(this.TxtEmailLeave);
            // 
            // lblErrors
            // 
            this.lblErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrors.AutoEllipsis = true;
            this.lblErrors.ForeColor = System.Drawing.Color.Red;
            this.lblErrors.Location = new System.Drawing.Point(5, 537);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(545, 35);
            this.lblErrors.TabIndex = 42;
            // 
            // txtNowDownloading
            // 
            this.txtNowDownloading.AutoSize = true;
            this.txtNowDownloading.Location = new System.Drawing.Point(10, 489);
            this.txtNowDownloading.MaximumSize = new System.Drawing.Size(450, 0);
            this.txtNowDownloading.Name = "txtNowDownloading";
            this.txtNowDownloading.Size = new System.Drawing.Size(0, 13);
            this.txtNowDownloading.TabIndex = 47;
            // 
            // btnNAS
            // 
            this.btnNAS.Location = new System.Drawing.Point(461, 104);
            this.btnNAS.Name = "btnNAS";
            this.btnNAS.Size = new System.Drawing.Size(75, 23);
            this.btnNAS.TabIndex = 51;
            this.btnNAS.Text = "Browse";
            this.btnNAS.UseVisualStyleBackColor = true;
            this.btnNAS.Click += new System.EventHandler(this.BtnFromClick);
            // 
            // txtNAS
            // 
            this.txtNAS.BackColor = System.Drawing.Color.White;
            this.txtNAS.Location = new System.Drawing.Point(144, 107);
            this.txtNAS.Name = "txtNAS";
            this.txtNAS.ReadOnly = true;
            this.txtNAS.Size = new System.Drawing.Size(311, 20);
            this.txtNAS.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "NAS Location";
            // 
            // cbCompany
            // 
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(204, 207);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(251, 21);
            this.cbCompany.TabIndex = 52;
            this.cbCompany.SelectedIndexChanged += new System.EventHandler(this.cbCompany_SelectedIndexChanged);
            // 
            // chkClient
            // 
            this.chkClient.AutoSize = true;
            this.chkClient.Location = new System.Drawing.Point(7, 209);
            this.chkClient.Name = "chkClient";
            this.chkClient.Size = new System.Drawing.Size(193, 17);
            this.chkClient.TabIndex = 53;
            this.chkClient.Text = "Run Only For Specified Client";
            this.chkClient.UseVisualStyleBackColor = true;
            // 
            // chkGetAll
            // 
            this.chkGetAll.AutoSize = true;
            this.chkGetAll.Location = new System.Drawing.Point(7, 186);
            this.chkGetAll.Name = "chkGetAll";
            this.chkGetAll.Size = new System.Drawing.Size(232, 17);
            this.chkGetAll.TabIndex = 54;
            this.chkGetAll.Text = "Get All Files (Ignore Date Received)";
            this.chkGetAll.UseVisualStyleBackColor = true;
            // 
            // lblLastSucceed
            // 
            this.lblLastSucceed.AutoSize = true;
            this.lblLastSucceed.BackColor = System.Drawing.Color.Transparent;
            this.lblLastSucceed.ForeColor = System.Drawing.Color.Green;
            this.lblLastSucceed.Location = new System.Drawing.Point(248, 26);
            this.lblLastSucceed.Name = "lblLastSucceed";
            this.lblLastSucceed.Size = new System.Drawing.Size(175, 13);
            this.lblLastSucceed.TabIndex = 55;
            this.lblLastSucceed.Text = "Last Successful Run: 8:00 PM";
            // 
            // txtBackup
            // 
            this.txtBackup.BackColor = System.Drawing.Color.White;
            this.txtBackup.Location = new System.Drawing.Point(144, 133);
            this.txtBackup.Name = "txtBackup";
            this.txtBackup.ReadOnly = true;
            this.txtBackup.Size = new System.Drawing.Size(311, 20);
            this.txtBackup.TabIndex = 57;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "Backup Location";
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(461, 131);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 58;
            this.btnBackup.Text = "Browse";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.BtnFromClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "FTP Site Location";
            // 
            // txtFrom
            // 
            this.txtFrom.BackColor = System.Drawing.Color.White;
            this.txtFrom.Location = new System.Drawing.Point(144, 55);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.ReadOnly = true;
            this.txtFrom.Size = new System.Drawing.Size(311, 20);
            this.txtFrom.TabIndex = 27;
            // 
            // btnFrom
            // 
            this.btnFrom.Location = new System.Drawing.Point(461, 53);
            this.btnFrom.Name = "btnFrom";
            this.btnFrom.Size = new System.Drawing.Size(75, 23);
            this.btnFrom.TabIndex = 28;
            this.btnFrom.Text = "Browse";
            this.btnFrom.UseVisualStyleBackColor = true;
            this.btnFrom.Visible = false;
            this.btnFrom.Click += new System.EventHandler(this.BtnFromClick);
            // 
            // btnSpecific
            // 
            this.btnSpecific.Enabled = false;
            this.btnSpecific.Location = new System.Drawing.Point(461, 232);
            this.btnSpecific.Name = "btnSpecific";
            this.btnSpecific.Size = new System.Drawing.Size(75, 23);
            this.btnSpecific.TabIndex = 61;
            this.btnSpecific.Text = "Browse";
            this.btnSpecific.UseVisualStyleBackColor = true;
            this.btnSpecific.Click += new System.EventHandler(this.BtnFromClick);
            // 
            // txtSpecificFolder
            // 
            this.txtSpecificFolder.BackColor = System.Drawing.Color.White;
            this.txtSpecificFolder.Location = new System.Drawing.Point(144, 233);
            this.txtSpecificFolder.Name = "txtSpecificFolder";
            this.txtSpecificFolder.Size = new System.Drawing.Size(311, 20);
            this.txtSpecificFolder.TabIndex = 60;
            // 
            // chkSpecificFolder
            // 
            this.chkSpecificFolder.AutoSize = true;
            this.chkSpecificFolder.Enabled = false;
            this.chkSpecificFolder.Location = new System.Drawing.Point(7, 236);
            this.chkSpecificFolder.Name = "chkSpecificFolder";
            this.chkSpecificFolder.Size = new System.Drawing.Size(109, 17);
            this.chkSpecificFolder.TabIndex = 62;
            this.chkSpecificFolder.Text = "Specific Folder";
            this.chkSpecificFolder.UseVisualStyleBackColor = true;
            this.chkSpecificFolder.CheckedChanged += new System.EventHandler(this.chkSpecificFolder_CheckedChanged);
            // 
            // chkSFTP
            // 
            this.chkSFTP.AutoSize = true;
            this.chkSFTP.Enabled = false;
            this.chkSFTP.Location = new System.Drawing.Point(7, 262);
            this.chkSFTP.Name = "chkSFTP";
            this.chkSFTP.Size = new System.Drawing.Size(69, 17);
            this.chkSFTP.TabIndex = 65;
            this.chkSFTP.Text = "Is SFTP";
            this.chkSFTP.UseVisualStyleBackColor = true;
            this.chkSFTP.CheckedChanged += new System.EventHandler(this.chkSFTP_CheckedChanged);
            // 
            // btnSFTP
            // 
            this.btnSFTP.Enabled = false;
            this.btnSFTP.Location = new System.Drawing.Point(461, 258);
            this.btnSFTP.Name = "btnSFTP";
            this.btnSFTP.Size = new System.Drawing.Size(75, 23);
            this.btnSFTP.TabIndex = 64;
            this.btnSFTP.Text = "Browse";
            this.btnSFTP.UseVisualStyleBackColor = true;
            this.btnSFTP.Visible = false;
            this.btnSFTP.Click += new System.EventHandler(this.BtnFromClick);
            // 
            // txtSFTP
            // 
            this.txtSFTP.BackColor = System.Drawing.Color.White;
            this.txtSFTP.Location = new System.Drawing.Point(144, 259);
            this.txtSFTP.Name = "txtSFTP";
            this.txtSFTP.Size = new System.Drawing.Size(311, 20);
            this.txtSFTP.TabIndex = 63;
            // 
            // AutoTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 581);
            this.Controls.Add(this.chkSFTP);
            this.Controls.Add(this.btnSFTP);
            this.Controls.Add(this.txtSFTP);
            this.Controls.Add(this.chkSpecificFolder);
            this.Controls.Add(this.btnSpecific);
            this.Controls.Add(this.txtSpecificFolder);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.txtBackup);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblLastSucceed);
            this.Controls.Add(this.chkGetAll);
            this.Controls.Add(this.chkClient);
            this.Controls.Add(this.cbCompany);
            this.Controls.Add(this.btnNAS);
            this.Controls.Add(this.txtNAS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNowDownloading);
            this.Controls.Add(this.lblErrors);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblErrorFiles);
            this.Controls.Add(this.lstErrorFiles);
            this.Controls.Add(this.lblDone);
            this.Controls.Add(this.btnBeginNow);
            this.Controls.Add(this.chkSendNotifications);
            this.Controls.Add(this.btnTo);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFrom);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.cbFromPeriod);
            this.Controls.Add(this.txtRunTime);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoTransfer";
            this.Text = "MortgageDriver Sweeper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoTransfer_FormClosing);
            this.Load += new System.EventHandler(this.AutoTransfer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtRunTime;
        private System.Windows.Forms.Timer timeCheck;
        private System.Windows.Forms.ComboBox cbFromPeriod;
        private System.Windows.Forms.Label lblCurrentTime;
        internal System.Windows.Forms.Button btnTo;
        internal System.Windows.Forms.TextBox txtTo;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkSendNotifications;
        private System.Windows.Forms.Button btnBeginNow;
        internal System.Windows.Forms.Label lblErrorFiles;
        internal System.Windows.Forms.ListBox lstErrorFiles;
        internal System.Windows.Forms.Label lblDone;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblErrors;
        internal System.Windows.Forms.Label txtNowDownloading;
        internal System.Windows.Forms.Button btnNAS;
        internal System.Windows.Forms.TextBox txtNAS;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.CheckBox chkClient;
        private System.Windows.Forms.CheckBox chkGetAll;
        private System.Windows.Forms.Label lblLastSucceed;
        internal System.Windows.Forms.TextBox txtBackup;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Button btnBackup;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtFrom;
        internal System.Windows.Forms.Button btnFrom;
        internal System.Windows.Forms.Button btnSpecific;
        internal System.Windows.Forms.TextBox txtSpecificFolder;
        private System.Windows.Forms.CheckBox chkSpecificFolder;
        private System.Windows.Forms.CheckBox chkSFTP;
        internal System.Windows.Forms.Button btnSFTP;
        internal System.Windows.Forms.TextBox txtSFTP;
    }
}