using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OutLook = Microsoft.Office.Interop.Outlook;

namespace MortgageTransfer
{
    public partial class AutoTransfer : Form
    {
        #region Global Variables

        DateTime lastRun = new DateTime();
        DateTime timeToRun = new DateTime();
        DateTime minlastRun = new DateTime();
        int fileCount = 0;
        int errorCount = 0;
        int archCount = 0;
        int companyID = 0;
        int orphans = 0;
        bool ranOnce = false;
        string clientName = "";
        string mailMessage = "";
        string mailMessageFileReceived = "";
        string mailMessageFileIn = "";
        string mailFileIn = "";
        string mailFileOut = "";
        bool invalid = false;
        List<FTPLog> logCheck = new List<FTPLog>();
        List<LoanDetail> clientLoans = new List<LoanDetail>();
        List<FTPLog> log = new List<FTPLog>();
        List<FTPSubDirectory> checkSubs = new List<FTPSubDirectory>();
        string baseName = System.Environment.UserName;
        DateTime timeStart = new DateTime();
        List<ComboBoxFill> directoryList = new List<ComboBoxFill>();
        List<EmailList> emails = new List<EmailList>();
        List<EmailList> archEmails = new List<EmailList>();
        List<ComboBoxFill> rootFiles = new List<ComboBoxFill>();
        string archDataDD = Properties.Settings.Default.UploadDirectoryDD;
        string sftp = Properties.Settings.Default.SFTPDirectory;
        string ftplocation = Properties.Settings.Default.DownloadDirectory;
        string moveXmlFile = Properties.Settings.Default.MoveXmlFile;
        public static int MortgageSweeper = 2;

        //added by abhilasha
        string ILMFilePath = Properties.Settings.Default.ILMFilePath;

        int fileCountIn = 0;
        int fileCountOut = 0;
        #endregion
        #region Form Events

        public AutoTransfer()
        {
            InitializeComponent();
        }

        private void AutoTransfer_Load(object sender, EventArgs e)
        {

            txtFrom.Text = MortgageTransfer.Properties.Settings.Default.DownloadDirectory;
            txtTo.Text = MortgageTransfer.Properties.Settings.Default.UploadDirectory;
            txtEmail.Text = MortgageTransfer.Properties.Settings.Default.EmailTo;
            txtNAS.Text = MortgageTransfer.Properties.Settings.Default.NASDirectory;
            txtBackup.Text = MortgageTransfer.Properties.Settings.Default.BackupDirectory;
            lastRun = DateTime.Now;

            TSGDataContext db = new TSGDataContext();
            db.ObjectTrackingEnabled = false;
            cbCompany.DisplayMember = "DisplayValue";
            cbCompany.ValueMember = "ValueID";
            cbCompany.DataSource = db.AllActiveCompanies();

            FTPTransferLogin cl = db.CurrentLogin();
            this.Text += " version " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

            if (cl != null) lastRun = Convert.ToDateTime(cl.LastMortgageRun);
            CompanyAceMapping cCheck = db.CompanyAceMappings.Where(x => x.LastMortgageRun != null).OrderByDescending(x => x.LastMortgageRun).First();

            lblLastSucceed.Text = "Last Successful Run: " + cCheck.LastMortgageRun.Value.ToShortDateString() + " " + cCheck.LastMortgageRun.Value.ToShortTimeString();
            lblLastSucceed.ForeColor = System.Drawing.Color.Green;

            SetTimeToRun();

            timeCheck.Tick += new EventHandler(TimerTick); // Everytime timer ticks, timer_Tick will be called
            timeCheck.Interval = (1000) * (60);              // Timer will tick every minute
            timeCheck.Enabled = true;                       // Enable the timer
            timeCheck.Start();
            TimerTick(sender, e);
        }

        private void AutoTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            TSGDataContext db = new TSGDataContext();
            FTPTransferLogin cl = db.CurrentLogin();

            if (cl != null)
            {
                cl.Insystem = false;
            }
            db.SubmitChanges();

        }
        #endregion
        #region General Controls
        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dir = "";
            string clientNamePath = "";

            using (TSGDataContext db2 = new TSGDataContext())
            {
                FTPDirectory ftp = db2.FTPDirectories.Where(x => x.CompanyID == Convert.ToInt32(cbCompany.SelectedValue)).FirstOrDefault();
                if (ftp != null) dir = ftp.DirectoryName.Trim();

                //added by javed by showing client specific path
                if (Convert.ToInt32(cbCompany.SelectedValue) != 0)
                {
                    CompanyFolder compFolder = db2.CompanyFolders.Where(x => x.CompanyID == Convert.ToInt32(cbCompany.SelectedValue) && x.ParentID == null).FirstOrDefault();
                    if (compFolder != null) clientNamePath = compFolder.FolderPath;
                    else clientNamePath = cbCompany.Text;
                }

            }
            if (cbCompany.SelectedIndex <= 0)
            {
                chkSpecificFolder.Enabled = false;
                btnSpecific.Enabled = false;
                chkSFTP.Enabled = false;
                btnSFTP.Enabled = false;
                return;
            }
            chkSpecificFolder.Enabled = true;
            btnSpecific.Enabled = true;
            if (dir != "")
            {
                chkSFTP.Enabled = true;
                btnSFTP.Enabled = true;
            }
            if (Directory.Exists(txtFrom.Text + "\\" + clientNamePath) == false)
            {
                chkSpecificFolder.Enabled = false;
                btnSpecific.Enabled = false;
                //return;
            }
            if (!Directory.Exists(dir))
            {
                chkSFTP.Enabled = false;
                btnSFTP.Enabled = false;
            }
            //txtSpecificFolder.Text = txtFrom.Text + "\\" + cbCompany.Text;
            txtSpecificFolder.Text = txtFrom.Text + "\\" + clientNamePath; //added by javed for showing clientfolder path
            txtSFTP.Text = dir;
        }
        private void TxtRunTimeLeave(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtRunTime.Text) < 0)
                txtRunTime.Text = "1";
            SetTimeToRun();
        }

        private void CbFromPeriodLeave(object sender, EventArgs e)
        {
            SetTimeToRun();
        }
        private void TxtEmailLeave(object sender, EventArgs e)
        {
            invalid = false;

            if (String.IsNullOrEmpty(txtEmail.Text))
                invalid = true;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                string strIn = Regex.Replace(txtEmail.Text, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);
            }
            catch
            {
                invalid = true;
            }

            // Return true if strIn is in valid e-mail format. 
            try
            {
                bool test = Regex.IsMatch(txtEmail.Text,
                                                        @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                invalid = true;
            }

            if (invalid == true)
            {
                txtEmail.Text = MortgageTransfer.Properties.Settings.Default.EmailTo;
                MessageBox.Show("Email is not in the correct format", "Error");
            }
            //else
            //    MortgageTransfer.Properties.Settings.Default.EmailTo = txtEmail.Text;
        }
        private void BtnFromClick(System.Object sender, System.EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog myFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            // Description that displays above the dialog box control. 
            myFolderBrowser.ShowNewFolderButton = false;
            myFolderBrowser.Description = "Select the Folder";

            // Sets the root folder where the browsing starts from 
            System.Windows.Forms.Button btnSender = (System.Windows.Forms.Button)sender;
            switch (btnSender.Name)
            {
                case "btnFrom":
                    myFolderBrowser.SelectedPath = txtFrom.Text;
                    break;
                case "btnTo":
                    myFolderBrowser.SelectedPath = txtTo.Text;
                    break;
                case "btnNAS":
                    myFolderBrowser.SelectedPath = txtNAS.Text;
                    break;
                case "btnBackup":
                    myFolderBrowser.SelectedPath = txtBackup.Text;
                    break;
                case "btnSpecific":
                    myFolderBrowser.SelectedPath = txtSpecificFolder.Text;
                    break;
                case "btnSFTP":
                    myFolderBrowser.SelectedPath = txtSFTP.Text;
                    break;
            }

            DialogResult dlgResult = myFolderBrowser.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                switch (btnSender.Name)
                {
                    case "btnFrom":
                        txtFrom.Text = myFolderBrowser.SelectedPath;
                        //    MortgageTransfer.Properties.Settings.Default.DownloadDirectory = txtFrom.Text;
                        break;
                    case "btnTo":
                        txtTo.Text = myFolderBrowser.SelectedPath;
                        //    MortgageTransfer.Properties.Settings.Default.UploadDirectory = txtTo.Text;
                        break;
                    case "btnNAS":
                        txtNAS.Text = myFolderBrowser.SelectedPath;
                        //    MortgageTransfer.Properties.Settings.Default.UploadDirectory = txtNAS.Text;
                        break;
                    case "btnBackup":
                        txtBackup.Text = myFolderBrowser.SelectedPath;
                        //    MortgageTransfer.Properties.Settings.Default.UploadDirectory = txtBackup.Text;
                        break;
                    case "btnSpecific":
                        txtSpecificFolder.Text = myFolderBrowser.SelectedPath;
                        break;
                    case "btnSFTP":
                        myFolderBrowser.SelectedPath = txtSFTP.Text;
                        break;
                }
            }
        }

        private void BtnBeginNowClick(object sender, EventArgs e)
        {

            if (chkClient.Checked == true && cbCompany.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a company.", "No Company Selected");
                return;
            }
            if (chkClient.Checked == false)
            {
                if (MessageBox.Show("This will run all companies, and could take several hours.  Continue?", "Running File Sweep", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No) return;
            }
            BeginTransfer();

        }

        #endregion
        #region Timer

        private void SetTimeToRun()
        {
            switch (cbFromPeriod.Text.ToLower())
            {
                case "minutes":
                    timeToRun = DateTime.Now.AddMinutes(Convert.ToInt32(txtRunTime.Text));
                    break;
                case "hours":
                    timeToRun = DateTime.Now.AddHours(Convert.ToInt32(txtRunTime.Text));
                    break;
                case "days":
                    timeToRun = DateTime.Now.AddDays(Convert.ToInt32(txtRunTime.Text));
                    break;
            }
            lblCurrentTime.Text = "Next Run: " + timeToRun.ToShortDateString() + " " + timeToRun.ToShortTimeString();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (txtNowDownloading.Text.Length > 0) return;

            //Write to the database
            TSGDataContext db = new TSGDataContext();
            FTPTransferLogin login = db.CurrentLogin();
            if (login == null) login = new FTPTransferLogin();

            login.Insystem = true;
            login.LastVerified = DateTime.Now;
            login.NextMortgageRun = timeToRun;
            login.PersonLogged = baseName;
            login.CurrentTransfer = false;

            //         if (login.FTPTransferLoginID == 0) db.FTPTransferLogins.InsertOnSubmit(login);
            //           db.SubmitChanges();

            if (DateTime.Now > timeToRun && lblCurrentTime.Text != "Running Files Transfer" && login.CurrentTransfer == false)
            {
                //      login.CurrentTransfer = true;
                //        db.SubmitChanges();
                BeginTransfer();
                if (DateTime.Now.Day == 1 && ranOnce == false)
                {
                    ranOnce = true;
                    timeCheck.Start();
                    DeleteFTP();
                    timeCheck.Start();
                }
                if (DateTime.Now.Day == 2) ranOnce = false;
                //      login.CurrentTransfer = false;

            }
            else
            {
                login.CurrentTransfer = false;
                db.SubmitChanges();
            }


        }

        #endregion

        #region Events
        private void ExtractZip(string from, string to, Loanfile lf, long length)
        {
            string finalLoc = to.Substring(0, to.LastIndexOf("\\")) + "\\";
            try
            {
                string chkFileOnDest = to.Substring(0, to.IndexOf(".zip"));
                if (!File.Exists(chkFileOnDest))
                {
                    // ZipArchiveHelper.ExtractToDirectory(from, finalLoc, true);
                    if (!Directory.Exists(finalLoc))
                    {
                        Directory.CreateDirectory(finalLoc);
                    }
                    ZipFile.ExtractToDirectory(from, finalLoc);
                    File.Delete(from);
                }
                File.Delete(from);
            }
            catch (Exception ex)
            {
                if (!(ex.Message.Contains("already exists")))
                    File.Copy(from, to, true);
            }

            if (lf != null)
            {
                TSGDataContext db2 = new TSGDataContext();
                var txtFiles = new DirectoryInfo(finalLoc).GetFiles("*.*", SearchOption.TopDirectoryOnly).AsEnumerable();

                foreach (FileInfo fileInfo in txtFiles.ToList())
                {
                    if (fileInfo.Extension == ".pdf")
                    {
                        bool lfCheck = db2.LoanFileDownExists(lf.LoanfileID, fileInfo.Name);
                        if (lfCheck == true) continue;

                        LoanFileDownload lfd = new LoanFileDownload();
                        FileInfo fl = new FileInfo(to);

                        lfd.LoanFileID = lf.LoanfileID;
                        lfd.FileName = fileInfo.Name;
                        lfd.FileReceived = DateTime.Now;
                        lfd.FileSize = length;
                        if (!db2.LoanFileDownExists(lf.LoanfileID, fl.Name)) db2.LoanFileDownloads.InsertOnSubmit(lfd);

                        db2.SubmitChanges();
                    }
                }
            }
        }


        private void BeginTransfer()
        {
            timeCheck.Stop();

            Impersonator imp = new Impersonator(MortgageTransfer.Properties.Settings.Default.ImpUser, MortgageTransfer.Properties.Settings.Default.Domain, MortgageTransfer.Properties.Settings.Default.Password);
            timeStart = DateTime.Now;
            using (imp)
            {
                // Get The File List    
                if (!Directory.Exists(txtFrom.Text))
                    Directory.CreateDirectory(txtFrom.Text);
                if (!Directory.Exists(txtTo.Text))
                    Directory.CreateDirectory(txtTo.Text);
                if (!Directory.Exists(txtNAS.Text))
                    Directory.CreateDirectory(txtNAS.Text);

                // set the default save folder

                //Get all files in the various client directories
                lblCurrentTime.Text = "Running Files Transfer";
                lblDone.Visible = false;

                fileCount = 0;
                errorCount = 0;
                lblErrors.Text = "";
                lstErrorFiles.Items.Clear();
                btnBeginNow.Visible = false;
                this.Refresh();

                mailMessage = "";
                mailMessageFileReceived = "";
                mailMessageFileIn = "";
                mailFileIn = "";
                mailFileOut = "";

                TSGDataContext db = new TSGDataContext();
                logCheck = new List<FTPLog>();
                clientLoans = new List<LoanDetail>();
                log = new List<FTPLog>();

                //try
                //{
                db = new TSGDataContext();
                checkSubs = db.FTPSubDirectories.ToList();
                int clientCount = 0;
                List<Company> compList = new List<Company>();
                if (chkClient.Checked == true && Convert.ToInt32(cbCompany.SelectedValue) != 0) compList = db.Companies.Where(x => x.CompanyID == Convert.ToInt32(cbCompany.SelectedValue)).ToList();
                else compList = db.CompList();

                IEnumerable<string> clientFiles = Directory.EnumerateDirectories(txtFrom.Text, "*.*", SearchOption.TopDirectoryOnly);

                List<string> clientFilesList = clientFiles.ToList();
                clientFilesList.Sort();

                foreach (Company comp in compList)
                {

                    //if (comp.Name != "Allen Tate Mortgage") continue;
                    companyID = comp.CompanyID;

                    CompanyFolder compFolder = db.CompanyFolders.Where(x => x.CompanyID == companyID && x.ParentID == null).FirstOrDefault();

                    if (compFolder != null) clientName = compFolder.FolderPath;
                    else clientName = comp.Name;

                    TSGDataContext db2 = new TSGDataContext();

                    clientLoans = ClientNas(comp.Name);


                    bool addedClient = false;

                    CompanyAceMapping cAce = db2.CompanyAceMappings.Where(x => x.CompanyID == companyID).FirstOrDefault();
                    if (cAce != null)
                    {
                        if (cAce.LastMortgageRun != null)
                        {
                            lastRun = Convert.ToDateTime(cAce.LastMortgageRun);
                            minlastRun = Convert.ToDateTime(cAce.LastMortgageRun);
                        }
                        //if (cAce.HasOrphans == true)
                        //{

                        //}
                    }
                    //Takes care of the Field Review sweep.
                    SweepAppraisalOrders(comp, imp);

                    if (chkSpecificFolder.Checked == true)
                    {
                        orphans = 0;
                        archCount = 0;
                        clientCount = 0;
                        fileCountIn = 0;
                        fileCountOut = 0;
                        ComboBoxFill addVal = new ComboBoxFill();
                        addVal.BitField = true;
                        addVal.DisplayValue = txtSpecificFolder.Text.TrimEnd('\\') + "\\";
                        addVal.ValueID = 1;

                        clientCount += DirectorySearch(addVal, ref imp, comp, txtSpecificFolder.Text, comp.Name, 1);
                        while (directoryList.Count > 0)
                        {
                            clientCount += DirectorySearch(directoryList[0], ref imp, comp, txtSpecificFolder.Text, comp.Name, 0);
                            directoryList.RemoveAt(0);
                        }
                        //Added by javed for sending mail for In folder
                        if (mailMessageFileIn.Length == 0)
                        {
                            mailMessageFileIn = "<p><b>The following client files have been received</b></p>" +
                            "<table border='0' style=\"table-layout:fixed;\">" +
                                "<tr><td width=\"60%\"><b>Directory Searched</b></td> <td><b># Files Received</b></td><td td width=\"20%\"><b># ARCH Files</b></td></tr>";
                            if (fileCountIn > 0 || archCount > 0)
                            {
                                mailMessageFileIn += "<tr ><td width =\"50%\">" + clientName + "</td> <td>" + fileCountIn + "</td><td>" + archCount + "</td></tr>";
                            }
                            mailMessageFileIn += mailFileIn;
                        }

                        //added by javed for moving file for mortgage out to destination folder.
                        var ilmoutpath = db.SweeperFolders.Where(x => x.isActive == true && x.SweeperType == 2 && x.MortgageTransfer == true && x.ILMOutPath != null).Select(x => x.ILMOutPath).Distinct().ToList();
                        string ilmout = string.Empty;

                        foreach (var item in ilmoutpath)
                        {
                            string finalDirectoryOut = string
                                .Empty;
                            ComboBoxFill addVal1 = new ComboBoxFill();
                            addVal1.BitField = true;
                            addVal1.DisplayValue = txtSpecificFolder.Text.TrimEnd('\\') + "\\";
                            addVal1.ValueID = 1;
                            string ftpFlocation = string.Concat(ftplocation, "\\");
                            if (addVal1.DisplayValue.IndexOf(ftpFlocation, StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                addVal1.DisplayValue = addVal1.DisplayValue.Replace(ftplocation.ToUpper(), ILMFilePath);
                            }
                            else
                            {
                                addVal1.DisplayValue = addVal1.DisplayValue.Replace(sftp, ILMFilePath);
                            }
                            clientCount += DirectorySearch(addVal1, ref imp, comp, txtSpecificFolder.Text + "", comp.Name, 1);
                            while (directoryList.Count > 0)
                            {
                                clientCount += DirectorySearch(directoryList[0], ref imp, comp, txtSpecificFolder.Text, comp.Name, 0);
                                directoryList.RemoveAt(0);
                            }
                        }

                        //Added by javed for sending mail for all file from Out
                        if (mailMessageFileReceived.Length == 0)
                        {
                            mailMessageFileReceived = "<p><b>The following client files have been received</b></p>" +
                            "<table border='0' style=\"table-layout:fixed;\">" +
                                "<tr><td width=\"60%\"><b>Directory Searched</b></td> <td><b># Files Received</b></td><td width=\"20%\"><b># Orphan Files</b></td></tr>";
                            if (fileCountOut > 0)
                            {
                                mailMessageFileReceived += "<tr ><td width =\"50%\">" + clientName + "</td> <td>" + fileCountOut + "</td><td>" + orphans + "</td></tr>";
                            }
                            mailMessageFileReceived += mailFileOut;
                        }


                        if (clientCount > 0 || archCount > 0)
                        {
                            if (mailMessage.Length == 0)
                            {
                                mailMessage = "<p><b>The following client files have been received</b></p><table border='0' style=\"table-layout:fixed;\"><tr><td width=\"60%\">Directory Searched</td>"
                                    + "<td><b># Files Received</b></td><td width=\"20%\"><b># Orphan Files</b></td><td td width=\"20%\"><b># ARCH Files</b></td>";
                            }
                            if (addedClient == false)
                            {
                                mailMessage += "<tr><td colspan = \"2\"><b>" + comp.Name + "</b></td></tr>";
                                addedClient = true;
                            }
                            mailMessage += "<tr><td width=\"50%\">" + txtSpecificFolder.Text + "</td><td>" + clientCount.ToString() + "</td><td>" + orphans.ToString() + "</td><td>" + archCount.ToString() + "</tr>";
                            foreach (EmailList el in emails) mailMessage += "<tr><td colspan=\"3\">&nbsp;&nbsp;<i>" + el.AuditDesc + "</i> - " + el.FileCount.ToString() + "</td></tr>";
                            foreach (ComboBoxFill cf in rootFiles)
                            {
                                mailMessage += "<tr><td style=\"color:green\" colspan=\"3\"><i>&nbsp;&nbsp;Arch Folder \"" + cf.DisplayValue + "\"</i> - " + cf.ValueID.ToString() + "</td></tr>";
                            }
                            emails.Clear();
                            rootFiles.Clear();
                        }
                    }
                    else if (chkSFTP.Checked == true)
                    {
                        orphans = 0;
                        archCount = 0;
                        clientCount = 0;
                        fileCountIn = 0;
                        fileCountOut = 0;
                        ComboBoxFill addVal = new ComboBoxFill();
                        addVal.BitField = true;
                        addVal.DisplayValue = txtSFTP.Text.TrimEnd('\\') + "\\";
                        addVal.ValueID = 1;
                        txtFrom.Text = txtSFTP.Text.Replace("Incoming", "").TrimEnd('\\') + "\\";
                        clientCount += DirectorySearch(addVal, ref imp, comp, txtSFTP.Text, comp.Name, 1);
                        while (directoryList.Count > 0)
                        {
                            clientCount += DirectorySearch(directoryList[0], ref imp, comp, null, comp.Name, 0);
                            directoryList.RemoveAt(0);
                        }
                        //Added by javed for sending mail for In folder
                        if (mailMessageFileIn.Length == 0)
                        {
                            mailMessageFileIn = "<p><b>The following client files have been received</b></p>" +
                            "<table border='0' style=\"table-layout:fixed;\">" +
                                "<tr><td width=\"60%\"><b>Directory Searched</b></td> <td><b># Files Received</b></td><td td width=\"20%\"><b># ARCH Files</b></td></tr>";
                            if (fileCountIn > 0 || archCount > 0)
                            {
                                mailMessageFileIn += "<tr ><td width =\"50%\">" + clientName + "</td> <td>" + fileCountIn + "</td><td>" + archCount + "</td></tr>";
                            }
                            mailMessageFileIn += mailFileIn;
                        }

                        //added by javed for moving file for mortgage out to destination folder.
                        var ilmoutpath = db.SweeperFolders.Where(x => x.isActive == true && x.SweeperType == 2 && x.MortgageTransfer == true && x.ILMOutPath != null).Select(x => x.ILMOutPath).Distinct().ToList();
                        string ilmout = string.Empty;

                        foreach (var item in ilmoutpath)
                        {
                            string finalDirectoryOut = string
                                .Empty;
                            ComboBoxFill addVal1 = new ComboBoxFill();
                            addVal1.BitField = true;
                            addVal1.DisplayValue = txtSFTP.Text.TrimEnd('\\') + "\\";
                            addVal1.ValueID = 1;
                            txtFrom.Text = txtSFTP.Text.Replace("Incoming", "").TrimEnd('\\') + "\\";
                            ilmout = finalDirectoryOut;
                            string ftpFlocation = string.Concat(ftplocation, "\\");
                        if (addVal.DisplayValue.IndexOf(ftpFlocation, StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            addVal.DisplayValue = addVal.DisplayValue.Replace(ftplocation.ToUpper(), ILMFilePath);
                        }
                        else
                        {
                            addVal.DisplayValue = addVal.DisplayValue.Replace(sftp, ILMFilePath);
                        }
                            clientCount += DirectorySearch(addVal1, ref imp, comp, txtSpecificFolder.Text + "", comp.Name, 1);
                            while (directoryList.Count > 0)
                            {
                                clientCount += DirectorySearch(directoryList[0], ref imp, comp, txtSpecificFolder.Text, comp.Name, 0);
                                directoryList.RemoveAt(0);
                            }

                        }
                        //Added by javed for sending mail for all file from Out
                        if (mailMessageFileReceived.Length == 0)
                        {
                            mailMessageFileReceived = "<p><b>The following client files have been received</b></p>" +
                            "<table border='0' style=\"table-layout:fixed;\">" +
                                "<tr><td width=\"60%\"><b>Directory Searched</b></td> <td><b># Files Received</b></td><td width=\"20%\"><b># Orphan Files</b></td></tr>";
                            if (fileCountOut > 0)
                            {
                                mailMessageFileReceived += "<tr ><td width =\"50%\">" + clientName + "</td> <td>" + fileCountOut + "</td><td>" + orphans + "</td></tr>";
                            }
                            mailMessageFileReceived += mailFileOut;
                        }

                        if (clientCount > 0 || archCount > 0)
                        {
                            if (mailMessage.Length == 0)
                            {
                                mailMessage = "<p><b>The following client files have been received</b></p><table border='0' style=\"table-layout:fixed;\"><tr><td width=\"60%\">Directory Searched</td>"
                                    + "<td><b># Files Received</b></td><td width=\"20%\"><b># Orphan Files</b></td><td td width=\"20%\"><b># ARCH Files</b></td>";
                            }
                            if (addedClient == false)
                            {
                                mailMessage += "<tr><td colspan = \"2\"><b>" + comp.Name + "</b></td></tr>";
                                addedClient = true;
                            }
                            mailMessage += "<tr><td width=\"50%\">" + txtSpecificFolder.Text + "</td><td>" + clientCount.ToString() + "</td><td>" + orphans.ToString() + "</td><td>" + archCount.ToString() + "</tr>";
                            foreach (EmailList el in emails) mailMessage += "<tr><td colspan=\"3\">&nbsp;&nbsp;<i>" + el.AuditDesc + "</i> - " + el.FileCount.ToString() + "</td></tr>";
                            foreach (ComboBoxFill cf in rootFiles)
                            {
                                mailMessage += "<tr><td style=\"color:green\" colspan=\"3\"><i>&nbsp;&nbsp;Arch Folder \"" + cf.DisplayValue + "\"</i> - " + cf.ValueID.ToString() + "</td></tr>";
                            }
                            //if (cAce != null)
                            //{
                            //    db2 = new TSGDataContext();
                            //    CompanyAceMapping compAce = db2.CompanyAceMappings.Where(x => x.CompanyID == companyID).FirstOrDefault();
                            //    if (orphans > 0 && compAce != null) compAce.HasOrphans = true;
                            //    if (compAce != null) compAce.LastRun = timeStart;
                            //    db2.SubmitChanges();
                            //}
                            emails.Clear();
                            rootFiles.Clear();
                        }
                    }
                    else
                        foreach (FTPDirectory useDir in db.FTPDirectories)
                        {
                            int useOverride = 0;
                            if (useDir.CompanyID != null)
                            {
                                if (useDir.CompanyID != comp.CompanyID) continue;
                                useOverride = 1;
                            }

                            ComboBoxFill addVal = new ComboBoxFill();
                            orphans = 0;
                            archCount = 0;
                            clientCount = 0;
                            fileCountIn = 0;
                            fileCountOut = 0;
                            string finalDirectory = useDir.DirectoryName.TrimEnd('\\') + "\\";
                            addVal.BitField = useDir.HasClientFolders;
                            if (useDir.HasClientFolders == true)
                                finalDirectory += clientName + "\\";
                            if (useDir.HasClientFolders == true) addVal.ValueID = 2;
                            else addVal.ValueID = 1;

                            addVal.DisplayValue = finalDirectory;
                            txtFrom.Text = useDir.DirectoryName;


                            clientCount += DirectorySearch(addVal, ref imp, comp, useDir.NewFolder + "", comp.Name, useOverride);

                            string ftpFlocation = string.Concat(ftplocation, "\\");
                            while (directoryList.Count > 0)
                            {
                                clientCount += DirectorySearch(directoryList[0], ref imp, comp, useDir.NewFolder, comp.Name, useOverride);
                                directoryList.RemoveAt(0);
                            }

                            //Added by javed for sending mail for In folder
                            if (mailMessageFileIn.Length == 0)
                            {
                                mailMessageFileIn = "<p><b>The following client files have been received</b></p>" +
                                "<table border='0' style=\"table-layout:fixed;\">" +
                                    "<tr><td width=\"60%\"><b>Directory Searched</b></td> <td><b># Files Received</b></td><td td width=\"20%\"><b># ARCH Files</b></td></tr>";
                                if (fileCountIn > 0 || archCount > 0)
                                {
                                    mailMessageFileIn += "<tr ><td width =\"50%\">" + clientName + "</td> <td>" + fileCountIn + "</td><td>" + archCount + "</td></tr>";
                                }
                                mailMessageFileIn += mailFileIn;
                            }

                            //added by javed for moving file for mortgage out to destination folder.

                            var ilmoutpath = db.SweeperFolders.Where(x => x.isActive == true && x.SweeperType == 2 && x.MortgageTransfer == true && x.ILMOutPath != null).Select(x => x.ILMOutPath).Distinct().ToList();
                            string ilmout = string.Empty;

                            foreach (var item in ilmoutpath)
                            {
                                string finalDirectoryOut = string
                                    .Empty;
                                ComboBoxFill addVa1 = new ComboBoxFill();
                                addVa1.BitField = useDir.HasClientFolders;
                                if (useDir.HasClientFolders == true)
                                    finalDirectoryOut = item + clientName + "\\";
                                if (useDir.HasClientFolders == true) addVa1.ValueID = 2;
                                else addVa1.ValueID = 1;
                                addVa1.DisplayValue = finalDirectoryOut;
                                ilmout = finalDirectoryOut;
                                if (addVal.DisplayValue.IndexOf(ftpFlocation, StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    addVal.DisplayValue = addVal.DisplayValue.Replace(ftplocation.ToUpper() + "\\", ILMFilePath);
                                }
                                else
                                {
                                    addVal.DisplayValue = addVal.DisplayValue.Replace(sftp, ILMFilePath);
                                }
                                clientCount += DirectorySearch(addVa1, ref imp, comp, useDir.NewFolder + "", comp.Name, useOverride);
                                while (directoryList.Count > 0)
                                {
                                    clientCount += DirectorySearch(directoryList[0], ref imp, comp, useDir.NewFolder, comp.Name, useOverride);
                                    directoryList.RemoveAt(0);
                                }

                            }
                            //Added by javed for sending mail for all file from Out
                            if (mailMessageFileReceived.Length == 0)
                            {
                                mailMessageFileReceived = "<p><b>The following client files have been received</b></p>" +
                                "<table border='0' style=\"table-layout:fixed;\">" +
                                    "<tr><td width=\"60%\"><b>Directory Searched</b></td> <td><b># Files Received</b></td><td width=\"20%\"><b># Orphan Files</b></td></tr>";
                                if (fileCountOut > 0)
                                {
                                    mailMessageFileReceived += "<tr ><td width =\"50%\">" + clientName + "</td> <td>" + fileCountOut + "</td><td>" + orphans + "</td></tr>";
                                }
                                mailMessageFileReceived += mailFileOut;
                            }

                            if (useDir.DirectoryName.ToLower().IndexOf("\\old republic\\incoming") != -1 && (DateTime.Now.Hour == 0 || DateTime.Now.Hour == 1)) DeleteOldRepublic(useDir.DirectoryName, imp);//

                            if (clientCount > 0 || archCount > 0)
                            {
                                if (mailMessage.Length == 0)
                                {
                                    mailMessage = "<p><b>The following client files have been received</b></p><table border='0' style=\"table-layout:fixed;\"><tr><td width=\"60%\">Directory Searched</td>"
                                        + "<td><b># Files Received</b></td><td width=\"20%\"><b># Orphan Files</b></td><td td width=\"20%\"><b># ARCH Files</b></td>";
                                }
                                if (addedClient == false)
                                {
                                    mailMessage += "<tr><td colspan = \"2\"><b>" + comp.Name + "</b></td></tr>";
                                    addedClient = true;
                                }
                                mailMessage += "<tr><td width=\"50%\">" + useDir.DisplayName + "</td><td>" + clientCount.ToString() + "</td><td>" + orphans.ToString() + "</td><td>" + archCount.ToString() + "</tr>";
                                foreach (EmailList el in emails) mailMessage += "<tr><td colspan=\"3\">&nbsp;&nbsp;<i>" + el.AuditDesc + "</i> - " + el.FileCount.ToString() + "</td></tr>";
                                foreach (ComboBoxFill cf in rootFiles)
                                {
                                    mailMessage += "<tr><td style=\"color:green\" colspan=\"3\"><i>&nbsp;&nbsp;Arch Folder \"" + cf.DisplayValue + "\"</i> - " + cf.ValueID.ToString() + "</td></tr>";
                                }

                                emails.Clear();
                                rootFiles.Clear();
                            }

                        }

                    //added by javed for deleting all empty folder. for loan File and data folder (Quality control & Due diligenece) file.
                    string dLoanFolder = string.Concat(ftplocation, "\\", comp.Name, @"\Due Diligence\Loan Files");
                    DirectoryInfo dIdLoanFolder = new DirectoryInfo(string.Concat(ftplocation, "\\", comp.Name, @"\Due Diligence\Loan Files"));
                    string dDataFolder = string.Concat(ftplocation, "\\", comp.Name, @"\Due Diligence\Data\");
                    DirectoryInfo dIdDataFolder = new DirectoryInfo(string.Concat(ftplocation, "\\", comp.Name, @"\Due Diligence\Data\"));
                    string QLoanFolder = string.Concat(ftplocation, "\\", comp.Name, @"\Quality Control\Loan Files");
                    DirectoryInfo dIQLoanFolder = new DirectoryInfo(string.Concat(ftplocation, "\\", comp.Name, @"\Quality Control\Loan Files"));
                    string QDataFolder = string.Concat(ftplocation, "\\", comp.Name, @"\Quality Control\Data\");
                    DirectoryInfo dIQDataFolder = new DirectoryInfo(string.Concat(ftplocation, "\\", comp.Name, @"\Quality Control\Data\"));


                    if (Directory.Exists(dLoanFolder))
                    {
                        string[] dLoanFile = Directory.GetFiles(dLoanFolder, "*.*", SearchOption.AllDirectories);
                        if (dLoanFile.Length == 0)
                        {
                            foreach (DirectoryInfo subDirectory in dIdLoanFolder.EnumerateDirectories())
                            {
                                subDirectory.Delete(true);
                            }
                        }
                    }
                    if (Directory.Exists(dDataFolder))
                    {
                        string[] dDataFile = Directory.GetFiles(dDataFolder, "*.*", SearchOption.AllDirectories);
                        if (dDataFile.Length == 0)
                        {
                            foreach (DirectoryInfo subDirectory in dIdDataFolder.EnumerateDirectories())
                            {
                                subDirectory.Delete(true);
                            }
                        }
                    }
                    if (Directory.Exists(QLoanFolder))
                    {
                        string[] QLoanFile = Directory.GetFiles(QLoanFolder, "*.*", SearchOption.AllDirectories);
                        if (QLoanFile.Length == 0)
                        {
                            foreach (DirectoryInfo subDirectory in dIQLoanFolder.EnumerateDirectories())
                            {
                                subDirectory.Delete(true);
                            }
                            // Directory.Delete(QLoanFolder, true);
                        }
                    }
                    if (Directory.Exists(QDataFolder))
                    {
                        string[] QDataFile = Directory.GetFiles(QDataFolder, "*.*", SearchOption.AllDirectories);
                        if (QDataFile.Length == 0)
                        {
                            foreach (DirectoryInfo subDirectory in dIQDataFolder.EnumerateDirectories())
                            {
                                subDirectory.Delete(true);
                            }
                        }
                    }



                    db2.FTPLogs.InsertAllOnSubmit(log);
                    if (cAce != null)
                    {
                        db2 = new TSGDataContext();
                        CompanyAceMapping compAce = db2.CompanyAceMappings.Where(x => x.CompanyID == companyID).FirstOrDefault();
                        if (orphans > 0 || checkOrphans())
                        {
                            compAce.HasOrphans = true;

                        }
                        else
                        {
                            compAce.HasOrphans = false;
                        }
                        compAce.LastMortgageRun = timeStart;
                        db2.SubmitChanges();
                    }
                    //Added by Amar Palai Dated on 31 march 2017
                    List<LoanFileAudit> updateList = db2.UpdateCompleteAudits(companyID);
                    foreach (LoanFileAudit useAudit in updateList)
                    {
                        if (useAudit.FileReceivedDate == null)
                        {
                            useAudit.FileReceivedDate = DateTime.Now;
                        }
                        useAudit.FileReceivedDate = DateTime.Now;
                    }
                    db2.SubmitChanges();

                    db.SubmitChanges();

                    log.Clear();

                }

                if (mailMessage.Length > 0) mailMessage += "</table>";

                btnBeginNow.Visible = true;
                lblDone.Visible = true;
                SendMail(MailDetailType.AllDetails);
                SendMail(MailDetailType.InDetails);
                SendMail(MailDetailType.OutDetails);


                FTPTransferLogin login = db.CurrentLogin();
                login.LastMortgageRun = timeStart;
                db.SubmitChanges();
                db = new TSGDataContext();
                if (chkClient.Checked == false) db.UpdateTimes(timeStart, 0);
                else db.UpdateTimes(timeStart, Convert.ToInt32(cbCompany.SelectedValue));
                db.UpdateAuditTypes();
                chkClient.Checked = false;
                chkSpecificFolder.Checked = false;
                cbCompany.SelectedIndex = -1;
                chkSpecificFolder.Enabled = true;
                txtSpecificFolder.Text = "";
                btnSpecific.Enabled = false;
                txtFrom.Text = MortgageTransfer.Properties.Settings.Default.DownloadDirectory;
                txtSFTP.Text = "";
                chkSFTP.Checked = chkSFTP.Enabled = false;
                btnSFTP.Enabled = false;
                txtNowDownloading.Text = "";
                lblLastSucceed.Text = "Last Successful Run: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                lblLastSucceed.ForeColor = System.Drawing.Color.Green;
                SetTimeToRun();
                timeCheck.Start();
                return;

            }
        }
        private bool checkOrphans()
        {
            //check the company's "Orphan" directory to see if anything is actually in there.
            string orphanDirectory = txtNAS.Text + "\\" + clientName + "\\Orphans\\";
            bool hasOrphans = true;

            if (Directory.Exists(orphanDirectory) == false) hasOrphans = false;
            else
            {
                if (Directory.GetFiles(orphanDirectory, "*.*", SearchOption.AllDirectories).Length == 0)
                {
                    DirectoryInfo di = new DirectoryInfo(orphanDirectory);
                    hasOrphans = false;
                    di.Delete(true);
                }
            }
            return hasOrphans;

        }
        private string TransferFile(string extension, string from, string to, MortgageTransfer.Loanfile lf, long length, Impersonator imp)
        {
            using (imp)
            {
                FileInfo use = new FileInfo(to);
                //temp
                //     if (to.IndexOf("arch") == -1) return "";
                switch (extension.ToLower())
                {
                    case ".zip":
                        ExtractZip(from, to, lf, length);
                        string xmlPath = to.Substring(0, to.LastIndexOf("\\")) + "\\";

                        var txtFiles = new DirectoryInfo(xmlPath).GetFiles("*.xml*", SearchOption.TopDirectoryOnly).FirstOrDefault();
                        if (txtFiles != null)
                        {
                            if (!Directory.Exists(moveXmlFile + clientName))
                            {
                                Directory.CreateDirectory(moveXmlFile + clientName);
                            }
                            File.Copy(txtFiles.FullName, moveXmlFile + clientName + "\\" + txtFiles.Name);
                        }
                        break;
                    default:
                        int ctr = 1;

                        string fileCheck = to;
                        FileInfo ft = new FileInfo(to);
                        if (Directory.Exists(ft.Directory.FullName) == false) Directory.CreateDirectory(ft.Directory.FullName);
                        if (File.Exists(to))
                        {

                            FileInfo ff = new FileInfo(from);
                            //If the files are within 5000 bytes of being the same length, we don't need to copy.
                            if ((ft.Length - ff.Length) < 5000)
                            {
                                File.Delete(from);
                                return to.Substring(to.LastIndexOf("\\") + 1);
                            }

                            //Start looking for a unique name.

                            while (File.Exists(fileCheck))
                            {
                                ctr++;
                                fileCheck = use.FullName.Substring(0, use.FullName.Length - use.Extension.Length - 1) + " " + ctr.ToString() + use.Extension;

                            }
                        }
                        to = fileCheck;

                        File.Copy(from, to, true);
                        File.Delete(from);
                        break;

                }
                if (extension.ToLower() == ".zip") return "";
                else return to.Substring(to.LastIndexOf("\\") + 1);
            }
        }
        private int DirectorySearch(ComboBoxFill directoryName, ref Impersonator imp, Company useComp, string newFolder, string companyName, int overrideID = 0)
        {
            int rtn = 0;

            TSGDataContext db = new TSGDataContext();
            using (imp)
            {
                if (Directory.Exists(directoryName.DisplayValue) == false)
                {
                    return 0;
                }
                if (directoryName.DisplayValue.IndexOf("\\2014 Questionnaires and Highlights", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\Checklist", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\Negative Mail", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\QC Credit Reports", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\QC Field Reviews", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\QC Reverifications", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\QC Trail Mail", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                //if (directoryName.DisplayValue.IndexOf("\\Rebuttal", StringComparison.OrdinalIgnoreCase) > -1) return 0;//rebuttal folder now use for backup file.
                if (directoryName.DisplayValue.IndexOf("\\Reports", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\Vendor Review", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                if (directoryName.DisplayValue.IndexOf("\\Images", StringComparison.OrdinalIgnoreCase) > -1) return 0;
                //Check to see if one of the directories' children have their name in this set
                List<CompanyAceMapping> compChildren = db.CompanyAceMappings.Where(x => x.CompanyParentID == useComp.CompanyID).ToList();
                string compareDir = directoryName.DisplayValue.ToLower().Replace("&", "and").Replace(".", "").Replace(",", "");
                foreach (CompanyAceMapping compChild in compChildren)
                {
                    int childID = compChild.CompanyID;
                    Company childCheck = db.Companies.Where(x => x.CompanyID == childID).FirstOrDefault();

                    string childName = childCheck.Name.ToLower();
                    if (compareDir.IndexOf(childName) > -1) return 0;
                }

                IEnumerable<string> txtDirectory = Directory.EnumerateDirectories(directoryName.DisplayValue, "*.*", SearchOption.TopDirectoryOnly);
                txtDirectory.Concat(Directory.EnumerateDirectories(directoryName.DisplayValue, "*.*", SearchOption.TopDirectoryOnly));
                foreach (string dir in txtDirectory.ToList())
                {
                    //avoids a lambda
                    string compare = dir;
                    if (directoryName.BitField == true && chkSFTP.Checked == false && dir.IndexOf("Quality Control") == -1 && dir.IndexOf("Prefund") == -1 && dir.IndexOf("Due Diligence") == -1 && dir.IndexOf("Servicing") == -1) continue;
                    var x1 = db.SweeperFolders.Where(x => x.SweeperType == MortgageSweeper).ToList();
                    if (directoryList.Where(x => x.DisplayValue == compare).FirstOrDefault() == null && ValidateParentFolder(compare) && chkSpecificFolder.Checked == false)
                    {
                        ComboBoxFill addCbf = new ComboBoxFill();
                        addCbf.DisplayValue = dir;
                        addCbf.ValueID = directoryName.ValueID;
                        addCbf.BitField = directoryName.BitField;
                        directoryList.Add(addCbf);

                    }
                    else
                    {
                        if (compare.ToLower().EndsWith("Quality Control\\Rebuttals".ToLower()))
                        {
                            ComboBoxFill addCbf = new ComboBoxFill();
                            addCbf.DisplayValue = dir;
                            addCbf.ValueID = directoryName.ValueID;
                            addCbf.BitField = directoryName.BitField;
                            directoryList.Add(addCbf);
                        }
                        else
                        {
                            for (int i = 0; i <= x1.Count - 1; i++)
                            {
                                if (compare.ToLower().Contains(x1[i].SweeperFolder.ToLower()))
                                {
                                    ComboBoxFill addCbf = new ComboBoxFill();
                                    addCbf.DisplayValue = dir;
                                    addCbf.ValueID = directoryName.ValueID;
                                    addCbf.BitField = directoryName.BitField;
                                    directoryList.Add(addCbf);
                                }
                            }
                        }
                    }
                }

                //We are only sweeping Quality Control for now EXCEPT Mortgage Masters; for that company, I had to override... they can only put files to the root directory.
                if (!directoryName.DisplayValue.Contains("Outbound"))
                {
                    if (directoryName.BitField == true && chkSFTP.Checked == false && (directoryName.DisplayValue.IndexOf("Quality Control") == -1) && (directoryName.DisplayValue.IndexOf("Prefund") == -1) && (directoryName.DisplayValue.IndexOf("Due Diligence") == -1) && (directoryName.DisplayValue.IndexOf("Servicing") == -1)) return 0;
                    if (directoryName.DisplayValue.Split('\\').Last() == "Quality Control" || directoryName.DisplayValue.Split('\\').Last() == "Prefund" || directoryName.DisplayValue.Split('\\').Last() == "Due Diligence" || directoryName.DisplayValue.Split('\\').Last() == "Servicing")
                        return 0;

                }
                var txtFiles = new DirectoryInfo(directoryName.DisplayValue).GetFiles("*.*", SearchOption.TopDirectoryOnly);//.Where(file => file.LastWriteTime.ToLocalTime() >  lastRun && file.LastWriteTime.ToLocalTime() < timeStart);

                CompanyAceMapping ca = db.CompanyAceMappings.Where(x => x.CompanyID == useComp.CompanyID).First();

                if (ValidateMortgageFolder(directoryName.DisplayValue))
                {
                    string mpth;

                    foreach (FileInfo fl in txtFiles.ToList())
                    {
                        //Determine if the file was put up by Stonehill
                        if (fl.Name == "Thumbs.db") continue;

                        if (directoryName.BitField == false && overrideID == 0)
                        {
                            if (ca.CompanyShortName == null) continue;
                            string shortName = ca.CompanyShortName.ToLower();
                            if (fl.Name.ToLower().StartsWith(shortName) == false) continue;
                            List<String> matchList = db.CompanyNamePossibleMatches(shortName);
                            foreach (string match in matchList)
                            {
                                if (fl.Name.ToLower().StartsWith(match.ToLower()) == true) continue;
                            }
                        }

                        //Copy file from client Dashboard/ FTP to ILM in folder added by abhilasha
                        string sourceFilePath = fl.FullName;
                        string iLMSourcePath = fl.DirectoryName;
                        string iLMInPath = string.Empty;
                        string ILMInFile = string.Empty;
                        var iLMInPathList = db.SweeperFolders.Where(x => x.isActive == true && x.SweeperType == 2 && x.MortgageTransfer == true).ToList();
                        foreach (var item in iLMInPathList)
                        {
                            if (iLMSourcePath.Contains(item.SweeperFolder))
                            {
                                iLMInPath = item.ILMInPath;
                                ILMInFile = iLMSourcePath.Replace(txtFrom.Text, iLMInPath);
                                ILMInFile = ILMInFile.Replace(item.SweeperFolder, "");
                                break;
                            }
                        }

                        if (fl.FullName.ToLower().Contains("incoming"))
                        {
                            ILMInFile = string.Concat(iLMInPath, cbCompany.Text, "\\incoming\\", fl);
                            mpth = string.Concat(iLMInPath, cbCompany.Text, "\\incoming\\");
                        }
                        FileInfo sourceFile = new FileInfo(fl.FullName);

                        if (ValidateFileType(Path.GetExtension(fl.Extension)))
                        {

                            if (fl.DirectoryName.ToLower().EndsWith("\\incoming") && fl.DirectoryName.ToLower().IndexOf("\\ftpfiles") != -1)
                            {
                                string sftpFile = sourceFilePath.Replace(sftp, string.Concat(txtBackup.Text, "\\"));
                                string sftpBackupPath = sftpFile.Substring(0, sftpFile.LastIndexOf("\\"));
                                if (!Directory.Exists(sftpBackupPath))
                                    Directory.CreateDirectory(sftpBackupPath);
                                fl.CopyTo(sftpFile, true);
                                //sourceFile.CopyTo(sftpFile,true);
                            }
                            else
                            {
                                string backupFile = string.Empty;
                                string backupPath = string.Empty;
                                if (chkSFTP.Checked)
                                {
                                    backupFile = sourceFilePath.Replace(sftp, string.Concat(txtBackup.Text, "\\"));
                                    backupPath = backupFile.Substring(0, backupFile.LastIndexOf("\\"));
                                }
                                else
                                {
                                    backupFile = sourceFilePath.Replace(txtFrom.Text, string.Concat(txtBackup.Text, "\\"));
                                    backupPath = backupFile.Substring(0, backupFile.LastIndexOf("\\"));
                                }
                                if (!Directory.Exists(backupPath))
                                    Directory.CreateDirectory(backupPath);
                                if (ValidateforRebuttals(directoryName.DisplayValue))
                                {
                                    if (!File.Exists(backupFile))
                                    {
                                        fl.CopyTo(backupFile, true);
                                    }
                                    continue;
                                }
                                else
                                {
                                    if (backupFile.Length < 248)
                                    {
                                        fl.CopyTo(backupFile, true);
                                    }
                                }

                            }
                            if (!File.Exists(ILMInFile))
                            {
                                if (sourceFile.Extension.ToLower() == ".zip")
                                {
                                    string destinationPath = ILMInFile.Substring(0, ILMInFile.IndexOf(".zip"));
                                    ZipFile.ExtractToDirectory(sourceFile.FullName, destinationPath);
                                    mailFileIn += "<tr ><td width =\"50%\">" + destinationPath + "</td> <td> </td>" + "</tr>";
                                    fileCountIn += 1;
                                    File.Delete(sourceFile.FullName);
                                }
                                else
                                {
                                    if (ILMInFile.Length < 248)
                                    {
                                        if (!Directory.Exists(ILMInFile))
                                        {
                                            Directory.CreateDirectory(ILMInFile);
                                        }
                                        ILMInFile = string.Concat(ILMInFile, fl.Name);
                                        if (!File.Exists(ILMInFile))
                                        {
                                            fl.MoveTo(ILMInFile);
                                        }
                                        mailFileIn += "<tr ><td width =\"50%\">" + ILMInFile + "</td> <td> </td>" + "</tr>";
                                        fileCountIn += 1;
                                    }
                                }
                                Loanfile lf = new Loanfile();
                                List<LoanDetail> loanused = clientLoans.Where(x => fl.Name.IndexOf(x.LoanNbr, StringComparison.OrdinalIgnoreCase) > -1 && fl.Name.IndexOf(x.LastName, StringComparison.OrdinalIgnoreCase) > -1).OrderByDescending(x => x.LoanNbr.Length).ToList();
                                if (loanused.Count == 0)
                                {
                                    loanused = clientLoans.Where(x => fl.Name.IndexOf(x.LoanNbr, StringComparison.OrdinalIgnoreCase) > -1).OrderByDescending(x => x.LoanNbr.Length).ToList();
                                    if (loanused.Count == 0)
                                    {
                                        loanused = clientLoans.Where(x => fl.Name.IndexOf(x.LastName, StringComparison.OrdinalIgnoreCase) > -1).OrderByDescending(x => x.LoanNbr.Length).ToList();
                                        if (loanused.Count == 1)
                                            lf = db.Loanfiles.Where(x => x.LoanFileAuditID == loanused[0].LoanFileAuditID && x.LoanNumber.ToLower() == loanused[0].LoanNbr.ToLower()).First();
                                    }
                                    else
                                    {
                                        if (loanused.Count == 1)
                                            lf = db.Loanfiles.Where(x => x.LoanFileAuditID == loanused[0].LoanFileAuditID && x.LoanNumber.ToLower() == loanused[0].LoanNbr.ToLower()).First();
                                    }
                                }
                                else
                                {
                                    if (loanused.Count == 1)
                                        lf = db.Loanfiles.Where(x => x.LoanFileAuditID == loanused[0].LoanFileAuditID && x.LoanNumber.ToLower() == loanused[0].LoanNbr.ToLower()).First();
                                }

                                if (loanused.Count == 1 && (!db.LoanFileDownExists(lf.LoanfileID, fl.Name)))
                                {
                                    LoanFileDownload lfd = new LoanFileDownload();
                                    lfd.LoanFileID = lf.LoanfileID;
                                    lfd.FileName = fl.Name;
                                    lfd.FileReceived = DateTime.Now;
                                    lfd.FileSize = fl.Length;

                                    if (!db.LoanFileDownExists(lf.LoanfileID, fl.Name)) db.LoanFileDownloads.InsertOnSubmit(lfd);
                                    db.SubmitChanges();
                                }

                            }
                        }
                    }

                    //added by javed when we get all file in loanfiledown table add file received date in loanFileAudit table.
                    List<LoanFileAudit> updateList = db.UpdateCompleteAudits(companyID);
                    foreach (LoanFileAudit useAudit in updateList)
                    {
                        if (useAudit.FileReceivedDate == null)
                        {
                            useAudit.FileReceivedDate = DateTime.Now;
                        }
                        useAudit.FileReceivedDate = DateTime.Now;
                    }
                    db.SubmitChanges();
                    return 0;


                }

                foreach (FileInfo fl in txtFiles.ToList())
                {
                    //Determine if the file was put up by Stonehill
                    if (fl.Name == "Thumbs.db") continue;

                    if (directoryName.BitField == false && overrideID == 0)
                    {
                        if (ca.CompanyShortName == null) continue;
                        string shortName = ca.CompanyShortName.ToLower();
                        if (fl.Name.ToLower().StartsWith(shortName) == false) continue;
                        List<String> matchList = db.CompanyNamePossibleMatches(shortName);
                        foreach (string match in matchList)
                        {
                            if (fl.Name.ToLower().StartsWith(match.ToLower()) == true) continue;
                        }
                    }

                    string currentFile = fl.FullName;
                    string compareFile = currentFile.ToLower();
                    string fileName = string.Empty;

                    if (currentFile.IndexOf("Outbound") != -1)
                    {
                        fileName = currentFile.Replace(ILMFilePath, "");
                    }
                    else
                    {
                        fileName = currentFile.Replace(txtFrom.Text, "");
                    }
                    if (fileName.ToLower().IndexOf("\\images") == -1 && fileName.ToLower().IndexOf("\\loan files") == -1)
                    {

                        FTPLog flCheck = db.FTPLogs.Where(x => x.FileTransferTo.Replace("\\\\", "\\").IndexOf(currentFile.Replace("\\\\", "\\")) > -1).FirstOrDefault();
                        if (flCheck != null)
                            break;
                    }
                    txtNowDownloading.Text = "Now Downloading: " + fileName;
                    txtNowDownloading.Refresh();
                    System.Windows.Forms.Application.DoEvents();
                    string finalLoc = string.Empty;
                    string pth = string.Empty;
                    if (ValidateDDDataFolder(fileName))
                    {
                        if (fileName.IndexOf("due diligence\\data", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            finalLoc = Path.Combine(archDataDD, fileName);
                            //finalLoc = string.Concat(archDataDD.Substring(0, archDataDD.LastIndexOf("\\")), fileName);
                            pth = finalLoc.Substring(0, finalLoc.LastIndexOf("\\") + 1);
                        }
                        else
                        {
                            if (fileName.StartsWith("\\"))
                            {
                                fileName = fileName.Substring(1);
                            }
                            finalLoc = Path.Combine(txtTo.Text, fileName);
                            //finalLoc = string.Concat(txtTo.Text.Substring(0, txtTo.Text.LastIndexOf("\\")), fileName);
                            pth = finalLoc.Substring(0, finalLoc.LastIndexOf("\\") + 1);
                        }
                    }

                    bool isPotentialLoan = false;
                    if (directoryName.ValueID == 1 && directoryName.DisplayValue.IndexOf("Homestead") == -1 && ValidateMortgageFolder(directoryName.DisplayValue)) isPotentialLoan = true;
                    else
                    {
                        checkSubs = db.FTPSubDirectories.Where(x => x.Type != 3).ToList();
                        //See if it has at least one set of the "FTP Sub Directories" in it.
                        foreach (FTPSubDirectory useSub in checkSubs)
                        {
                            if (compareFile.ToLower().IndexOf(useSub.FTPSub) > -1)
                            {
                                isPotentialLoan = true;
                                break;
                            }
                        }
                    }
                    if (!chkSpecificFolder.Checked)
                    {
                        if (overrideID != 0)
                            isPotentialLoan = true;
                    }

                    string dirName = fl.DirectoryName.ToLower();
                    bool isFulfillment = (dirName.IndexOf("\\fulfillment") > -1);

                    //We're setting up a series of rules so that we eliminate anything that is not a file potential.

                    List<LoanDetail> loansToUse = new List<LoanDetail>();
                    string flName = fl.Name.ToLower();
                    string zipFileName = fl.Directory.FullName.Substring(fl.Directory.FullName.LastIndexOf("\\"));
                    string sendName = fl.Name.Replace(",", "").Replace(";", "").Replace("`", "");
                    if (isPotentialLoan == true)
                    {
                        bool loanNotFound = false;
                        //We'll check first if both LoanNbr AND Name are in there, then if Loan Number alone is in there, then if Name alone is in there (match in descending priority).1
                        loansToUse = clientLoans.Where(x => flName.IndexOf(x.LoanNbr.Trim(), StringComparison.OrdinalIgnoreCase) > -1 && flName.IndexOf(x.LastName, StringComparison.OrdinalIgnoreCase) > -1 && x.IsFulfillment == isFulfillment).ToList();
                        if (loansToUse.Count == 0)
                        {
                            loansToUse = clientLoans.Where(x => zipFileName.IndexOf(x.LoanNbr.Trim(), StringComparison.OrdinalIgnoreCase) > -1 && zipFileName.IndexOf(x.LastName, StringComparison.OrdinalIgnoreCase) > -1 && x.IsFulfillment == isFulfillment).ToList();
                        }
                        if (loansToUse.Count == 0)
                        {
                            loansToUse = clientLoans.Where(x => flName.IndexOf(x.LoanNbr, StringComparison.OrdinalIgnoreCase) > -1 && x.IsFulfillment == isFulfillment).ToList();
                            if (loansToUse.Count == 0)
                            {
                                loansToUse = clientLoans.Where(x => zipFileName.IndexOf(x.LoanNbr, StringComparison.OrdinalIgnoreCase) > -1 && x.IsFulfillment == isFulfillment).ToList();
                            }

                            if (loansToUse.Count == 0)
                            {
                                loanNotFound = true;
                                //from here on we only want one loan
                                List<LoanDetail> checkList = clientLoans.Where(x => flName.IndexOf(x.LoanNbr, StringComparison.OrdinalIgnoreCase) > -1 && x.IsFulfillment == isFulfillment).OrderByDescending(x => x.LoanNbr.Length).ToList();

                                //Determine if we have multiple loans with the same length that meet the criteria.  If we do, this has to regulate to orphans.
                                if (checkList.Count > 1)
                                {
                                    if (checkList[0].LoanNbr.Length != checkList[1].LoanNbr.Length) loansToUse.Add(checkList[0]);
                                }
                            }
                        }

                        if (loansToUse.Count == 0 && loanNotFound == false)
                        {
                            loansToUse = clientLoans.Where(x => flName.IndexOf(x.LastName, StringComparison.OrdinalIgnoreCase) > -1 && x.IsFulfillment == isFulfillment).OrderByDescending(x => x.LoanFileAuditID).ToList();//Latest audits first
                                                                                                                                                                                                                            //determine if we have multiple people with that last name (Exact Name), if we do then it's going to be an orphan anyway


                            if (loansToUse.Count != 0)
                            {
                                int expectedLength = loansToUse[0].LastName.Length;

                                for (int ctr = loansToUse.Count - 1; ctr > 0; ctr--)
                                {
                                    if (loansToUse[ctr].LastName.Length != expectedLength) loansToUse.RemoveAt(ctr);
                                }

                            }
                            //determine if we have multiple people with that last name (Look a like Name), if we do then it's going to be an orphan anyway
                            if (loansToUse.Count != 0)
                            {
                                int expectedLength = loansToUse[0].LastName.Length;

                                for (int ctr = loansToUse.Count - 1; ctr > 0; ctr--)
                                {
                                    if (loansToUse[ctr].LastName.Length == expectedLength) loansToUse.RemoveAt(ctr);
                                }

                            }
                        }

                        //the last check is to check to see if the FOLDER has the Loan Number in it.  If it does, that can be used
                        if (loansToUse.Count == 0 && loanNotFound == false)
                        {
                            string higherLevel = fl.Directory.Name.ToLower();
                            loansToUse = clientLoans.Where(x => higherLevel.IndexOf(x.LoanNbr, StringComparison.OrdinalIgnoreCase) > -1).ToList();
                        }

                    }

                    Loanfile lf;
                    if (loansToUse.Count != 0)
                    {

                        for (int ctrx = loansToUse.Count - 1; ctrx >= 0; ctrx--)
                        {
                            lf = db.Loanfiles.Where(x => x.LoanFileAuditID == loansToUse[ctrx].LoanFileAuditID && x.LoanNumber.ToLower() == loansToUse[ctrx].LoanNbr.ToLower()).FirstOrDefault();
                            if (lf == null) loansToUse.RemoveAt(ctrx);
                        }
                    }

                    FTPLog lg = new FTPLog();
                    try
                    {
                        if (isPotentialLoan == true)
                        {
                            if (loansToUse.Count != 0)
                            {
                                foreach (LoanDetail loanToUse in loansToUse)
                                {
                                    // we have the Client and the loan number
                                    lf = db.Loanfiles.Where(x => x.LoanFileAuditID == loanToUse.LoanFileAuditID && x.LoanNumber.ToLower() == loanToUse.LoanNbr.ToLower()).First();
                                    string loanPath = Path.Combine(loanToUse.LoanPath + "\\" + fl.Name);
                                    if (File.Exists(loanPath)) File.Delete(loanPath);


                                    TSGDataContext db2 = new TSGDataContext();
                                    sendName = TransferFile(fl.Extension, currentFile, loanPath, lf, fl.Length, imp);
                                    mailFileOut += "<tr ><td width =\"50%\">" + currentFile + "</td> <td> </td>" + "</tr>";
                                    fileCountOut += 1;
                                    // File.Delete(currentFile);

                                    lg.FileTransferTo = loanPath;
                                    if (fl.Extension.ToLower() != ".zip")
                                    {

                                        LoanFileDownload lfd = new LoanFileDownload();
                                        lfd.LoanFileID = lf.LoanfileID;
                                        lfd.FileName = sendName;
                                        lfd.FileReceived = DateTime.Now;
                                        lfd.FileSize = fl.Length;

                                        if (!db2.LoanFileDownExists(lf.LoanfileID, sendName)) db2.LoanFileDownloads.InsertOnSubmit(lfd);

                                        db2.SubmitChanges();
                                    }
                                    bool found2 = false;
                                    int ctr2 = 0;

                                    for (ctr2 = 0; ctr2 < emails.Count; ctr2++)
                                    {
                                        if (emails[ctr2].AuditDesc == loanToUse.AuditName)
                                        {
                                            found2 = true;
                                            break;
                                        }
                                    }

                                    if (found2 == false)
                                    {

                                        EmailList el = new EmailList();
                                        el.AuditDesc = loanToUse.AuditName;
                                        el.FileCount = 1;
                                        emails.Add(el);
                                    }
                                    else emails[ctr2].FileCount += 1;
                                }
                            }
                            else
                            {

                                orphans += 1;

                                string loanPath = currentFile.Replace(Path.Combine(ILMFilePath, clientName), Path.Combine(txtNAS.Text, useComp.Name + "\\Orphans"));
                                //if (directoryName.BitField == false || chkSFTP.Checked) loanPath = currentFile.Replace(mortgageDriverOutPath, txtNAS.Text + "\\" + useComp.Name + "\\Orphans\\");
                                lg.FileTransferTo = loanPath;

                                String pth2 = loanPath.Substring(0, loanPath.LastIndexOf("\\") + 1);

                                //added by javed for extration need for orphan also
                                string fName = loanPath.Substring(loanPath.LastIndexOf("\\")).Replace("\\", "");
                                string folderName = string.Empty;
                                if (fName.Contains(".tiff"))
                                {
                                    folderName = string.Concat(fName.Substring(0, fName.ToLower().IndexOf(".tiff")).Replace("\\", ""), "\\");
                                }
                                else
                                {
                                    folderName = string.Concat(fName.Substring(0, fName.ToLower().IndexOf(".pdf")).Replace("\\", ""), "\\");
                                }
                                if (!pth2.ToLower().Contains(folderName.ToLower()))
                                {
                                    pth2 = string.Concat(pth2, folderName);
                                }
                                string loanPathN = string.Concat(pth2, fName);

                                if (Directory.Exists(pth2) == false)
                                    Directory.CreateDirectory(pth2);
                                //TransferFile(fl.Extension, currentFile, loanPath, null, fl.Length, imp);
                                TransferFile(fl.Extension, currentFile, loanPathN, null, fl.Length, imp);
                                mailFileOut += "<tr ><td width =\"50%\">" + currentFile + "</td> <td> </td>" + "</tr>";
                                fileCountOut += 1;
                            }
                        }

                        //if (isPotentialLoan == false || (newFolder + "").Length > 0)
                        else if (isPotentialLoan == false || (newFolder + "").Length > 0)
                        {
                            //We store non-potential loans on ARCH, or always store it on arch if we have been told to do so.
                            if ((newFolder + "").Length > 0 && newFolder.IndexOf("QC Credit Reports") == -1)
                            {
                                //commented by javed because checks specifc folder checked not working.
                                //finalLoc = Path.Combine(txtTo.Text, companyName + "\\" + newFolder);
                                //pth = Path.Combine(txtTo.Text, companyName + "\\" + newFolder);
                            }
                            if (isPotentialLoan == false || ((newFolder + "").Length > 0 && newFolder.IndexOf("QC Credit Reports") == -1))
                            {
                                finalLoc = pth + fl.Name;
                                //Remove the service type in midle of same it is working for QC //added by javed
                                if (finalLoc.Contains("Due Diligence"))
                                {
                                    pth = pth.Replace("\\Due Diligence", "");
                                    finalLoc = finalLoc.Replace("\\Due Diligence", "");
                                }
                                else
                                {
                                    pth = pth.Replace("\\Quality Control", "");
                                    finalLoc = finalLoc.Replace("\\Quality Control", "");
                                }
                                if (currentFile.Contains("Outbound"))
                                {
                                    //pth = txtNAS.Text + clientName currentFile.Remove(currentFile.LastIndexOf("\\"));

                                    //if (Directory.Exists(pth) == false)
                                    //    Directory.CreateDirectory(pth);

                                    //TransferFile(fl.Extension, currentFile, finalLoc, null, fl.Length, imp);

                                    //File.Delete(currentFile);
                                }
                                else
                                {
                                    if (Directory.Exists(pth) == false)
                                        Directory.CreateDirectory(pth);

                                    TransferFile(fl.Extension, currentFile, finalLoc, null, fl.Length, imp);

                                    File.Delete(currentFile);
                                }
                                //added by javed for deleting file from dashboard afer moving file to destination for data file.
                                //mailFileIn += "<tr ><td width =\"50%\">" + currentFile + "</td> <td> </td>" + "</tr>";//Arch fie move
                                lg.FileTransferTo = finalLoc;
                                if (isPotentialLoan == false) archCount += 1;

                                //Create an entry for a possible email by determining the "Root" folder
                                //string rootFolder = currentFile.Replace(txtFrom.Text.TrimEnd('\\') + "\\" + clientName + "\\", "");
                                string rootFolder = currentFile.Replace(currentFile.TrimEnd('\\') + "\\" + clientName + "\\", "");
                                rootFolder = rootFolder.Replace("Quality Control\\", "");
                                rootFolder = rootFolder.Replace("Fulfillment\\", "");
                                rootFolder = rootFolder.Replace("Due Diligence\\", "");// for due diligence folder added by javed

                                if (rootFolder.IndexOf("\\") > -1) rootFolder = rootFolder.Substring(0, rootFolder.IndexOf("\\"));

                                if (rootFiles.Count == 0)
                                {
                                    ComboBoxFill cf = new ComboBoxFill();
                                    cf.DisplayValue = rootFolder;
                                    cf.ValueID = 1;
                                    rootFiles.Add(cf);
                                }
                                else
                                {
                                    for (int ctrNew = 0; ctrNew < rootFiles.Count; ctrNew++)
                                    {
                                        if (rootFiles[ctrNew].DisplayValue == rootFolder)
                                        {
                                            rootFiles[ctrNew].ValueID += 1;
                                            break;
                                        }
                                        if (ctrNew == rootFiles.Count - 1)
                                        {
                                            ComboBoxFill cf = new ComboBoxFill();
                                            cf.DisplayValue = rootFolder;
                                            cf.ValueID = 1;
                                            rootFiles.Add(cf);
                                        }
                                    }
                                }
                            }

                        }

                        //Write to the Database
                        if ((newFolder + "").Length == 0)
                        {
                            lg.DateCreated = DateTime.Now;
                            lg.FileTransferFrom = currentFile;
                            lg.IsDownload = true;
                            lg.FileLength = fl.Length;
                            lg.CompanyID = companyID;
                            log.Add(lg);
                        }
                        else
                        {
                            if (newFolder.IndexOf("QC Credit Reports") == -1)
                            {
                                lg.DateCreated = DateTime.Now;
                                lg.FileTransferFrom = currentFile;
                                lg.IsDownload = true;
                                lg.FileLength = fl.Length;
                                lg.CompanyID = companyID;
                                log.Add(lg);
                            }

                        }
                        if (isPotentialLoan == true)
                        {
                            fileCount += 1;
                            rtn += 1;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return rtn;
        }

        private bool ValidateParentFolder(string fname)
        {
            TSGDataContext db = new TSGDataContext();
            var x1 = db.SweeperFolders.Where(x => x.SweeperType == 2 && x.MortgageTransfer == true && x.isActive == true).Distinct().ToList();
            for (int i = 0; i <= x1.Count - 1; i++)
            {
                if (fname.ToUpper().EndsWith(x1[i].FolderPath.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }
        private bool ValidateMortgageFolder(string fname)
        {
            TSGDataContext db = new TSGDataContext();
            var x1 = db.SweeperFolders.Where(x => x.SweeperType == 2 && x.MortgageTransfer == true && x.isActive == true).ToList();
            for (int i = 0; i <= x1.Count - 1; i++)
            {
                if (fname.ToUpper().Contains(x1[i].SweeperFolder.ToUpper()) || fname.ToUpper().Contains("Quality Control\\Rebuttals".ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }
        private bool ValidateDDDataFolder(string fname)
        {
            TSGDataContext db = new TSGDataContext();
            var x1 = db.SweeperFolders.Where(x => x.SweeperType == 2 && x.MortgageTransfer == false && x.isActive == true && (x.PathLocationType == 3 || x.PathLocationType == 2)).ToList();
            for (int i = 0; i <= x1.Count - 1; i++)
            {
                if (fname.ToUpper().Contains(x1[i].SweeperFolder.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidateforRebuttals(string fname)
        {
            if (fname.ToUpper().Contains("QUALITY CONTROL\\REBUTTALS")) //Validate only for rebuttal folder in Quality control
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private void AccessEmailForFieldReviews()
        {
            //To use this, the program must be run from a system signed on as the user for FieldReviews.  This is TBD; leaving code open; but currently nothing actually calls this code.
            string errorNotice = "";
            OutLook.Application oApp = new OutLook.Application();
            OutLook._NameSpace oNS = oApp.GetNamespace("MAPI");
            OutLook.MAPIFolder oFolder = oNS.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox).Parent;

            MessageBox.Show(oNS.Accounts[0].UserName);

            foreach (Microsoft.Office.Interop.Outlook.MAPIFolder folder in oFolder.Folders)
                if (folder.Name == "FieldReviews")
                    oFolder = folder;
            if (oFolder == null) return;

            //    oExp = oFolder.GetExplorer(false);
            oNS.Logon("", "", false, true);

            OutLook.Items items = oFolder.Items;
            foreach (OutLook.MailItem mail in items)
            {

                if (mail.Attachments.Count == 0)
                {
                    errorNotice += mail.Subject + "<br />";
                    continue;
                }
                //read the subject


            }

        }
        private void SendMail(MailDetailType mortgageType)
        {
            Impersonator imp = new Impersonator(MortgageTransfer.Properties.Settings.Default.ImpUser, MortgageTransfer.Properties.Settings.Default.Domain, MortgageTransfer.Properties.Settings.Default.Password);
            using (imp)
            {
                if (mailMessage == "")
                {
                    lblErrors.Text = "No new files to send " + DateTime.Now.ToString();
                    return;
                }

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(MortgageTransfer.Properties.Settings.Default.EmailFrom);
                msg.To.Add(new MailAddress(txtEmail.Text));
                //msg.To.Add(new MailAddress("jsiddique@stonehillgroup.com"));
                msg.Subject = "New files from FTP - " + minlastRun.ToString("MM/dd/yyyy hh:mm tt") + " to " + timeStart.ToString("MM/dd/yyyy hh:mm tt");

                if (mortgageType == MailDetailType.AllDetails)
                {
                    msg.Body = mailMessageFileIn;
                }
                if (mortgageType == MailDetailType.InDetails)
                {
                    msg.Body = mailMessageFileReceived;
                }
                if (mortgageType == MailDetailType.OutDetails)
                {
                    msg.Body = mailMessage;
                }


                msg.IsBodyHtml = true;

                try
                {
                    sendMessage(msg);
                }
                catch (Exception ex)
                {
                    lblErrors.Text = "There is an error sending an email.  The files downloaded, but the email did not send.";
                }
            }
        }

        // Adding enum for checking mail type
        enum MailDetailType
        {
            InDetails,
            OutDetails,
            AllDetails
        }


        #endregion

        #region Monthly File Deletes
        private void DeleteFTP()
        {
            IEnumerable<string> txtFiles = Directory.EnumerateFiles(txtFrom.Text, "*.*", SearchOption.AllDirectories);
            int filesToDownload = 0;
            int dayCount = 180;
            fileCount = 0;
            errorCount = 0;
            foreach (string currentFile in txtFiles)
            {
                try
                {

                    if (currentFile.IndexOf("\\Quality Control\\") == -1) continue;
                    if (currentFile.Length > 259) continue;

                    DateTime d1 = File.GetLastWriteTime(currentFile);
                    DateTime d2 = DateTime.Now;
                    TimeSpan t1 = d2 - d1;
                    int daysDiff = Convert.ToInt16(t1.Days);
                    if (daysDiff > dayCount)
                        File.Delete(currentFile);

                }
                catch (Exception)
                {
                    errorCount += 1;
                    lstErrorFiles.Items.Add(currentFile);
                    lstErrorFiles.Visible = true;
                    lblErrorFiles.Visible = true;
                    this.Refresh();
                }
            }
            DeleteEmptyFolders(txtFrom.Text, true);
        }

        private void DeleteEmptyFolders(string path, bool skip)
        {
            path = path.ToLower().TrimEnd('\\');
            bool skipNext = (path.EndsWith("\\quality control"));
            TSGDataContext db = new TSGDataContext();
            Impersonator imp = new Impersonator(MortgageTransfer.Properties.Settings.Default.ImpUser, MortgageTransfer.Properties.Settings.Default.Domain, MortgageTransfer.Properties.Settings.Default.Password);
            using (imp)
            {
                string[] subDirectories = Directory.GetDirectories(path);
                if (skip == true)
                {
                    string compTest = path.Substring(path.LastIndexOf("\\") + 1).ToLower();
                    Company c = db.Companies.Where(x => x.Name.ToLower() == compTest || x.Name.ToLower() == compTest.Replace("&", "and").Replace(".", "").Replace(",", "")).FirstOrDefault();
                    if (c != null)
                        skipNext = true;

                }
                foreach (string strDirectory in subDirectories)
                {
                    DeleteEmptyFolders(strDirectory, skipNext);
                }

                if (skipNext == true || skip == true) return;
                if (path.IndexOf("\\quality control") == -1 || path.EndsWith("\\images") == true) return;

                if (Directory.GetFiles(path).Length + Directory.GetDirectories(path).Length == 0)
                {
                    try
                    {
                        Directory.Delete(path);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void DeleteOldRepublic(string folder, Impersonator imp)
        {
            if (Directory.Exists(folder))
            {
                IEnumerable<string> txtFiles = Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories);
                int filesToDownload = 0;
                int dayCount = 60;
                fileCount = 0;
                errorCount = 0;
                Impersonator imp1 = new Impersonator(MortgageTransfer.Properties.Settings.Default.ImpUser, "STONEHILL", MortgageTransfer.Properties.Settings.Default.Password);
                using (imp1)
                {
                    foreach (string currentFile in txtFiles)
                    {
                        try
                        {

                            if (currentFile.Length > 259) continue;
                            File.Delete(currentFile);

                        }
                        catch (Exception)
                        {
                            errorCount += 1;
                            lstErrorFiles.Items.Add(currentFile);
                            lstErrorFiles.Visible = true;
                            lblErrorFiles.Visible = true;
                            this.Refresh();
                        }
                    }
                }
            }
        }
        #endregion


        #region Functions
        private void sendMessage(MailMessage msg)
        {
            Impersonator imp = new Impersonator(MortgageTransfer.Properties.Settings.Default.ImpUser, MortgageTransfer.Properties.Settings.Default.Domain, MortgageTransfer.Properties.Settings.Default.Password);
            using (imp)
            {
                try
                {
                    SmtpClient s = new SmtpClient();
                    s.Host = Properties.Settings.Default.SMTPHost;
                    //s.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;//added by javed
                    s.DeliveryMethod = SmtpDeliveryMethod.Network; //commnetd by javed
                                                                   //s.Credentials = new System.Net.NetworkCredential("AppAdmin", "StoneMtg117#");
                    s.PickupDirectoryLocation = Properties.Settings.Default.DropDirectory;

                    s.Send(msg);
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        private List<LoanDetail> ClientNas(string clientName)
        {
            Impersonator imp = new Impersonator(MortgageTransfer.Properties.Settings.Default.ImpUser, MortgageTransfer.Properties.Settings.Default.Domain, MortgageTransfer.Properties.Settings.Default.Password);
            using (imp)
            {
                TSGDataContext db = new TSGDataContext();
                CompanyAceMapping ca = db.CompanyAceMappings.Where(x => x.CompanyID == companyID).FirstOrDefault();

                int clientID = 0;

                if (ca != null) clientID = Convert.ToInt32(ca.ACESClientID);

                List<LoanDetail> loanList = new List<LoanDetail>();
                LoanFileAudit useAudit = new LoanFileAudit();
                System.Windows.Forms.Application.DoEvents();
                int lastAuditID = 0;

                int lastDeal = 0;
                if (companyID > 0)
                {

                    txtNowDownloading.Text = "Building LES structure for :" + clientName;
                    txtNowDownloading.Refresh();
                    System.Windows.Forms.Application.DoEvents();

                    //Now we get the loans that are in LFA from LES

                    List<Deal> dealList = db.DealsByCompany(companyID);
                    foreach (Deal curDeal in dealList)
                    {
                        List<LoanDeal> dealLoanList = db.GetLoansByDetails(curDeal.DealID);
                        if (lastAuditID != curDeal.DealID)
                        {
                            //check to see if the audit exists 
                            lastDeal = curDeal.DealID;
                            useAudit = db.LoanFileAudits.Where(x => x.DealID == lastDeal && x.CompanyID == companyID).FirstOrDefault();

                            if (useAudit == null)
                            {
                                useAudit = new LoanFileAudit();
                                useAudit.AuditName = curDeal.DealName;
                                useAudit.AuditID = curDeal.DealID;
                                useAudit.Added_Date = DateTime.Now;
                                useAudit.ListReceivedDate = DateTime.Now;
                                useAudit.DealID = curDeal.DealID;
                                useAudit.CompanyID = companyID;
                                db.LoanFileAudits.InsertOnSubmit(useAudit);
                                db.SubmitChanges();
                            }
                        }

                        foreach (LoanDeal ld in dealLoanList)
                        {
                            if (ld.LoanNo == null || ld.BorrowerName == null) continue;
                            string loanPath = Path.Combine(txtNAS.Text, clientName.Replace(".", ""));
                            loanPath = Path.Combine(loanPath, curDeal.DealName.Replace(",", "").Replace(".", "").Replace("/", "-").Replace("*", ""));
                            loanPath = Path.Combine(loanPath, ld.BorrowerName.Replace(",", "").Replace(".", "").Replace("*", "") + " - " + ld.LoanNo);

                            LoanDetail dt = new LoanDetail();

                            dt.AuditName = curDeal.DealName;
                            dt.AuditID = curDeal.DealID.ToString();

                            dt.CompanyID = companyID;
                            dt.LastName = ld.BorrowerName;
                            dt.LoanNbr = ld.LoanNo;
                            LoanFileAudit lfaUse = db.LoanFileAudits.Where(x => x.DealID == curDeal.DealID).First();
                            dt.LoanFileAuditID = lfaUse.LoanFileAuditID;
                            dt.LoanPath = loanPath;
                            dt.IsFulfillment = false;
                            loanList.Add(dt);
                            TSGDataContext db2 = new TSGDataContext();
                            if (!db2.LoanFileExists(ld.LoanNo.Trim(), ld.BorrowerName.Trim(), companyID) && ld.BorrowerName != "" && ld.BorrowerName != null)
                            {

                                string ldLoanNo = ld.LoanNo.Trim();

                                Loanfile lf = db2.Loanfiles.Where(x => x.LoanFileAuditID == useAudit.LoanFileAuditID && x.LoanNumber == ldLoanNo).FirstOrDefault();
                                if (lf != null)
                                {
                                    lf.LastName = ld.BorrowerName.Trim();
                                }
                                else
                                {
                                    lf = new Loanfile();
                                    lf.LoanFileAuditID = useAudit.LoanFileAuditID;
                                    lf.LoanNumber = ld.LoanNo.Trim();
                                    lf.LastName = ld.BorrowerName.Trim();
                                    lf.Added_Date = DateTime.Now;
                                    db2.Loanfiles.InsertOnSubmit(lf);
                                }
                                db2.SubmitChanges();
                            }
                        }
                    }

                }
                return loanList.OrderByDescending(x => x.LastName.Length).ToList();
            }
        }

        private void SweepAppraisalOrders(Company useComp, Impersonator imp)
        {
            string folderPath = txtTo.Text + "\\" + useComp.Name + "\\Appraisal Orders\\";
            if (Directory.Exists(folderPath) == false) return;

            var txtFiles = new DirectoryInfo(folderPath).GetFiles("*.*", SearchOption.TopDirectoryOnly);
            TSGDataContext db = new TSGDataContext();
            CompanyAceMapping ca = db.CompanyAceMappings.Where(x => x.CompanyID == useComp.CompanyID).FirstOrDefault();

            List<Loanfile> potentialLoans = db.LoanFilesNeedingOrdering(useComp.CompanyID);
            try
            {
                foreach (FileInfo fl in txtFiles.ToList())
                {
                    string flName = fl.Name;

                    Loanfile loanToUse = potentialLoans.Where(x => flName.IndexOf(x.LoanNumber.Trim(), StringComparison.OrdinalIgnoreCase) > -1).FirstOrDefault();

                    if (loanToUse != null)
                    {
                        LoanFileAudit lfa = db.LoanFileAudits.Where(x => x.LoanFileAuditID == loanToUse.LoanFileAuditID).First();

                        string copylocation = fl.FullName.Replace(flName, "") + lfa.AuditName.Replace("/", "-") + "\\";
                        if (Directory.Exists(copylocation) == false) Directory.CreateDirectory(copylocation);
                        loanToUse.FRStatusID = 2;
                        LoanFileFRDate lfDate = db.LoanFileFRDates.Where(x => x.FRStatusID == 2 && x.LoanFileID == loanToUse.LoanfileID).FirstOrDefault();
                        if (lfDate == null)
                        {
                            lfDate = new LoanFileFRDate();
                            lfDate.FRStatusDate = DateTime.Now;
                            lfDate.FRStatusID = 2;
                            lfDate.UserID = null;
                            lfDate.LoanFileID = loanToUse.LoanfileID;
                            db.LoanFileFRDates.InsertOnSubmit(lfDate);

                        }

                        if (File.Exists(copylocation + fl.Name))
                            File.Delete(copylocation + fl.Name);
                        File.Move(fl.FullName, copylocation + fl.Name);
                        db.SubmitChanges();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private bool ValidateFileType(string fileType)
        {
            switch (fileType.ToLower())
            {
                case ".pdf":
                    return true;
                case ".zip":
                    return true;
                case ".tiff":
                    return true;
                default:
                    return false;
            }
        }
        public static Boolean isNumeric(string strToCheck)
        {
            return Regex.IsMatch(strToCheck, @"^\d+$");
        }

        #endregion

        private void chkSpecificFolder_CheckedChanged(object sender, EventArgs e)
        {
            chkSFTP.Checked = false;

        }

        private void chkSFTP_CheckedChanged(object sender, EventArgs e)
        {
            chkSpecificFolder.Checked = false;

        }

    }

    public static class ZipArchiveHelper
    {
        public static void ExtractToDirectory(string archiveFileName, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                ZipFile.ExtractToDirectory(archiveFileName, destinationDirectoryName);
            }
            else
            {
                using (var archive = ZipFile.OpenRead(archiveFileName))
                {

                    foreach (var file in archive.Entries)
                    {
                        var completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                        var directory = Path.GetDirectoryName(completeFileName);

                        if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
                            Directory.CreateDirectory(directory);

                        if (file.Name != "")
                            file.ExtractToFile(completeFileName, true);
                    }

                }
            }
        }
    }
}