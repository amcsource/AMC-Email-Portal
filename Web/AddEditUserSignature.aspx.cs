using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class AddEditUserSignature : System.Web.UI.Page
{
    public int Id, responseCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Id = 0;
            BindUsers();

            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Id = Convert.ToInt32(Request.QueryString["Id"]);
                GetSignatureById(Id);
            }

            hdnId.Value = Id.ToString();
        }
        else
        {
            Id = Convert.ToInt32(hdnId.Value);
        }

        // Enable Submit button ccording user permission
        if (Id == 0)
            btnSubmit.Enabled = PermissionSession.UserPermission.CanCreateTemplate;
    }

    private void BindUsers()
    {
        UserGroup ug = new UserGroup();
        ddlUsers.DataSource = ug.GetAllUsersInGroups();
        ddlUsers.DataTextField = "UserId";
        ddlUsers.DataValueField = "UserId";
        ddlUsers.DataBind();
        ddlUsers.Items.Insert(0, new ListItem("Please select user", "0"));
    }

    private void GetSignatureById(int id)
    {
        BAL_AMCPE.UserSignature us = new BAL_AMCPE.UserSignature();
        us.obj = us.GetUserSignatureByID(Id);
        txtName.Text = us.obj.Name;
        ddlUsers.SelectedValue = us.obj.UserId;
        txtSignature.Text = us.obj.Signature;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.UserSignature us = new BAL_AMCPE.UserSignature();

        if (Id == 0)
        {
            us.obj = new DAL_AMCPE.UserSignature();
            us.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            us.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            us.obj = us.GetUserSignatureByID(Id);
            us.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            us.obj.UpdatedOn = DateTime.Now;
        }

        us.obj.Name = txtName.Text.Trim();
        us.obj.UserId = ddlUsers.SelectedValue;
        us.obj.Signature = txtSignature.Text.Trim();
        us.obj.DeletedBy = "";
        us.obj.IsDeleted = false;

        responseCode = us.Save();

        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            Response.Redirect("UserSignatures.aspx");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserSignatures.aspx");
    }
}