using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class EmailTemplateCategory
    {
        public DAL_AMCPE.EmailTemplateCategory obj;
        //public Permission permissionSet;

        public List<DAL_AMCPE.EmailTemplateCategory> GetEmailTemplateCategories()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailTemplateCategories.Where(a => a.IsDeleted == false).OrderBy(a => a.CategoryName).ToList();
            }
        }

        public List<DAL_AMCPE.EmailTemplateCategory> GetEmailTemplateCategoriesNew()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailTemplateCategories.Where(a => a.IsDeleted == false).OrderBy(a => a.CategoryName).ToList();
            }
        }
        
       


        public DAL_AMCPE.EmailTemplateCategory GetEmailTemplateCategoryByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.EmailTemplateCategories.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
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
                            DB.EmailTemplateCategories.AddObject(obj);
                        }
                        else
                        {
                            DB.EmailTemplateCategories.Attach(obj);
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
                DAL_AMCPE.EmailTemplateCategory data;
                if (id == 0)
                {
                    data = (from a in DB.EmailTemplateCategories
                            where a.CategoryName == name && a.IsDeleted == false
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.EmailTemplateCategories
                            where a.CategoryName == name && a.Id != id && a.IsDeleted == false
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