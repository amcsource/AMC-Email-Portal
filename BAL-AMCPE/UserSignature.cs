using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class UserSignature : DAL_AMCPE.UserSignature
    {
        public DAL_AMCPE.UserSignature obj;

        public List<Signatures> GetUserSignatures()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.UserSignatures.Where(a => a.IsDeleted == false).Select(a => new Signatures()
                {
                    Id = a.Id,
                    Name = a.Name,
                    UserId = a.UserId,
                    CreatedBy = a.CreatedBy,
                    UpdatedBy = a.UpdatedBy
                }).ToList();
            }
        }

        public DAL_AMCPE.UserSignature GetUserSignatureByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.UserSignatures.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
        }

        public DAL_AMCPE.UserSignature GetUserSignatureByUserID(string userId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.UserSignatures.Where(a => a.IsDeleted == false && a.UserId == userId).FirstOrDefault();
            }
        }

        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (obj.Id == 0)
                    {
                        DB.UserSignatures.AddObject(obj);
                    }
                    else
                    {
                        DB.UserSignatures.Attach(obj);
                        DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                    }
                    DB.SaveChanges();
                    return obj.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public class Signatures
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
