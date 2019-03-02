using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["UserId"]))
            {
                //Check if user logs in with other user name while he is already loged in with other name if then deny access
                if (Session["UserId"] != null && (Convert.ToString(Session["UserId"]).ToLower() != Convert.ToString(Request.QueryString["UserId"]).ToLower()))
                {
                    Response.Redirect("~/SessionExpired.aspx?reason=duplicateuser");
                }
                else // allow as normal
                {
                    Session["UserId"] = Convert.ToString(Request.QueryString["UserId"]);
                    ltlUserName.Text = Convert.ToString(Request.QueryString["UserId"]);

                    //Enable operations as per User permisssion
                    BAL_AMCPE.UserGroup obj = new BAL_AMCPE.UserGroup();
                    var data = obj.GetUserByUserId(Convert.ToString(Session["UserId"]));

                    if (data != null)
                    {
                        obj.SetPermissionForUser(Convert.ToString(Session["UserId"]));
                    }
                    else
                        Response.Redirect("~/SessionExpired.aspx?reason=nouser");

                    imgGroups.Enabled = (PermissionSession.UserPermission.CanCreateGroup || PermissionSession.UserPermission.CanEditGroup || PermissionSession.UserPermission.CanDeleteGroup);
                    imgTemplates.Enabled = (PermissionSession.UserPermission.CanCreateTemplate || PermissionSession.UserPermission.CanEditTemplate || PermissionSession.UserPermission.CanDeleteTemplate || PermissionSession.UserPermission.CanEditOtherTemplate || PermissionSession.UserPermission.CanDeleteOtherTemplate);

                    //commented as ahmz said to remove this tab, but do not remove permanemtly for future reference
                    //imgQueries.Enabled = (PermissionSession.UserPermission.CanCreateSQLQuery || PermissionSession.UserPermission.CanEditSQLQuery || PermissionSession.UserPermission.CanDeleteSQLQuery);
                    imgEmails.Enabled = (PermissionSession.UserPermission.CanSendEmail);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["PatientNumber"]))
                    Session["PatientNumber"] = Convert.ToString(Request.QueryString["PatientNumber"]);

                if (!string.IsNullOrEmpty(Request.QueryString["RecId"]))
                    Session["RecId"] = Convert.ToString(Request.QueryString["RecId"]);

            }
            else
            {
                Response.Redirect("~/SessionExpired.aspx");
            }
        }
        else
        { 
            if(Session["UserId"] == null)
                Response.Redirect("~/SessionExpired.aspx");
        }
    }
}