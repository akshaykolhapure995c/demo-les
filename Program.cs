using System;
using System.Linq;
using System.Windows.Forms;

namespace MortgageTransfer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

                    
            TSGDataContext db = new TSGDataContext();
            ReportServer rs = db.ReportServers.First();

            MortgageTransfer.Properties.Settings.Default.DownloadDirectory =  rs.FTPFolder;
            MortgageTransfer.Properties.Settings.Default.NASDirectory = rs.NASFolder;
            MortgageTransfer.Properties.Settings.Default.UploadDirectory = rs.ArchFolder;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AutoTransfer());
        }
    }
}
