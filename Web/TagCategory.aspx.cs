using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class TagCategory : System.Web.UI.Page
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

        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
        var data = tc.GetTagCategories();
        if (data.Count > 0)
        {
            rptList.DataSource = data;
            message.Visible = false;
        }
        else
        {
            rptList.DataSource = null;
            message.Visible = true;
            lblMessage.Text = "No record found";
        }
        rptList.DataBind();
    }

    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
            tc.obj = tc.GetTagCategoryByID(Convert.ToInt32(e.CommandArgument));
            tc.obj.IsDeleted = true;
            tc.Save();
            Session["dataction"] = "d";
            BindData();
        }
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string currentUser = Convert.ToString(Session["UserId"]).ToLower();

            LinkButton lbEdit = (LinkButton)e.Item.FindControl("Edit");
            lbEdit.Enabled = (currentUser == "amc\\ahmz");

            LinkButton lbDelete = (LinkButton)e.Item.FindControl("Delete");
            //lbDelete.Enabled = ((user == currentUser && PermissionSession.CanDeleteSQLQuery) || (user != currentUser && PermissionSession.CanDeleteOtherSQLQuery)); //PermissionSession.CanDeleteSQLQuery;
            if (currentUser == "amc\\ahmz")
                lbDelete.Enabled = true;
            else
            {
                lbDelete.Enabled = false;
                lbDelete.OnClientClick = null;
            }
        }
    }
}