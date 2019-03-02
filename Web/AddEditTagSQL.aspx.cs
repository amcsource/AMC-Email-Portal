using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddEditTagSQL : System.Web.UI.Page
{
    public int id, responseCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetTagCategories();
            id = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                id = Convert.ToInt32(Request.QueryString["Id"]);
                GetTagSQLById(id);
            }
            hdnId.Value = id.ToString();
        }
        else
        {
            id = Convert.ToInt32(hdnId.Value);
        }

        // Enable Submit button according user permission
        btnSubmit.Enabled = (Convert.ToString(Session["UserId"]) == "amc\\ahmz" ? true : false);
    }

    private void GetTagCategories()
    {
        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
        ddlTagCategory.DataSource = tc.GetTagCategories();
        ddlTagCategory.DataTextField = "CategoryName";
        ddlTagCategory.DataValueField = "Id";
        ddlTagCategory.DataBind();
        ddlTagCategory.Items.Insert(0, new ListItem("Choose Category", "0"));
    }

    private void GetTagSQLById(int id)
    {
        BAL_AMCPE.TagSQL ts = new BAL_AMCPE.TagSQL();
        ts.obj = ts.GetTagSQLByID(id);

        ddlTagCategory.SelectedValue = Convert.ToString(ts.obj.TagCategoryId);
        txtName.Text = ts.obj.Name;
        txtTaqQuery.Text = ts.obj.Query;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.TagSQL ts = new BAL_AMCPE.TagSQL();

        if (id == 0)
        {
            ts.obj = new DAL_AMCPE.TagSQL();
            ts.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            ts.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            ts.obj = ts.GetTagSQLByID(id);
            ts.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            ts.obj.UpdatedOn = DateTime.Now;
        }

        ts.obj.TagCategoryId = Convert.ToInt32(ddlTagCategory.SelectedValue);
        ts.obj.Name = txtName.Text.Trim();
        ts.obj.Query = txtTaqQuery.Text;
        ts.obj.IsDeleted = false;

        responseCode = ts.Save();
        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            Response.Redirect("TagSQL.aspx");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("TagSQL.aspx");
    }
}