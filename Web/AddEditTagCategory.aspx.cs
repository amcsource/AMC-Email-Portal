using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using DAL_AMCPE;

public partial class AddEditTagCategory : System.Web.UI.Page
{
    public int categoryId, responseCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            categoryId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                categoryId = Convert.ToInt32(Request.QueryString["Id"]);
                GetCategoryById(categoryId);
            }
            hdnCategoryId.Value = categoryId.ToString();
        }
        else
        {
            categoryId = Convert.ToInt32(hdnCategoryId.Value);
        }

        // Enable Submit button according user permission
        btnSubmit.Enabled = (Convert.ToString(Session["UserId"]) == "amc\\ahmz"? true : false);
    }

    private void GetCategoryById(int id)
    {
        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
        tc.obj = tc.GetTagCategoryByID(id);
        txtCategoryName.Text = tc.obj.CategoryName;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();

        if (categoryId == 0)
        {
            tc.obj = new DAL_AMCPE.TagCategory();
            tc.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            tc.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            tc.obj = tc.GetTagCategoryByID(categoryId);
            tc.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            tc.obj.UpdatedOn = DateTime.Now;
        }
        
        tc.obj.CategoryName = txtCategoryName.Text.Trim();
        tc.obj.IsDeleted = false;

        responseCode = tc.Save();
        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            Response.Redirect("TagCategory.aspx");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("TagCategory.aspx");
    }
}