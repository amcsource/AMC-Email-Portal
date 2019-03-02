using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class AddEditQuery : System.Web.UI.Page
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
                GetQueryById(Id);
            }
            hdnId.Value = Id.ToString();
        }
        else
        {
            Id = Convert.ToInt32(hdnId.Value);
        }

        // Enable Submit button according user permission
        if (Id == 0)
            btnSubmit.Enabled = PermissionSession.UserPermission.CanCreateSQLQuery;
    }

    private void GetQueryById(int id)
    {
        BAL_AMCPE.SQLQueries sq = new BAL_AMCPE.SQLQueries();
        sq.obj = sq.GetSQLQueryByID(id);
        txtName.Text = sq.obj.Name;
        txtSQLQuery.Text = sq.obj.Query;

        //// Enable Submit button according user permission
        //btnSubmit.Enabled = PermissionSession.CanEditSQLQuery;

        // Enable Submit button according user permission
        if ((Convert.ToString(Session["UserId"]).ToLower() == sq.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditSQLQuery) || (Convert.ToString(Session["UserId"]).ToLower() != sq.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditOtherSQLQuery))
            btnSubmit.Enabled = true;
        else
            btnSubmit.Enabled = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.SQLQueries sq = new BAL_AMCPE.SQLQueries();

        if (Id == 0)
        {
            sq.obj = new DAL_AMCPE.SQLQuery();
            sq.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            sq.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            sq.obj = sq.GetSQLQueryByID(Id);
            sq.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            sq.obj.UpdatedOn = DateTime.Now;
        }
        sq.obj.Name = txtName.Text.Trim();
        sq.obj.Query = txtSQLQuery.Text.Trim();

        sq.obj.DeletedBy = "";
        //sq.obj.DeletedOn = DateTime.Now;
        sq.obj.IsDeleted = false;

        responseCode = sq.Save();
        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            Response.Redirect("Queries.aspx");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Queries.aspx");
    }
}