using System;
using System.Collections.Generic;
using System.Linq;


namespace FTPTransfer
{

    partial class enterpriseDBDataContext
    {
        public List<LoanDetail> LoanDetailsByClient(int clientID)
        {
            List<AuditData> ad = ExecuteQuery<AuditData>(@"Select i.LastName, i.LOANNBR, i.AuditID, j.AuditDesc
            From AcesSQL.acesdata.AUDITRK i inner join dbo.AuditDetail j on i.AuditID = j.AuditID and i.Customernbr = j.ClientID
            where j.statusid <= 13 and j.AuditDesc not like '%TEST%' and j.AuditDesc not like '%Closed%' and j.AuditStatusDate >= '9/1/2014' and j.ClientID = " + clientID).ToList();

            List<LoanDetail> rtn = new List<LoanDetail>();

            for (int ctr = 0; ctr < ad.Count(); ctr++)
            {
                LoanDetail ld = new LoanDetail();
                
                ld.LastName = ad[ctr].LastName.Trim();
                ld.LoanNbr = ad[ctr].LOANNBR.Trim();
                ld.AuditID = ad[ctr].AuditID.ToString();
                ld.AuditName = ad[ctr].AuditDesc.Trim();
                ld.IsFulfillment = false;
                if (rtn.IndexOf(ld) == -1) rtn.Add(ld);
            }
            return rtn.OrderBy(x => x.AuditID).ToList();

        }
        public List<LoanDetail> LoanDetailByClientAndAuditID(int clientID, int auditID)
        {
            var dl = (from i in AUDITRKs
                      join j in AuditDetails on new { AuditID = i.AUDITID, ClientID = i.CUSTOMERNBR } equals new { j.AuditID, j.ClientID }
                      where (i.CUSTOMERNBR == clientID && i.AUDITID == auditID.ToString())
                      select new { i.LASTNAME, i.LOANNBR, i.AUDITID, j.AuditDesc }).ToList();

            List<LoanDetail> ldList = new List<LoanDetail>();

            foreach (var ad in dl)
            {
                LoanDetail ld = new LoanDetail();
                ld.LastName = ad.LASTNAME.Trim();
                ld.LoanNbr = ad.LOANNBR.Trim();
                ld.AuditID = ad.AUDITID.Trim();
                ld.AuditName = ad.AuditDesc.Trim();
                ld.IsFulfillment = false;
                ldList.Add(ld);
            }
            return ldList;
        }

        public AuditDetail BillAuditByCompanyAndAuditID(string auditId, string clientName)
        {
            return (from i in AuditDetails join j in ClientDetails on i.ClientID equals j.ClientID
                    where i.AuditID == auditId && j.ClientNm == clientName select i).FirstOrDefault();
        }
        
    }
}
