using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class Doctors
    {
        public DAL_AMCPE.Doctor obj;

        public List<DAL_AMCPE.Doctor> GetDoctors()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Doctors.Where(a => a.IsDeleted == false).OrderBy(a => a.DoctorName).ToList();
            }
        }

        public DAL_AMCPE.Doctor GetDoctorByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Doctors.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
        }


        public GetDoctorForPatient_Result GetDoctorForPatient(string patientNumber)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.GetDoctorForPatient(patientNumber).SingleOrDefault();
            }
        }

        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (DoesAleardyExist(obj.Id, obj.DoctorName))
                        return -1;
                    else
                    {
                        if (obj.Id == 0)
                        {
                            DB.Doctors.AddObject(obj);
                        }
                        else
                        {
                            DB.Doctors.Attach(obj);
                            DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                        }
                        
                        DB.SaveChanges();
                        return obj.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool Delete()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                try
                {
                    //DB.UsersInGroups.Where(a => a.GroupId == obj.Id).ToList().ForEach(DB.UsersInGroups.DeleteObject);
                    //DB.Permissions.Where(a => a.GroupId == obj.Id).ToList().ForEach(DB.Permissions.DeleteObject);
                    
                    //DB.Groups.Attach(obj);
                    //DB.Groups.DeleteObject(obj);
                    //DB.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }


        private bool DoesAleardyExist(int id, string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                DAL_AMCPE.Doctor data;
                if (id == 0)
                {
                    data = (from a in DB.Doctors
                            where a.DoctorName == name
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.Doctors
                            where a.DoctorName == name && a.Id != id
                            select a).SingleOrDefault();
                }

                if (data != null)
                    return true;
                else
                    return false;

            }
        }
    }
}