using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class PrintJobProcess
    {
        public PrintJob obj;
        public List<PrintJob> GetPrintJobs()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.GetPrintJobs().Select(a => new PrintJob()
                {
                    Id = a.Id,
                    DocumentTemplateId = a.DocumentTemplateId,
                    DocumentTemplateName = a.DocumentTemplateName,
                    Process = a.Process,
                    IsProcessed = a.IsProcessed,
                    ProcessedOn = a.ProcessedOn,
                    ProcessFailed = a.ProcessFailed,
                    ProcessFailedCount = a.ProcessFailedCount,
                    ProcessFailedReason = a.ProcessFailedReason,
                    AddedOn = a.AddedOn,
                    Filename = a.Filename,
                    PatientFullName = a.PatientFullName,
                    PatientNumber = a.PatientNumber,
                    PatientRecId = a.PatientRecId,
                    SaveInCRM = a.SaveInCRM
                }).ToList();
            }
        }

        public PrintJob GetPrintJobById(long id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.PrintJobs.Where(a => a.Id == id).FirstOrDefault();
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
                        DB.PrintJobs.AddObject(obj);
                    }
                    else
                    {
                        DB.PrintJobs.Attach(obj);
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