using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class UserSignatures : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            if (Session["dataction"] != null)
            {
                if (Convert.ToString(Session["dataction"]) == "s")
                    lblMessage.Text = "Data saved successfully!!";
                message.Visible = true;
            }
            Session["dataction"] = null;
        }
    }

    private void BindData()
    {
        if (Convert.ToString(Session["dataction"]) == "d")
        {
            lblMessage.Text = "Data deleted successfully!!";
            message.Visible = true;
            Session["dataction"] = null;
        }

        BAL_AMCPE.UserSignature us = new BAL_AMCPE.UserSignature();
        var data = us.GetUserSignatures();
        if (data.Count > 0)
        {
            rptTemplates.DataSource = data;
            norecord.Visible = false;
        }
        else
        {
            rptTemplates.DataSource = null;
            norecord.Visible = true;
        }
        rptTemplates.DataBind();
    }

    protected void rptTemplates_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.UserSignature us = new BAL_AMCPE.UserSignature();
            us.obj = us.GetUserSignatureByID(Convert.ToInt32(e.CommandArgument));
            us.obj.DeletedBy = Convert.ToString(Session["UserId"]);
            us.obj.DeletedOn = DateTime.Now;
            us.obj.IsDeleted = true;
            us.Save();
            Session["dataction"] = "d";
            BindData();
        }
    }
}