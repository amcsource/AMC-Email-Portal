using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class SQLQueries
    {
        public SQLQuery obj;

        public List<SQLQuery> GetSQLQueries()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.SQLQueries.Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
            }
        }


        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (DoesAleardyExist(obj.Id, obj.Name))
                        return -1;
                    else
                    {
                        if (obj.Id == 0)
                        {
                            DB.SQLQueries.AddObject(obj);
                        }
                        else
                        {
                            DB.SQLQueries.Attach(obj);
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


        public SQLQuery GetSQLQueryByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.SQLQueries.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
        }

        public SQLQuery GetSQLQueryByName(string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.SQLQueries.Where(a => a.IsDeleted == false && a.Name == name).FirstOrDefault();
            }
        }

        private bool DoesAleardyExist(int id, string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                if (id == 0)
                {
                   var data = (from a in DB.SQLQueries
                                where a.Name == name
                                select a).SingleOrDefault();
                   if (data != null)
                       return true;
                   else
                       return false;
                }
                else
                {
                    var data = (from a in DB.SQLQueries
                                where a.Name == name && a.Id != id
                                select a).SingleOrDefault();
                    if (data != null)
                        return true;
                    else
                        return false;
                }
                
            }
        }

        //public int AddSQLQuery()
        //{
        //    try
        //    {
        //        using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
        //        {
        //            DB.SQLQueries.AddObject(obj);
        //            DB.SaveChanges();
        //            return obj.Id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        //public bool UpdateSQLQuery()
        //{
        //    try
        //    {
        //        using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
        //        {
        //            DB.SQLQueries.Attach(obj);
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
