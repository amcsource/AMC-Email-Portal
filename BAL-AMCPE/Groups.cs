using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class Groups
    {
        public Group obj;
        public Permission permissionSet;

        public List<Group> GetGroups()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Groups.Where(a => a.IsDeleted == false).OrderBy(a => a.GroupName).ToList();
            }
        }



        public Group GetGroupsByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.Groups.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
        }

        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (DoesAleardyExist(obj.Id, obj.GroupName))
                        return -1;
                    else
                    {
                        if (obj.Id == 0)
                        {
                            DB.Groups.AddObject(obj);
                            
                            // add permission
                            permissionSet.GroupId = obj.Id;  
                            DB.Permissions.AddObject(permissionSet);
                        }
                        else
                        {
                            DB.Groups.Attach(obj);
                            DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);

                            // update permission
                            DB.Permissions.Attach(permissionSet);
                            DB.ObjectStateManager.ChangeObjectState(permissionSet, System.Data.EntityState.Modified);
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
                    DB.UsersInGroups.Where(a => a.GroupId == obj.Id).ToList().ForEach(DB.UsersInGroups.DeleteObject);
                    DB.Permissions.Where(a => a.GroupId == obj.Id).ToList().ForEach(DB.Permissions.DeleteObject);
                    
                    DB.Groups.Attach(obj);
                    DB.Groups.DeleteObject(obj);
                    //DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                    DB.SaveChanges();
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
                DAL_AMCPE.Group data;
                if (id == 0)
                {
                    data = (from a in DB.Groups
                            where a.GroupName == name
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.Groups
                            where a.GroupName == name && a.Id != id
                            select a).SingleOrDefault();
                }

                if (data != null)
                    return true;
                else
                    return false;

            }
        }


        //public int AddGroup()
        //{
        //    try
        //    {
        //        using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
        //        {
        //            DB.Groups.AddObject(obj);
        //            DB.SaveChanges();
        //            return obj.Id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        //public bool UpdateGroup()
        //{
        //    try
        //    {
        //        using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
        //        {
        //            DB.Groups.Attach(obj);
        //            DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
        //            DB.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
