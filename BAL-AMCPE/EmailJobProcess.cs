using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class EmailJobProcess
    {
        public EmailJob obj;
        public List<SentEmailAttachment> objAttachment;

        public List<EmailJob> GetEmailJobs()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.GetScheduledEmailData().Select(a => new EmailJob() { 
                    Id = a.Id, 
                    TemplateId = a.TemplateId,
                    TemplateName = a.TemplateName,
                    SendDate = a.SendDate,
                    PatientNumber = a.PatientNumber,
                    PatientRecId = a.PatientRecId,
                    Prescription = a.Prescription
                }).ToList();
            }
        }

        public EmailJob GetEmailJobById(long id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailJobs.Where(a => a.Id == id).FirstOrDefault();
            }
        }

        public long Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (obj.Id == 0)
                    {
                        DB.EmailJobs.AddObject(obj);
                    }
                    else
                    {
                        DB.EmailJobs.Attach(obj);
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
    }
}