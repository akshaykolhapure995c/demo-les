using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MortgageTransfer_FLUW
{
    public partial class AutoTransfer : Form
    {

        string mailMessage = "";
        DateTime timeStart = new DateTime();
        DateTime minlastRun = new DateTime();
        DateTime timeToRun = new DateTime();
        bool invalid = false;

        IList<Notification> lstEmailMessage = new List<Notification>();
        public AutoTransfer(string[] args)
        {
            InitializeComponent();
            btnRunNow.Enabled = false;

            //---------- For Windows Service call.-------------------------------------------------------------
            if (args.Length >= 0)
            {
                bool isError = false;
                int BackgroundProcessID = 2; //MortgageTransfer_FLUW

                using (TSGDataContext db = new TSGDataContext())
                {

                    try
                    {
                        GetClientDirectoryFromFtp();
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        string msg = "ADDED BY:::MortgageTransfer_FLUW," + "ErrorMessgae:" + ex.Message + Environment.NewLine;
                        msg += "trace:" + ex.StackTrace;

                        BackgroundProcessQueue newErrorlog = new BackgroundProcessQueue();
                        newErrorlog.BackgroundProcessID = BackgroundProcessID;
                        newErrorlog.RequestedBy = 1; //Admin
                        newErrorlog.RequestedOn = DateTime.Now;
                        newErrorlog.IsCompleted = false;
                        newErrorlog.Status = 3; //Error
                        newErrorlog.StatusError = msg;

                        db.BackgroundProcessQueues.InsertOnSubmit(newErrorlog);
                        db.SubmitChanges();
                    }

                    if (!isError)
                    {
                        BackgroundProcessQueue newErrorlog = new BackgroundProcessQueue();
                        newErrorlog.BackgroundProcessID = BackgroundProcessID; 
                        newErrorlog.RequestedBy = 1; //Admin
                        newErrorlog.CompletedOn = DateTime.Now;
                        newErrorlog.IsCompleted = true;
                        newErrorlog.Status = 4; //Completed

                        db.BackgroundProcessQueues.InsertOnSubmit(newErrorlog);
                    }

                    BackgroundProcess proc = db.BackgroundProcesses.Where(x => x.BackgroundProcessID == BackgroundProcessID).FirstOrDefault();
                    proc.IsRunning = false;

                    db.SubmitChanges();

                    Environment.Exit(0);
                }
            }
            //--------------------------------------------------------------------------------------------------------
        }

        private void btnRunNow_Click(object sender, EventArgs e)
        {
            if (CheckValidation())
                GetClientDirectoryFromFtp();
        }
        private void AutoTransfer_Load(object sender, EventArgs e)
        {
            PageLoad();

        }
        public void PageLoad()
        {
            txtFtpLocation.Text = MortgageTransfer_FLUW.Resource.DownloadDirectory;
            txtLoanFileLocation.Text = MortgageTransfer_FLUW.Resource.UploadDirectory;
            txtToEmail.Text = MortgageTransfer_FLUW.Resource.EmailTo;
            cbCompany.Enabled = false;

        }
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
            try
            {
                using (TSGDBDataContext context = new TSGDBDataContext())
                {
                    GetClientDirectoryFromFtp();

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                btnRunNow.Enabled = true;
                Start.Enabled = false;
                using (TSGDBDataContext dataContext = new TSGDBDataContext())
                {

                    cbCompany.DataSource = dataContext.GetAllActiveCompany();
                    cbCompany.DisplayMember = "companyName";
                    cbCompany.ValueMember = "companyId";
                }
                cbCompany.Enabled = true;
            }
            else
            {
                cbCompany.SelectedIndex = 0;
                cbCompany.Enabled = false;
                btnRunNow.Enabled = false;
                Start.Enabled = true;
            }
        }
        /// <summary>
        /// Developer Name:Priya kant
        /// Date:11-20-2014
        /// Description: Get the directory from FTP location.
        /// </summary>
        private void GetClientDirectoryFromFtp()
        {

            Impersonator imp = new Impersonator(MortgageTransfer_FLUW.Resource.ImpUser, MortgageTransfer_FLUW.Resource.Domain, MortgageTransfer_FLUW.Resource.Password);
            using (imp)
            {
                lblCurrentTime.Text = "Running Files Transfer";
                mailMessage = "";
                lstErrorLog.Items.Clear();
                lstEmailMessage.Clear();
                if (checkBox1.Checked && Convert.ToInt32(cbCompany.SelectedValue) != 0)
                {
                    if (Directory.Exists(txtFtpLocation.Text + cbCompany.Text + "\\" + MortgageTransfer_FLUW.Resource.UnderWritingServiceName + "\\Loan Files"))
                    {
                        btnRunNow.Visible = false;
                        GetClientFileNameAndParse(txtFtpLocation.Text + cbCompany.Text + "\\" + MortgageTransfer_FLUW.Resource.UnderWritingServiceName + "\\Loan Files", cbCompany.Text, Convert.ToInt32(cbCompany.SelectedValue), 0);
                    }
                    else
                    {
                        MessageBox.Show("Directory does not exist inside this company.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    using (TSGDBDataContext context = new TSGDBDataContext())
                    {
                        foreach (FTPDirectory useDir in context.FTPDirectories)
                        {
                            btnRunNow.Visible = false;
                            string finalDirectory = useDir.DirectoryName.TrimEnd('\\') + "\\";
                            if (Directory.Exists(finalDirectory))
                            {
                                IEnumerable<string> clientDirectory = Directory.EnumerateDirectories(finalDirectory, "*.*", SearchOption.TopDirectoryOnly);
                                List<string> clientDirectoryList = clientDirectory.ToList();
                                clientDirectoryList.Sort();
                                foreach (string path in clientDirectoryList)
                                {
                                    if (useDir.HasClientFolders)
                                    {
                                        int? ClientId = context.CompanyFolders.Where(x => x.FolderPath.Equals(Path.GetFileName(path).Trim())).Select(x => x.CompanyID).FirstOrDefault();
                                        if (ClientId == null)
                                        {
                                            ClientId = context.Companies.Where(x => x.Name.Equals(Path.GetFileName(path).Trim())).Select(x => x.CompanyID).FirstOrDefault();
                                        }

                                        GetClientFileNameAndParse(path + "\\" + MortgageTransfer_FLUW.Resource.UnderWritingServiceName + "\\Loan Files", Path.GetFileName(path), ClientId, 0);
                                    }
                                    else
                                    {
                                        if (path.IndexOf(MortgageTransfer_FLUW.Resource.UnderWritingServiceName) != -1)
                                        {
                                            GetClientFileNameAndParse(path + "\\Loan Files", context.Companies.Where(x => x.CompanyID == useDir.CompanyID).Select(x => x.Name).FirstOrDefault(), useDir.CompanyID, 0);
                                        }
                                    }
                                    //manual sweeper
                                    string folderName = string.Empty;
                                    CompanyComboBoxFill objCompanySweep = context.GetClientIDToSweep();
                                    if (objCompanySweep != null)
                                    {
                                        CompanyFolder companyFolder = context.CompanyFolders.Where(x => x.CompanyID == objCompanySweep.companyId).FirstOrDefault();
                                        if (companyFolder != null)
                                        {
                                            folderName = companyFolder.FolderPath;
                                        }
                                        else
                                        {
                                            folderName = context.Companies.Where(x => x.CompanyID == objCompanySweep.companyId).Select(x => x.Name).FirstOrDefault();
                                        }
                                        switch (objCompanySweep.serviceId)
                                        {
                                            case 12://Underwriting
                                                if (Directory.Exists(txtFtpLocation.Text + folderName + "\\" + MortgageTransfer_FLUW.Resource.UnderWritingServiceName + "\\Loan Files"))
                                                    GetClientFileNameAndParse(txtFtpLocation.Text + folderName + "\\" + MortgageTransfer_FLUW.Resource.UnderWritingServiceName + "\\Loan Files", folderName, objCompanySweep.companyId, objCompanySweep.userId);
                                                break;
                                            case 7://Quality control
                                                break;
                                        }
                                        context.DeleteClientIdfFromCompanySweep(objCompanySweep.SweepCompanyID);
                                    }
                                    //end sweeper
                                }
                            }
                        }
                    }
                }
                CreateMailMessageNotification(lstEmailMessage);
                SendMail();
                SetTimeToRun();
                lblLastSucceed.Text = "Last Successful Run: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                lblLastSucceed.ForeColor = System.Drawing.Color.Green;
                SetTimeToRun();
                btnRunNow.Visible = true;
            }
        }
        /// <summary>
        /// Developer Name:Priya kant
        /// Date:11-20-2014
        /// Description:Get file from ftp location and parse the loan number and last borrower.
        /// </summary>
        private void GetClientFileNameAndParse(string directoryPath, string companyName, int? companyId, int userId)
        {
            IList<string> lstFileName = new List<string>();
            int filemoveCount = 0;
            int fileReceivedCount = 0;
            int fileUnRecoCount = 0;
            string loanFilePath = string.Empty;
            string sourceFileName = string.Empty;
            try
            {
                if (Directory.Exists(directoryPath))
                {

                    IEnumerable<string> clientFilesName = Directory.GetFiles(directoryPath, "*.*");
                    if (checkBox1.Checked && Convert.ToInt32(cbCompany.SelectedValue) != 0 && clientFilesName.Count() == 0)
                    {
                        MessageBox.Show("File does not exist inside this company.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        foreach (string filename in clientFilesName)
                        {
                            fileReceivedCount++;
                            lstFileName = Path.GetFileName(filename).Split('_').ToList();
                            if (lstFileName.Count == 3)//If correct formate loan file received
                            {
                                if (EstablishingLoans(lstFileName[1], lstFileName[0], companyName, userId) == 1)//if database establishing with no error
                                {
                                    loanFilePath = MoveFileFromFtpToLoanFileFolder(filename, lstFileName[0], lstFileName[1], companyName);
                                    lstErrorLog.Items.Add("Succesfully Move:" + filename);
                                    filemoveCount++;
                                }
                                else
                                {
                                    lstErrorLog.Items.Add("Error has been occured:" + filename);
                                }
                            }
                            else
                            {
                                fileUnRecoCount++;
                                lstErrorLog.Items.Add("Unrecognize file has been found:" + filename);
                                string destinationPath = directoryPath.Replace("Loan Files", "Unrecognized Loan Files");
                                string destinationFilePath = Path.Combine(destinationPath, Path.GetFileName(filename));
                                if (!Directory.Exists(destinationPath))
                                    Directory.CreateDirectory(destinationPath);
                                if (!File.Exists(destinationFilePath))
                                {
                                    File.Copy(filename, destinationFilePath, false);
                                }
                                else
                                {
                                    destinationFilePath = destinationPath + "\\" + Path.GetFileNameWithoutExtension(filename) + DateTime.Now.ToString(" MM-dd-yyyy-HH-mm-ss") + Path.GetExtension(filename);
                                    File.Copy(filename, destinationFilePath);
                                }
                                if (File.Exists(destinationFilePath))
                                {
                                    File.Delete(filename);
                                }
                                loanFilePath = destinationFilePath;
                            }
                            sourceFileName = filename;
                            InsertLog(sourceFileName, loanFilePath, companyId, false, string.Empty);//insert the log
                        }
                        if (fileReceivedCount > 0)
                            lstEmailMessage.Add(new Notification() { companyName = companyName, fileRecived = fileReceivedCount, unrecognizedFile = fileUnRecoCount, fileMove = filemoveCount });
                    }
                }//end if directory exist 
            }
            catch (Exception ex)
            {
                InsertLog(sourceFileName, loanFilePath, companyId, true, ex.Message);//insert the log
            }
        }
        /// <summary>
        /// Developer Name:Priya kant
        /// Date:11-20-2014
        /// Description: Establishing the loan in LES table.
        /// </summary>
        public int EstablishingLoans(string loanNumber, string borrowerLastName, string companyName, int userId)
        {
            int sp_return = 1;
            try
            {
                using (TSGDBDataContext db = new TSGDBDataContext())
                {
                    if (!db.IsLoanExist(companyName, borrowerLastName, loanNumber))
                    {
                        int DealID = db.GetFLUWDealID(companyName);
                        if (DealID > 0)
                        {
                            sp_return = db.InsertLoans_MortgageTransfer_FLUW(DealID, userId, borrowerLastName, loanNumber);//Insert the loan info in the tables.
                        }
                    }
                }
            }
            catch (Exception)
            {
                sp_return = 0;
            }
            return sp_return;

        }
        /// <summary>
        /// Developer Name:Priya kant
        /// Date:11-20-2014
        /// Description: File move from FTP location to Loan file location.
        /// </summary>
        public string MoveFileFromFtpToLoanFileFolder(string fileName, string borrowerLastName, string loanNumber, string CompanyName)
        {
            if (!Directory.Exists(Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName)))
                Directory.CreateDirectory(Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName));
            if (!Directory.Exists(Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName) + "\\FLUW"))
                Directory.CreateDirectory(Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName) + "\\FLUW");
            if (!Directory.Exists(Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName) + "\\FLUW\\" + borrowerLastName + "-" + loanNumber))
                Directory.CreateDirectory(Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName) + "\\FLUW\\" + borrowerLastName + "-" + loanNumber);

            string loanFilePath = Path.Combine(Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName) + "\\FLUW\\" + borrowerLastName + "-" + loanNumber, Path.GetFileName(fileName));
            if (!File.Exists(loanFilePath))
            {
                File.Copy(fileName, loanFilePath);
            }
            else
            {
                loanFilePath = Path.Combine(MortgageTransfer_FLUW.Resource.UploadDirectory, CompanyName) + "\\FLUW\\" + borrowerLastName + "-" + loanNumber + "\\" + Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString(" MM-dd-yyyy-HH-mm-ss") + Path.GetExtension(fileName);
                File.Copy(fileName, loanFilePath);
            }
            if (File.Exists(loanFilePath))
            {
                File.Delete(fileName);
            }
            return loanFilePath;
        }
        public void CreateMailMessageNotification(IList<Notification> lstEmailMessage)
        {
            foreach (var item in lstEmailMessage)
            {
                if (mailMessage.Length == 0)
                {
                    mailMessage = "<p><b>The following client underwrinting files have been received</b></p><table border='0' style=\"table-layout:fixed;\"><tr><td width=\"50%\">Directory Searched</td>"
                    + "<td><b># Files Received</b></td><td td width=\"30%\"><b># Files Move</b></td><td td width=\"20%\"><b># Unrecognized file </b></td>";
                }
                mailMessage += "<tr><td colspan = \"2\"><b>" + item.companyName + "</b></td></tr>";
                mailMessage += "<tr><td width=\"50%\">" + "FTP Directory" + "</td><td>" + item.fileRecived + "</td><td>" + item.fileMove + "</td><td>" + item.unrecognizedFile + "</td></tr>";
            }
            //return mailMessage;
        }
        /// <summary>
        /// Developer Name:Priya kant
        /// Date:11-20-2014
        /// Description: Send the email after sweep the company.
        /// </summary>
        private void SendMail()
        {
            if (mailMessage == "")
            {
                lstErrorLog.Items.Add("No new files to send " + DateTime.Now.ToString());
                return;
            }

            MailMessage msg = new MailMessage();
            //Set TO
            if (!(txtToEmail.Text == string.Empty))
            {
                IList<string> toaddr = txtToEmail.Text.Split(';').ToList();
                foreach (string item in toaddr)
                {
                    if ((item + "").Length > 0)
                        msg.To.Add(item);
                }
            }
            msg.From = new MailAddress(MortgageTransfer_FLUW.Resource.EmailFrom);
            msg.Subject = "New files from FTP Underwrinting - " + minlastRun.ToString("MM/dd/yyyy hh:mm tt") + " to " + timeStart.ToString("MM/dd/yyyy hh:mm tt");
            msg.Body = mailMessage;
            msg.IsBodyHtml = true;

            SmtpClient sclient = new SmtpClient();
            sclient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            sclient.PickupDirectoryLocation = MortgageTransfer_FLUW.Resource.EmailSaveDirectory;

            //sclient.Host = MortgageTransfer_FLUW.Resource.SMTPHost;
            try
            {
                sclient.Send(msg);
            }
            catch
            {
                lstErrorLog.Items.Add("There is an error sending an email.  The files downloaded, but the email did not send.");
            }
        }
        private bool CheckValidation()
        {
            bool isPass = false;
            if (string.IsNullOrEmpty(txtFtpLocation.Text.Trim()))
                MessageBox.Show("Please enter ftp location Path.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrEmpty(txtLoanFileLocation.Text.Trim()))
                MessageBox.Show("Please enter loan file location path.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (String.IsNullOrEmpty(txtToEmail.Text))
                MessageBox.Show("Please enter email address.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (!Regex.IsMatch(txtToEmail.Text,
                           @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                           @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", RegexOptions.IgnoreCase))
                MessageBox.Show("Email is not in the correct format.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else if (checkBox1.Checked && Convert.ToInt32(cbCompany.SelectedValue) == 0)
                MessageBox.Show("Please select company.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                isPass = true;
            }
            return isPass;
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
        /// <summary>
        /// Developer Name:Priya kant
        /// Date:11-20-2014
        /// Description: Insert the log in FTP table after success or fail move.
        /// </summary>
        private void InsertLog(string fileFrom, string fileTo, int? companyId, bool isFail, string errorMessage)
        {
            using (TSGDBDataContext context = new TSGDBDataContext())
            {
                FTPLog tblFtpLog = new FTPLog();
                tblFtpLog.FileTransferFrom = fileFrom;
                tblFtpLog.FileTransferTo = fileTo;
                tblFtpLog.CompanyID = companyId;
                tblFtpLog.ErrorMessage = errorMessage;
                tblFtpLog.isFail = isFail;
                tblFtpLog.DateCreated = DateTime.Now;
                context.FTPLogs.InsertOnSubmit(tblFtpLog);
                context.SubmitChanges();
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            lstErrorLog.Items.Clear();
            this.Text += " version " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            SetTimeToRun();
            timerInterval.Tick += new EventHandler(TimerTick); // Everytime timer ticks, timer_Tick will be called
            timerInterval.Interval = (60000) * Convert.ToInt32(MortgageTransfer_FLUW.Properties.Settings.Default.TimerTickInMinut);
            // Timer will tick every minute given in resource file.
            timerInterval.Enabled = true; // Enable the timer
            timerInterval.Start();
            TimerTick(sender, e);
        }
    }
    /// <summary>
    /// Developer Name:Priya kant
    /// Date:11-20-2014
    /// Description: class for notifacation email.
    /// </summary>
    public class Notification
    {
        public string companyName { get; set; }
        public int fileRecived { get; set; }
        public int fileMove { get; set; }
        public int unrecognizedFile { get; set; }
        public string fileTransferFrom { get; set; }
        public string fileTransferTo { get; set; }
        public int companyId { get; set; }
        public string errorMessage { get; set; }
    }
}
