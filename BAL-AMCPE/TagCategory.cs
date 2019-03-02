using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class TagCategory
    {
        public DAL_AMCPE.TagCategory obj;
        //public Permission permissionSet;

        public List<DAL_AMCPE.TagCategory> GetTagCategories()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.TagCategories.Where(a => a.IsDeleted == false).OrderBy(a => a.CategoryName).ToList();
            }
        }



        public DAL_AMCPE.TagCategory GetTagCategoryByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.TagCategories.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
        }


        public List<ColumnName> GetTagSQLNamesByTagCategoryId(int categoryId)
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    return (from l in DB.TagSQLs
                            where l.TagCategoryId == categoryId && l.IsDeleted == false
                            orderby l.Name ascending
                            select new ColumnName
                            {
                                name = l.Name.Trim()
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (DoesAleardyExist(obj.Id, obj.CategoryName))
                        return -1;
                    else
                    {
                        if (obj.Id == 0)
                        {
                            DB.TagCategories.AddObject(obj);
                        }
                        else
                        {
                            DB.TagCategories.Attach(obj);
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
                DAL_AMCPE.TagCategory data;
                if (id == 0)
                {
                    data = (from a in DB.TagCategories
                            where a.CategoryName == name
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.TagCategories
                            where a.CategoryName == name && a.Id != id
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