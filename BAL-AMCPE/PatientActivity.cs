using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class PatientActivity
    {
        public Activity objActivity;
        public Contact objContact;

        public Activity GetActivityByRecId(string recId)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                return DB.Activities.Where(a => a.RecId == recId).SingleOrDefault();
            }
        }

        public string GetPatientNameByPatientNumber(string patientNumber)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                var data = DB.Contacts.Where(a => a.xfPatientNumber == patientNumber).FirstOrDefault();
                if (data != null)
                {
                    return data.FullName;
                }
                else
                    return "";
            }
        }

        public string GetPatientDOBByPatientNumber(string patientNumber)
        {
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                var data = DB.Contacts.Where(a => a.xfPatientNumber == patientNumber).FirstOrDefault();
                if (data != null)
                {
                    return Convert.ToDateTime(data.Birthdate).ToString("ddMMyyyy");
                }
                else
                    return DateTime.Now.ToString("ddMMyyyy");
            }
        }

        public bool Save(int status)
        {
            // status = 0 - new
            // status = 1 - update
            try
            {
                using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
                {
                    if (status == 0)
                    {
                        DB.Activities.AddObject(objActivity);
                    }
                    else
                    {
                        DB.Activities.Attach(objActivity);
                        DB.ObjectStateManager.ChangeObjectState(objActivity, System.Data.EntityState.Modified);
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
