using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using DAL_AMCPE;

public partial class AddEditGroup : System.Web.UI.Page
{
    public int groupId, responseCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            groupId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                groupId = Convert.ToInt32(Request.QueryString["Id"]);
                GetGroupById(groupId);
            }
            hdnId.Value = groupId.ToString();
        }
        else
        {
            groupId = Convert.ToInt32(hdnId.Value);
        }

        // Enable Submit button according user permission
        if (groupId == 0)
            btnSubmit.Enabled = PermissionSession.UserPermission.CanCreateGroup;

    }

    private void GetGroupById(int gId)
    {
        BAL_AMCPE.Groups g = new BAL_AMCPE.Groups();
        g.obj = g.GetGroupsByID(gId);
        txtGroupName.Text = g.obj.GroupName;

        BAL_AMCPE.Permissions p = new Permissions();
        p.obj = p.GetPermissionByGroupID(gId);
        if (p.obj != null)
        {
            chkPermissions.Items[0].Selected = Convert.ToBoolean(p.obj.CanCreateGroup);
            chkPermissions.Items[1].Selected = Convert.ToBoolean(p.obj.CanEditGroup);
            chkPermissions.Items[2].Selected = Convert.ToBoolean(p.obj.CanDeleteGroup);
            chkPermissions.Items[3].Selected = Convert.ToBoolean(p.obj.CanEditOtherGroup);
            chkPermissions.Items[4].Selected = Convert.ToBoolean(p.obj.CanDeleteOtherGroup);

            //chkPermissions.Items[5].Selected = Convert.ToBoolean(p.obj.CanCreateSQLQuery);
            //chkPermissions.Items[6].Selected = Convert.ToBoolean(p.obj.CanEditSQLQuery);
            //chkPermissions.Items[7].Selected = Convert.ToBoolean(p.obj.CanDeleteSQLQuery);
            //chkPermissions.Items[8].Selected = Convert.ToBoolean(p.obj.CanEditOtherSQLQuery);
            //chkPermissions.Items[9].Selected = Convert.ToBoolean(p.obj.CanDeleteOtherSQLQuery);

            chkPermissions.Items[5].Selected = Convert.ToBoolean(p.obj.CanCreateTemplate);
            chkPermissions.Items[6].Selected = Convert.ToBoolean(p.obj.CanEditTemplate);
            chkPermissions.Items[7].Selected = Convert.ToBoolean(p.obj.CanDeleteTemplate);
            chkPermissions.Items[8].Selected = Convert.ToBoolean(p.obj.CanEditOtherTemplate);
            chkPermissions.Items[9].Selected = Convert.ToBoolean(p.obj.CanDeleteOtherTemplate);
            
            chkPermissions.Items[10].Selected = Convert.ToBoolean(p.obj.CanSendEmail);
            chkPermissions.Items[11].Selected = Convert.ToBoolean(p.obj.CanDeleteEmail);
            chkPermissions.Items[12].Selected = Convert.ToBoolean(p.obj.CanDeleteOtherEmail);
        }

        //// Enable Submit button according user permission
        //btnSubmit.Enabled = PermissionSession.CanEditGroup;

        // Enable Submit button according user permission
        if ((Convert.ToString(Session["UserId"]).ToLower() == g.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditGroup) || (Convert.ToString(Session["UserId"]).ToLower() != g.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditOtherGroup))
            btnSubmit.Enabled = true;
        else
            btnSubmit.Enabled = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.Groups g = new BAL_AMCPE.Groups();
        BAL_AMCPE.Permissions p = new BAL_AMCPE.Permissions();

        if (groupId == 0)
        {
            g.obj = new Group();
            g.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            g.obj.CreatedOn = DateTime.Now;
            g.permissionSet = new Permission();
        }
        else
        {
            g.obj = g.GetGroupsByID(groupId);
            g.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            g.obj.UpdatedOn = DateTime.Now;
            g.permissionSet = p.GetPermissionByGroupID(groupId);
        }
        g.obj.GroupName = txtGroupName.Text.Trim();
        g.obj.IsDeleted = false;

        g.permissionSet.CanCreateGroup = Convert.ToBoolean(chkPermissions.Items[0].Selected);
        g.permissionSet.CanEditGroup = Convert.ToBoolean(chkPermissions.Items[1].Selected);
        g.permissionSet.CanDeleteGroup = Convert.ToBoolean(chkPermissions.Items[2].Selected);
        g.permissionSet.CanEditOtherGroup = Convert.ToBoolean(chkPermissions.Items[3].Selected);
        g.permissionSet.CanDeleteOtherGroup = Convert.ToBoolean(chkPermissions.Items[4].Selected);

        g.permissionSet.CanCreateSQLQuery = false; //Convert.ToBoolean(chkPermissions.Items[5].Selected);
        g.permissionSet.CanEditSQLQuery = false; // Convert.ToBoolean(chkPermissions.Items[6].Selected);
        g.permissionSet.CanDeleteSQLQuery = false; // Convert.ToBoolean(chkPermissions.Items[7].Selected);
        g.permissionSet.CanEditOtherSQLQuery = false; // Convert.ToBoolean(chkPermissions.Items[8].Selected);
        g.permissionSet.CanDeleteOtherSQLQuery = false; // Convert.ToBoolean(chkPermissions.Items[9].Selected);
        
        g.permissionSet.CanCreateTemplate = Convert.ToBoolean(chkPermissions.Items[5].Selected);
        g.permissionSet.CanEditTemplate = Convert.ToBoolean(chkPermissions.Items[6].Selected);
        g.permissionSet.CanDeleteTemplate = Convert.ToBoolean(chkPermissions.Items[7].Selected);
        g.permissionSet.CanEditOtherTemplate = Convert.ToBoolean(chkPermissions.Items[8].Selected);
        g.permissionSet.CanDeleteOtherTemplate = Convert.ToBoolean(chkPermissions.Items[9].Selected);
        
        g.permissionSet.CanSendEmail = Convert.ToBoolean(chkPermissions.Items[10].Selected);
        g.permissionSet.CanDeleteEmail = Convert.ToBoolean(chkPermissions.Items[11].Selected);
        g.permissionSet.CanDeleteOtherEmail = Convert.ToBoolean(chkPermissions.Items[12].Selected);

        g.permissionSet.IsDeleted = false;



        responseCode = g.Save();
        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            Response.Redirect("Groups.aspx");
        }


    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Groups.aspx");
    }
}