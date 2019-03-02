using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class AddEditMasterTemplates : System.Web.UI.Page
{
    public int Id, responseCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Id = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Id = Convert.ToInt32(Request.QueryString["Id"]);
                GetTemplateById(Id);
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

    private void GetTemplateById(int id)
    {
        BAL_AMCPE.MasterTemplates mt = new BAL_AMCPE.MasterTemplates();
        mt.obj = mt.GetTemplateByID(id);
        txtTemplateName.Text = mt.obj.Name;
        txtTemplateHeader.Text = mt.obj.Header;
        txtTemplateFooter.Text = mt.obj.Footer;

        // Enable Submit button ccording user permission
        if ((Convert.ToString(Session["UserId"]).ToLower() == mt.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditTemplate) || (Convert.ToString(Session["UserId"]).ToLower() != mt.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditOtherTemplate))
            btnSubmit.Enabled = true;
        else
            btnSubmit.Enabled = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.MasterTemplates mt = new BAL_AMCPE.MasterTemplates();

        if (Id == 0)
        {
            mt.obj = new DAL_AMCPE.MasterTemplate();
            mt.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            mt.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            mt.obj = mt.GetTemplateByID(Id);
            mt.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            mt.obj.UpdatedOn = DateTime.Now;
        }

        mt.obj.Name = txtTemplateName.Text.Trim();
        mt.obj.Header = txtTemplateHeader.Text.Trim();
        mt.obj.Footer = txtTemplateFooter.Text.Trim();
        mt.obj.DeletedBy = "";
        //mt.obj.DeletedOn = DateTime.Now;
        mt.obj.IsDeleted = false;

        responseCode = mt.Save();

        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            Response.Redirect("MasterTemplates.aspx");
        }

        //if (Id == 0)
        //    mt.AddTemplate();
        //else
        //    mt.UpdateMasterTemplate();

        
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MasterTemplates.aspx");
    }
}