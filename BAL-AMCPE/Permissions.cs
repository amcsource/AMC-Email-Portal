using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class Permissions
    {
        public Permission obj;

        public int AddPermissions()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.Permissions.AddObject(obj);
                    DB.SaveChanges();
                    return obj.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Permission GetPermissionByGroupID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Permissions.Where(a => a.GroupId == id).FirstOrDefault();
            }
        }


        public bool UpdateGroupPermissions()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.Permissions.Attach(obj);
                    DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
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
