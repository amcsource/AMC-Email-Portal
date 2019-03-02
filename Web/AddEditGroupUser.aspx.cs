using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class AddEditGroupUser : System.Web.UI.Page
{
    public int Id, GroupId, responseCode;
    public string returnto;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Id = GroupId = 0;
            returnto = "";
            BindGroups();
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Id = Convert.ToInt32(Request.QueryString["Id"]);
                GetUserByRowId();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["GroupId"]))
            {
                GroupId = Convert.ToInt32(Request.QueryString["GroupId"]);
                ddlGroups.SelectedValue = Convert.ToString(GroupId);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["return"]))
            {
                returnto = Convert.ToString(Request.QueryString["return"]);
            }

            hdnId.Value = Id.ToString();
            hdnGroupId.Value = GroupId.ToString();
            hdnReturnTo.Value = returnto;
        }
        else
        {
            Id = Convert.ToInt32(hdnId.Value);
            GroupId = Convert.ToInt32(hdnGroupId.Value);
            returnto = hdnReturnTo.Value;
        }

        // Enable Submit button according user permission
        if (Id == 0)
            btnSubmit.Enabled = PermissionSession.UserPermission.CanCreateGroup;
    }

    private void BindGroups()
    {
        BAL_AMCPE.Groups g = new BAL_AMCPE.Groups();
        ddlGroups.DataSource = g.GetGroups();
        ddlGroups.DataBind();
        ddlGroups.DataTextField = "GroupName";
        ddlGroups.DataValueField = "Id";
        ddlGroups.DataBind();
        ddlGroups.Items.Insert(0, new ListItem("Choose group", "0"));
    }

    private void GetUserByRowId()
    {
        BAL_AMCPE.UserGroup ug = new BAL_AMCPE.UserGroup();
        ug.obj = ug.GetUserByRowId(Id);
        if (ug.obj != null)
        {
            ddlGroups.SelectedValue = Convert.ToString(ug.obj.GroupId);
            txtUserName.Text = ug.obj.UserId;
        }

        //// Enable Submit button according user permission
        //btnSubmit.Enabled = PermissionSession.CanEditGroup;

        // Enable Submit button according user permission
        if ((Convert.ToString(Session["UserId"]).ToLower() == ug.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditGroup) || (Convert.ToString(Session["UserId"]).ToLower() != ug.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditOtherGroup))
            btnSubmit.Enabled = true;
        else
            btnSubmit.Enabled = false;

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.UserGroup ug = new BAL_AMCPE.UserGroup();

        if (Id == 0)
        {
            ug.obj = new DAL_AMCPE.UsersInGroup();
            ug.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            ug.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            ug.obj = ug.GetUserByRowId(Id);
            ug.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            ug.obj.UpdatedOn = DateTime.Now;
        }
        ug.obj.GroupId = Convert.ToInt32(ddlGroups.SelectedValue);
        ug.obj.UserId = txtUserName.Text.Trim();

        responseCode = ug.Save();
        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same User Id already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            if (returnto=="groupuser")
                Response.Redirect("GroupUsers.aspx");
            else
                Response.Redirect("Groups.aspx");
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (returnto == "groupuser")
            Response.Redirect("GroupUsers.aspx");
        else
            Response.Redirect("Groups.aspx");
    }
}