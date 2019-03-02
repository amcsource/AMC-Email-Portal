using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class ExcelJobProcess
    {
        public ExcelJob obj;
        public List<ExcelJob> GetExcelJobs()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.GetExcelJobs().Select(a => new ExcelJob()
                {
                    Id = a.Id,
                    ProcName = a.ProcName,
                    EmailId = a.EmailId,
                    Process = a.Process,
                    IsProcessed = a.IsProcessed,
                    ProcessedOn = a.ProcessedOn,
                    ProcessFailed = a.ProcessFailed,
                    ProcessFailedCount = a.ProcessFailedCount,
                    ProcessFailedReason = a.ProcessFailedReason,
                    AddedOn = a.AddedOn,
                    Filename = a.Filename,
                    EmailSubject = a.EmailSubject,
                    EmailBody = a.EmailBody
                }).ToList();
            }
        }

        public ExcelJob GetExcelJobById(long id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.ExcelJobs.Where(a => a.Id == id).FirstOrDefault();
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
                        DB.ExcelJobs.AddObject(obj);
                    }
                    else
                    {
                        DB.ExcelJobs.Attach(obj);
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