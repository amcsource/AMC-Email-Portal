using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL_AMCPE
{
    public class PermissionSession
    {

        public static UserPermissions UserPermission
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["UserPermissions"] != null)
                    return System.Web.HttpContext.Current.Session["UserPermissions"] as UserPermissions;
                else
                    return null;
            }
            set
            {
                System.Web.HttpContext.Current.Session["UserPermissions"] = value;
            }
        }
    }

    public class UserPermissions
    {
        public bool CanCreateGroup { get; set; }
        public bool CanEditGroup { get; set; }
        public bool CanDeleteGroup { get; set; }
        public bool CanEditOtherGroup { get; set; }
        public bool CanDeleteOtherGroup { get; set; }

        public bool CanCreateTemplate { get; set; }
        public bool CanEditTemplate { get; set; }
        public bool CanDeleteTemplate { get; set; }
        public bool CanEditOtherTemplate { get; set; }
        public bool CanDeleteOtherTemplate { get; set; }

        public bool CanSendEmail { get; set; }
        public bool CanDeleteEmail { get; set; }
        public bool CanDeleteOtherEmail { get; set; }

        public bool CanCreateSQLQuery { get; set; }
        public bool CanEditSQLQuery { get; set; }
        public bool CanDeleteSQLQuery { get; set; }
        public bool CanEditOtherSQLQuery { get; set; }
        public bool CanDeleteOtherSQLQuery { get; set; }
    }
}
