using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddEditDoctor : System.Web.UI.Page
{
    public int id, responseCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            id = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                id = Convert.ToInt32(Request.QueryString["Id"]);
                GetDoctorById(id);
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

    private void GetDoctorById(int id)
    {
        BAL_AMCPE.Doctors d = new BAL_AMCPE.Doctors();
        d.obj = d.GetDoctorByID(id);

        txtName.Text = d.obj.DoctorName;
        if (!string.IsNullOrWhiteSpace(d.obj.ImageName))
        {
            imgDocImage.ImageUrl = "~/Uploads/" + d.obj.ImageName;
            imgDocImage.Visible = true;
        }
        else
        {
            imgDocImage.Visible = false;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.Doctors d = new BAL_AMCPE.Doctors();

        if (id == 0)
        {
            d.obj = new DAL_AMCPE.Doctor();
            d.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            d.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            d.obj = d.GetDoctorByID(id);
            d.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            d.obj.UpdatedOn = DateTime.Now;
        }

        
        d.obj.DoctorName = txtName.Text.Trim();
        if (fileImage.HasFile)
        {
            try
            {
                string filename = Guid.NewGuid().ToString().Replace("-", "");
                var FileExtension = Path.GetExtension(fileImage.PostedFile.FileName);
                fileImage.SaveAs(Server.MapPath("~/Uploads/") + filename + FileExtension);

                d.obj.ImageName = filename + FileExtension;
            }
            catch (Exception ex) { }
            
        }
        d.obj.IsDeleted = false;

        responseCode = d.Save();
        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            Session["dataction"] = "s";
            Response.Redirect("Doctors.aspx");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Doctors.aspx");
    }
}