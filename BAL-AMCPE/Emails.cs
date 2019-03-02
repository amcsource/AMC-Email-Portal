using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class Emails
    {
        public Email obj;
        public List<SentEmailAttachment> objAttachment;
        
        public List<Email> GetSentEmailsByUserId(string userId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Emails.Where(a => a.SentBy == userId && a.IsDraft == false && a.IsDeleted == false).OrderByDescending(a => a.SentOn).ToList();
            }
        }

        public List<Email> GetDraftEmailsByUserId(string userId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Emails.Where(a => a.SentBy == userId && a.IsDraft == true && a.IsDeleted == false).OrderByDescending(a => a.SentOn).ToList();
            }
        }

        public GetEmailFilters_Result GetEmailFilters()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.GetEmailFilters().FirstOrDefault();
            }
        }

        public List<procGetEmailsByUser_Result> GetEmailsByUser(int pageIndex, int pageSize, string sortExpression, string searchKeyword, string status, string sentBy, string patientNumber)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetEmailsByUser(pageIndex, pageSize, sortExpression, searchKeyword, status, sentBy, patientNumber).ToList();
            }
        }

        public List<procGetEmailsByUserWithFilter_Result> GetEmailsByUserWithFilter(int pageIndex, int pageSize, string sortExpression, string searchKeyword, string status, string sentBy, string patientNumber, string templateName)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetEmailsByUserWithFilter(pageIndex, pageSize, sortExpression, searchKeyword, status, sentBy, patientNumber, templateName).ToList();
            }
        }

        public Email GetEmailByEmailId(int emailId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Emails.Where(a => a.Id == emailId && a.IsDeleted == false).SingleOrDefault();
            }
        }

        public EmailData GetEmailDetailByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return (from a in DB.Emails
                        where a.IsDeleted == false && a.Id == id
                        select new EmailData()
                           {
                               Id = a.Id,
                               From = a.From,
                               To = a.To,
                               Cc = a.Cc,
                               Bcc = a.Bcc,
                               Subject = a.Subject,
                               Body = a.Body,
                               Letter = a.Letter,
                               SentOn = Helper.GetFormattedDate(Convert.ToDateTime(a.SentOn)),
                               HasAttachments = Convert.ToBoolean(a.HasAttachments),
                               SendAsSMS = Convert.ToBoolean(a.SendASSMS),
                               StorePatientLetter = Convert.ToBoolean(a.StorePatientLetter),
                               PatientFileName = a.PatientFileName,
                               AttachmentCategory = a.AttachmentCategory,
                               AttachmentDescription = a.AttachmentDescription
                           }).SingleOrDefault();
            }
        }

        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (obj.Id == 0)
                    {
                        DB.Emails.AddObject(obj);
                    }
                    else
                    {
                        DB.Emails.Attach(obj);
                        DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                    }
                    DB.SaveChanges();
                    return obj.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public bool SaveEmailAttachments()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    foreach (SentEmailAttachment attachment in objAttachment)
                    {
                        DB.SentEmailAttachments.AddObject(attachment);
                    }
                    DB.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}