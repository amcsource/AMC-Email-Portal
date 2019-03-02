using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_AMCPE;

namespace BAL_AMCPE
{
    public class UserGroup
    {
        public UsersInGroup obj;

        public List<procGetUsersInGroupsByGroupId_Result> GetUsersInGroupsByGroupId(int groupId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetUsersInGroupsByGroupId(groupId).ToList();
            }
        }

        public List<procGetAllUsersInGroups_Result> GetAllUsersInGroups()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetAllUsersInGroups().ToList();
            }
        }

        public UsersInGroup GetUserByRowId(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.UsersInGroups.Where(a => a.Id == id).SingleOrDefault();
            }
        }

        public UsersInGroup GetUserByUserId(string userId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.UsersInGroups.Where(a => a.UserId == userId).SingleOrDefault();
            }
        }

        public int Save()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (DoesAleardyExist(obj.Id, obj.UserId))
                        return -1;
                    else
                    {
                        if (obj.Id == 0)
                        {
                            DB.UsersInGroups.AddObject(obj);
                        }
                        else
                        {
                            DB.UsersInGroups.Attach(obj);
                            DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                        }
                        DB.SaveChanges();
                        return obj.Id;
                    }
                }
            }
            catch(Exception ex)
            {
                return 0;
            }
        }


        private bool DoesAleardyExist(int id, string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                if (id == 0)
                {
                    var data = (from a in DB.UsersInGroups
                                where a.UserId == name
                                select a).SingleOrDefault();
                    if (data != null)
                        return true;
                    else
                        return false;
                }
                else
                {
                    var data = (from a in DB.UsersInGroups
                                where a.UserId == name && a.Id != id
                                select a).SingleOrDefault();
                    if (data != null)
                        return true;
                    else
                        return false;
                }

            }
        }


        public bool RemoveUserFromGroup()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.UsersInGroups.Attach(obj);
                    DB.UsersInGroups.DeleteObject(obj);
                    DB.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SetPermissionForUser(string userId)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                UserPermissions permission = new UserPermissions();
                var data = DB.procGetPermissionForUser(userId).SingleOrDefault();
                if (data != null)
                {
                    permission.CanCreateGroup = data.CanCreateGroup.Value;
                    permission.CanEditGroup = data.CanEditGroup.Value;
                    permission.CanDeleteGroup = data.CanDeleteGroup.Value;
                    permission.CanEditOtherGroup = data.CanEditOtherGroup.Value;
                    permission.CanDeleteOtherGroup = data.CanDeleteOtherGroup.Value;
                    
                    permission.CanCreateTemplate = data.CanCreateTemplate.Value;
                    permission.CanEditTemplate = data.CanEditTemplate.Value;
                    permission.CanDeleteTemplate = data.CanDeleteTemplate.Value;
                    permission.CanEditOtherTemplate = data.CanEditOtherTemplate.Value;
                    permission.CanDeleteOtherTemplate = data.CanDeleteOtherTemplate.Value;
                    
                    permission.CanSendEmail = data.CanSendEmail.Value;
                    permission.CanDeleteEmail = data.CanDeleteEmail.Value;
                    permission.CanDeleteOtherEmail = data.CanDeleteOtherEmail.Value;
                    
                    permission.CanCreateSQLQuery = data.CanCreateSQLQuery.Value;
                    permission.CanEditSQLQuery = data.CanEditSQLQuery.Value;
                    permission.CanDeleteSQLQuery = data.CanDeleteSQLQuery.Value;
                    permission.CanEditOtherSQLQuery = data.CanEditOtherSQLQuery.Value;
                    permission.CanDeleteOtherSQLQuery = data.CanDeleteOtherSQLQuery.Value;
                }
                else
                {
                    permission.CanCreateGroup = false;
                    permission.CanEditGroup = false;
                    permission.CanDeleteGroup = false;
                    permission.CanEditOtherGroup = false;
                    permission.CanDeleteOtherGroup = false;
                    
                    permission.CanCreateTemplate = false;
                    permission.CanEditTemplate = false;
                    permission.CanDeleteTemplate = false;
                    permission.CanEditOtherTemplate = false;
                    permission.CanDeleteOtherTemplate = false;
                    
                    permission.CanSendEmail = false;
                    permission.CanDeleteEmail = false;
                    permission.CanDeleteOtherEmail = false;
                    
                    permission.CanCreateSQLQuery = false;
                    permission.CanEditSQLQuery = false;
                    permission.CanDeleteSQLQuery = false;
                    permission.CanEditOtherSQLQuery = false;
                    permission.CanDeleteOtherSQLQuery = false;
                }

                PermissionSession.UserPermission = permission;
            }
        }
    }
}