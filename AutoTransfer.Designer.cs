namespace MortgageTransfer_FLUW
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoanFileLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFtpLocation = new System.Windows.Forms.TextBox();
            this.btnRunNow = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.lstErrorLog = new System.Windows.Forms.ListBox();
            this.txtToEmail = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btBrowse2 = new System.Windows.Forms.Button();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbFromPeriod = new System.Windows.Forms.ComboBox();
            this.txtRunTime = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timerInterval = new System.Windows.Forms.Timer(this.components);
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblLastSucceed = new System.Windows.Forms.Label();
            this.Start = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTP Site Location";
            // 
            // txtLoanFileLocation
            // 
            this.txtLoanFileLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoanFileLocation.Location = new System.Drawing.Point(109, 47);
            this.txtLoanFileLocation.Name = "txtLoanFileLocation";
            this.txtLoanFileLocation.ReadOnly = true;
            this.txtLoanFileLocation.Size = new System.Drawing.Size(300, 20);
            this.txtLoanFileLocation.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "LoanFileLoacation";
            // 
            // txtFtpLocation
            // 
            this.txtFtpLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFtpLocation.Location = new System.Drawing.Point(109, 19);
            this.txtFtpLocation.Name = "txtFtpLocation";
            this.txtFtpLocation.ReadOnly = true;
            this.txtFtpLocation.Size = new System.Drawing.Size(300, 20);
            this.txtFtpLocation.TabIndex = 3;
            // 
            // btnRunNow
            // 
            this.btnRunNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunNow.Location = new System.Drawing.Point(361, 240);
            this.btnRunNow.Name = "btnRunNow";
            this.btnRunNow.Size = new System.Drawing.Size(199, 37);
            this.btnRunNow.TabIndex = 4;
            this.btnRunNow.Text = "Run Now";
            this.btnRunNow.UseVisualStyleBackColor = true;
            this.btnRunNow.Click += new System.EventHandler(this.btnRunNow_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(26, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(164, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Run Only For Specified Client";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cbCompany
            // 
            this.cbCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(23, 42);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(237, 21);
            this.cbCompany.TabIndex = 6;
            // 
            // lstErrorLog
            // 
            this.lstErrorLog.BackColor = System.Drawing.SystemColors.Control;
            this.lstErrorLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstErrorLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstErrorLog.FormattingEnabled = true;
            this.lstErrorLog.Location = new System.Drawing.Point(3, 16);
            this.lstErrorLog.MultiColumn = true;
            this.lstErrorLog.Name = "lstErrorLog";
            this.lstErrorLog.Size = new System.Drawing.Size(542, 147);
            this.lstErrorLog.TabIndex = 9;
            // 
            // txtToEmail
            // 
            this.txtToEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToEmail.Location = new System.Drawing.Point(14, 42);
            this.txtToEmail.Name = "txtToEmail";
            this.txtToEmail.Size = new System.Drawing.Size(244, 20);
            this.txtToEmail.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btBrowse2);
            this.groupBox1.Controls.Add(this.btnBrowse1);
            this.groupBox1.Controls.Add(this.txtFtpLocation);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtLoanFileLocation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 80);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FTP Directories";
            // 
            // btBrowse2
            // 
            this.btBrowse2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBrowse2.Location = new System.Drawing.Point(430, 45);
            this.btBrowse2.Name = "btBrowse2";
            this.btBrowse2.Size = new System.Drawing.Size(102, 23);
            this.btBrowse2.TabIndex = 5;
            this.btBrowse2.Text = "Browse";
            this.btBrowse2.UseVisualStyleBackColor = true;
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse1.Location = new System.Drawing.Point(430, 19);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(102, 23);
            this.btnBrowse1.TabIndex = 4;
            this.btnBrowse1.Text = "Browse";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtToEmail);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 76);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notifications";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Send Notifications";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.cbCompany);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(284, 158);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(276, 76);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Clients";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lstErrorLog);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 285);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(548, 166);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Log";
            // 
            // cbFromPeriod
            // 
            this.cbFromPeriod.FormattingEnabled = true;
            this.cbFromPeriod.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days"});
            this.cbFromPeriod.Location = new System.Drawing.Point(121, 13);
            this.cbFromPeriod.Name = "cbFromPeriod";
            this.cbFromPeriod.Size = new System.Drawing.Size(80, 21);
            this.cbFromPeriod.TabIndex = 17;
            this.cbFromPeriod.Text = "Hours";
            this.cbFromPeriod.SelectedIndexChanged += new System.EventHandler(this.CbFromPeriodLeave);
            // 
            // txtRunTime
            // 
            this.txtRunTime.Location = new System.Drawing.Point(88, 13);
            this.txtRunTime.Mask = "00";
            this.txtRunTime.Name = "txtRunTime";
            this.txtRunTime.Size = new System.Drawing.Size(27, 20);
            this.txtRunTime.TabIndex = 18;
            this.txtRunTime.Text = "1";
            this.txtRunTime.TextChanged += new System.EventHandler(this.TxtRunTimeLeave);
            this.txtRunTime.Leave += new System.EventHandler(this.TxtRunTimeLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Run every";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTime.Location = new System.Drawing.Point(281, 13);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(41, 13);
            this.lblCurrentTime.TabIndex = 20;
            this.lblCurrentTime.Text = "label4";
            // 
            // lblLastSucceed
            // 
            this.lblLastSucceed.AutoSize = true;
            this.lblLastSucceed.BackColor = System.Drawing.Color.Transparent;
            this.lblLastSucceed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastSucceed.ForeColor = System.Drawing.Color.Green;
            this.lblLastSucceed.Location = new System.Drawing.Point(281, 36);
            this.lblLastSucceed.Name = "lblLastSucceed";
            this.lblLastSucceed.Size = new System.Drawing.Size(179, 13);
            this.lblLastSucceed.TabIndex = 56;
            this.lblLastSucceed.Text = "Last Successful Run: 8:00 PM";
            // 
            // Start
            // 
            this.Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold);
            this.Start.Location = new System.Drawing.Point(15, 240);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(151, 37);
            this.Start.TabIndex = 58;
            this.Start.Text = "Start Utility";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // AutoTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(566, 518);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.lblLastSucceed);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRunTime);
            this.Controls.Add(this.cbFromPeriod);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnRunNow);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "AutoTransfer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Automatic FLUW  FTP Transfer Program";
            this.Load += new System.EventHandler(this.AutoTransfer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoanFileLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFtpLocation;
        private System.Windows.Forms.Button btnRunNow;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.ListBox lstErrorLog;
        private System.Windows.Forms.TextBox txtToEmail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btBrowse2;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cbFromPeriod;
        private System.Windows.Forms.MaskedTextBox txtRunTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timerInterval;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblLastSucceed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Start;
    }
}

