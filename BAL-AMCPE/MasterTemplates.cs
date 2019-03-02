using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class MasterTemplates
    {
        public MasterTemplate obj;

        public List<MasterTemplate> GetMasterTemplates()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.MasterTemplates.Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
            }
        }

        public List<procGetMasterTemplates_Result> GetMasterTemplates(int pageIndex, int pageSize, string sortExpression, string searchKeyword)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetMasterTemplates(pageIndex, pageSize, sortExpression, searchKeyword).ToList();
            }
        }

        public MasterTemplate GetTemplateByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.MasterTemplates.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
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
                            DB.MasterTemplates.AddObject(obj);
                        }
                        else
                        {
                            DB.MasterTemplates.Attach(obj);
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

        private bool DoesAleardyExist(int id, string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                DAL_AMCPE.MasterTemplate data;
                if (id == 0)
                {
                    data = (from a in DB.MasterTemplates
                            where a.Name == name && a.IsDeleted == false
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.MasterTemplates
                            where a.Name == name && a.Id != id && a.IsDeleted == false
                            select a).SingleOrDefault();
                }

                if (data != null)
                    return true;
                else
                    return false;

            }
        }

        public int AddTemplate()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.MasterTemplates.AddObject(obj);
                    DB.SaveChanges();
                    return obj.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        
        public bool UpdateMasterTemplate()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.MasterTemplates.Attach(obj);
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
