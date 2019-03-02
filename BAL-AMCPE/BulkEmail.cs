using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class BulkEmail
    {
        public List<PatientsBulkEmail> GetPatientsForBulkEmail(string patientType, int pageIndex, int pageSize, string sortExpression, string searchKeyword)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                return DB.GetPatientsForBulkEmail(patientType, pageIndex, pageSize, sortExpression, searchKeyword).Select(a => new PatientsBulkEmail()
                {
                    PatientNumber = a.PatientNumber,
                    PatientName = a.PatientName,
                    PatientRecId = a.PatientRecId,
                    Source = a.Source,
                    Stage = a.Stage,
                    Email = a.Email,
                    Phone = a.Phone
                }).ToList();
            }
        }

        public static void UpdatePatientFlag(string patientType, string patientRecId)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                DB.UpdatePatientFlag(patientType, patientRecId);
            }
        }
    }

    public class PatientsBulkEmail
    {
        public string PatientNumber { get; set; }
        public string PatientName { get; set; }
        public string PatientRecId { get; set; }
        public string Source { get; set; }
        public string Stage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TotalPages { get; set; }
    }
}
