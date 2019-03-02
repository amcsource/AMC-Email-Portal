using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class TagSQL
    {
        public DAL_AMCPE.TagSQL obj;
        //public Permission permissionSet;

        public List<DAL_AMCPE.TagSQL> GetTagSQL()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.TagSQLs.Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
            }
        }



        public List<procGetTagSQLByTagCategory_Result> GetTagSQLByTagCategory(int pageIndex, int pageSize, string sortExpression, string searchKeyword, int tagCategoryId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetTagSQLByTagCategory(pageIndex, pageSize, sortExpression, searchKeyword, tagCategoryId).ToList();
            }
        }


        public DAL_AMCPE.TagSQL GetTagSQLByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.TagSQLs.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
        }


        public string GetTagSQLByTagIDAndTagSQLName(int tagCategoryId, string tagSQLName)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                DAL_AMCPE.TagSQL data = DB.TagSQLs.Where(a => a.IsDeleted == false && a.TagCategoryId == tagCategoryId && a.Name.ToLower() == tagSQLName.ToLower()).SingleOrDefault();
                if (data != null)
                    return data.Query;
                else
                    return "";
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
                            DB.TagSQLs.AddObject(obj);
                        }
                        else
                        {
                            DB.TagSQLs.Attach(obj);
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
                DAL_AMCPE.TagSQL data;
                if (id == 0)
                {
                    data = (from a in DB.TagSQLs
                            where a.Name == name
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.TagSQLs
                            where a.Name == name && a.Id != id
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