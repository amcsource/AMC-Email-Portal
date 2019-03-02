using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class EmailAttachments
    {
        public EmailAttachment obj;

        public List<EmailAttachment> GetEmailAttachments()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailAttachments.Where(a => a.IsDeleted == false).ToList();
            }
        }

        public EmailAttachment GetAttachmentByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailAttachments.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
        }

        public List<string> GetBrowsedAttachmentsByTemplateId(int emailTemplateId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.GetBrowsedFileNamesByTemplateId(emailTemplateId).Select(a => a.Name).ToList();
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
                        DB.EmailAttachments.AddObject(obj);
                    }
                    //else
                    //{
                    //    DB.EmailAttachments.Attach(obj);
                    //    DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                    //}
                    DB.SaveChanges();
                    return obj.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void Delete(int emailTemplateId)
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.EmailAttachments.Where(a => a.EmailTemplateId == emailTemplateId).ToList().ForEach(DB.EmailAttachments.DeleteObject);
                    DB.SaveChanges();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
