using System;
using System.Collections.Generic;
using System.Linq;

namespace MortgageTransfer
{
    partial class CompanyAceMapping
    {
    }

    partial class TSGDataContext
    {

        public List<LoanDeal> GetLoansByDetails(int dealID)
        {
            string strSql = "SET ARITHABORT ON; Exec Get_LoansDetails_ByCriteria2 " + Convert.ToString(dealID);
            return ExecuteQuery<LoanDeal>(strSql).ToList();
        }
        public List<Deal> DealsByCompany(int CompanyID)
        {
            DateTime today = DateTime.Today;
            DateTime fewdaysearlier = today.AddDays(-210);

            return (from l in CompanyDeals
                    join m in Deals on l.DealID equals m.DealID
                    join n in LoanFileAudits on m.DealID equals n.DealID
                    where l.CompanyID == CompanyID && m.IsActive == true
                    && (n.AuditDate) >= fewdaysearlier
                    select m).ToList();
        }
        public List<Company> CompList()
        {
            return (from i in Companies
                    join j in CompanyAceMappings on i.CompanyID equals j.CompanyID
                    orderby i.Name
                    where i.IsActive == true && i.MDClientSweep==true
                    select i).ToList();
        }
        public List<ComboBoxFill> AllActiveCompanies()
        {
            //The Difference between this and CompaniesSorted is this does NOT require the company to have a value in "Company Deals"
            var dl = (from i in Companies
                      join j in CompanyAceMappings on i.CompanyID equals j.CompanyID
                      orderby i.Name
                      where i.IsActive == true && i.MDClientSweep==true
                      select new { i.Name, i.CompanyID, j.HasOrphans }).ToList();

            List<ComboBoxFill> cF = new List<ComboBoxFill>();

            foreach (var c in dl)
            {
                int ctr = 0;

                for (ctr = 0; ctr <= cF.Count - 1; ctr++)
                {
                    if (String.Compare(cF[ctr].DisplayValue, c.Name) > 0) break;
                }

                ComboBoxFill cL = new ComboBoxFill();
                cL.DisplayValue = c.Name;
                cL.ValueID = c.CompanyID;
                cL.BitField = (bool)c.HasOrphans;
                cF.Add(cL);
            }

            ComboBoxFill cL2 = new ComboBoxFill();
            cL2.DisplayValue = "<Select Company>";
            cL2.ValueID = 0;
            cL2.BitField = false;
            cF.Insert(0, cL2);

            return cF;
        }
        public List<LoanFileAudit> UpdateCompleteAudits(int companyId)
        {
            string strSql = "Select lfb.* " +
            "From LoanFileAudit lfb " +
            "where lfb.LoanFileAuditID in " +
            "(Select lfa.LoanFileAuditID " +
            "From LoanFileAudit lfa inner join Loanfile lf on lfa.LoanFileAuditID = lf.LoanFileAuditID " +
            "left join (Select Distinct LoanFileDownload.LoanFileID From LoanFileDownload) " +
            "lfd on lf.LoanfileID = lfd.LoanFileID " +
            "Where CompanyID = " + companyId + " and FileReceivedDate is null " +
            "group by lfa.LoanFileAuditID " +
            "Having Sum(Case When lfd.LoanFileID is null then 0 else 1 end) = COUNT(*))";
            return ExecuteQuery<LoanFileAudit>(strSql).ToList();
        }

        public List<FTPLog> LogsNotDeleted(int days)
        {
            return (from i in FTPLogs
                    where i.DeletedFromServer == false &&
                          i.DateCreated < DateTime.Now.AddDays(-1 * days)
                    select i).ToList();
        }
        public FTPTransferLogin CurrentLogin()
        {
            return (from i in FTPTransferLogins select i).FirstOrDefault();
        }

        public List<string> ClientID()
        {
            return (from i in Companies select i.Name).ToList();
        }
        public int ClientIDByFolderName(string foldername, bool returnCompanyID = false)
        {
            int? compID = (from i in Companies where i.Name == foldername select i.CompanyID).FirstOrDefault();

            if (compID == null)
            {
                compID = (from i in Companies where i.Name.Replace(".", "").Replace("&", "and") == foldername select i.CompanyID).FirstOrDefault();
            }
            if (compID == null)
            {
                return 0;
            }

            if (returnCompanyID == false) return ClientIDByCompanyID(Convert.ToInt32(compID));
            else return Convert.ToInt32(compID);

        }
        public int ClientIDByCompanyID(int companyID)
        {
            int? idClient = (from i in CompanyAceMappings where i.CompanyID == companyID select i.ACESClientID).FirstOrDefault();

            if (idClient != null) return Convert.ToInt32(idClient);
            else return 0;
        }
        public bool LoanFileExists(string loanNumber, string lastName, int companyID)
        {
            int? dl = (from i in Loanfiles
                       join j in LoanFileAudits on
                       i.LoanFileAuditID equals j.LoanFileAuditID
                       where i.LoanNumber == loanNumber &&
                       i.LastName == lastName &&
                       j.CompanyID == companyID
                       select i.LoanfileID).FirstOrDefault();

            if (dl != 0) return true;
            else return false;

        }
        public List<String> CompanyNamePossibleMatches(string shortName)
        {
            return (from i in CompanyAceMappings
                    where i.CompanyShortName.ToLower().StartsWith(shortName) == true && i.CompanyShortName.ToLower() != shortName
                    select i.CompanyShortName).ToList();

        }
        public bool LoanFileDownExists(int loanFileID, string fileName)
        {
            LoanFileDownload lfd = (from i in LoanFileDownloads where i.LoanFileID == loanFileID && i.FileName == fileName select i).FirstOrDefault();

            if (lfd == null) return false;
            else return true;

        }
        public Loanfile LoanFileByNumandName(string loanNumber, int companyID)
        {
            return (from i in Loanfiles
                    join j in LoanFileAudits on i.LoanFileAuditID equals j.LoanFileAuditID
                    where i.LoanNumber == loanNumber &&
                    j.CompanyID == companyID
                    select i).FirstOrDefault();

        }
        public CompanyAceMapping CompanyAcesByCompanyID(string clientName)
        {
            return (from i in CompanyAceMappings
                    join j in Companies on i.CompanyID equals j.CompanyID
                    where j.Name == clientName
                    select i).FirstOrDefault();
        }
        public List<ComboBoxFill> SubDirectories()
        {
            List<FTPSubDirectory> dl = (from i in FTPSubDirectories select i).ToList();

            List<ComboBoxFill> rtn = new List<ComboBoxFill>();

            foreach (FTPSubDirectory useDir in dl)
            {
                ComboBoxFill rtnAdd = new ComboBoxFill();
                rtnAdd.DisplayValue = useDir.FTPSub + " - ";
                if (useDir.Type == 1) rtnAdd.DisplayValue += "Fulfillment";
                else rtnAdd.DisplayValue += "Quality Control";
                rtnAdd.ValueID = useDir.FTPSubID;
                rtn.Add(rtnAdd);
            }
            return rtn.OrderBy(x => x.DisplayValue).ToList();

        }
        public List<Company> companyListToRun()
        {
            return (from i in Companies
                    join j in CompanyAceMappings on i.CompanyID equals j.CompanyID
                    where j.ExternalMappings != true
                    select i).ToList();
        }
        public void UpdateTimes(DateTime setDate, int compID = 0)
        {
            string strSql = "Update CompanyAceMappings Set LastRun = '" + setDate.ToString() + "'";
            if (compID > 0) strSql += " Where CompanyID = " + compID;
            strSql += "; select 0";
            ExecuteQuery<int>(strSql);

        }
        public void UpdateAuditTypes()
        {
            string strSql = "Update LoanFileAudit " +
            "Set AuditDate = Convert(datetime, Convert(varchar(10),MonthVal) + '/1/' + SUBSTRING(auditname, LEN(Searchterm) + 2, 4)), " +
            "AuditTypeID = (Select Top 1 AuditTypeID From AuditTypeChecks atc Where AuditName like '%' + ShortName + '%' order by Checkorder Desc) " +
            "From LoanFileAudit cross join AuditTypeSearchTerms ats " +
            "Where AuditName like SearchTerm + '%' and (AuditTypeID is null or AuditDate is null); select 0 ";

            ExecuteQuery<int>(strSql);


        }
        public List<Loanfile> LoanFilesNeedingOrdering(int companyId)
        {
            return (from i in Loanfiles
                    join j in LoanFileAudits on i.LoanFileAuditID equals j.LoanFileAuditID
                    where i.FRStatusID == 1 && j.CompanyID == companyId
                    select i).ToList();
        }
    }
}
