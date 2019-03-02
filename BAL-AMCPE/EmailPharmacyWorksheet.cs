using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class EmailPharmacyWorksheet
    {
        public PharmacyWorksheet obj;
        public List<SentEmailAttachment> objAttachment;

        public List<PharmacyWorksheet> GetPharmacyWorksheets()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.GetScheduledPharmacyWorksheets().Select(a => new PharmacyWorksheet()
                {
                    Id = a.Id,
                    DocumentTemplateId = a.DocumentTemplateId,
                    DocumentTemplateName = a.DocumentTemplateName,
                    PatientNumber = a.PatientNumber,
                    PatientRecId = a.PatientRecId,
                    ProcessOn = a.ProcessOn,
                    Process = a.Process,
                    IsProcessed = a.IsProcessed,
                    ProcessedOn = a.ProcessedOn,
                    ProcessFailed = a.ProcessFailed,
                    ProcessFailedCount = a.ProcessFailedCount,
                    ProcessFailedReason = a.ProcessFailedReason,
                    Prescription = a.Prescription,
                    AddedOn = a.AddedOn,
                    ActivityRecId = a.ActivityRecId,
                    XSORecId = a.XSORecId,
                    Flag = a.Flag,
                    PatientFullName = a.PatientFullName,
                    Filename = a.Filename
                }).ToList();
            }
        }

        public PharmacyWorksheet GetPharmacyWorksheetById(long id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.PharmacyWorksheets.Where(a => a.Id == id).FirstOrDefault();
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
                        DB.PharmacyWorksheets.AddObject(obj);
                    }
                    else
                    {
                        DB.PharmacyWorksheets.Attach(obj);
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