using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    public string userId;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.QueryString["UserId"]) && Session["UserId"] == null)
        {
            Response.Redirect("~/SessionExpired.aspx");
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["UserId"]) || Session["UserId"] != null)
        {
            if (Request.QueryString["UserId"] != null)
                userId = Convert.ToString(Request.QueryString["userId"]);
            else
                userId = Convert.ToString(Session["userId"]);
            Session["UserId"] = userId;

            BAL_AMCPE.UserGroup obj = new BAL_AMCPE.UserGroup();
            var data = obj.GetUserByUserId(Convert.ToString(Session["UserId"]));
            if (data != null)
                obj.SetPermissionForUser(Convert.ToString(Session["UserId"]));
            else
                Response.Redirect("~/SessionExpired.aspx?reason=nouser");

            obj.SetPermissionForUser(userId);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!string.IsNullOrEmpty(Request.QueryString["UserId"]) || Session["UserId"] != null)
        //{
        //    if (Request.QueryString["UserId"] != null)
        //        userId = Convert.ToString(Request.QueryString["userId"]);
        //    else
        //        userId = Convert.ToString(Session["userId"]);
        //    Session["UserId"] = userId;
        //}

        if (!IsPostBack)
        {
            ltlUserName.Text = userId;
            //BAL_AMCPE.UserGroup obj = new BAL_AMCPE.UserGroup();
            //var data = obj.GetUserByUserId(Convert.ToString(Session["UserId"]));
            //if (data != null)
            //    obj.SetPermissionForUser(Convert.ToString(Session["UserId"]));
            //else
            //    Response.Redirect("~/SessionExpired.aspx?reason=nouser");

            //obj.SetPermissionForUser(userId);

            //Show operations as per User permisssion
            liGroups.Visible = (PermissionSession.UserPermission.CanCreateGroup || PermissionSession.UserPermission.CanEditGroup || PermissionSession.UserPermission.CanDeleteGroup);
            liTemplates.Visible = (PermissionSession.UserPermission.CanCreateTemplate || PermissionSession.UserPermission.CanEditTemplate || PermissionSession.UserPermission.CanDeleteTemplate || PermissionSession.UserPermission.CanEditOtherTemplate || PermissionSession.UserPermission.CanDeleteOtherTemplate);
            
            //commented as ahmz said to remove this tab, but do not remove permanemtly for future reference
            //liQueries.Visible = (PermissionSession.UserPermission.CanCreateSQLQuery || PermissionSession.UserPermission.CanEditSQLQuery || PermissionSession.UserPermission.CanDeleteSQLQuery);
            liEmails.Visible = (PermissionSession.UserPermission.CanSendEmail);

            //enable for Ahmz only
            liTags.Visible = liDoctors.Visible = (Convert.ToString(Session["UserId"]) == "amc\\ahmz" ? true : false);
        }
    }
}