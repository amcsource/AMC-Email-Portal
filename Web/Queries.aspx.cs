using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class Queries : System.Web.UI.Page
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

        BAL_AMCPE.SQLQueries sq = new BAL_AMCPE.SQLQueries();
        var data = sq.GetSQLQueries();
        if (data.Count > 0)
        {
            rptList.DataSource = data;
            ltlNoRecord.Visible = false;
        }
        else
        {
            rptList.DataSource = null;
            ltlNoRecord.Visible = true;
        }
        rptList.DataBind();
    }

    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.SQLQueries sq = new BAL_AMCPE.SQLQueries();
            sq.obj = sq.GetSQLQueryByID(Convert.ToInt32(e.CommandArgument));
            sq.obj.DeletedBy = Convert.ToString(Session["UserId"]);
            sq.obj.DeletedOn = DateTime.Now;
            sq.obj.IsDeleted = true;
            sq.Save();
            Session["dataction"] = "d";
            BindData();
        }
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string user = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "CreatedBy")).ToLower();
            string currentUser = Convert.ToString(Session["UserId"]).ToLower();

            LinkButton lbEdit = (LinkButton)e.Item.FindControl("Edit");
            lbEdit.Enabled = ((user == currentUser && PermissionSession.UserPermission.CanEditSQLQuery) || (user != currentUser && PermissionSession.UserPermission.CanEditOtherSQLQuery)); //PermissionSession.CanEditSQLQuery;

            LinkButton lbDelete = (LinkButton)e.Item.FindControl("Delete");
            //lbDelete.Enabled = ((user == currentUser && PermissionSession.CanDeleteSQLQuery) || (user != currentUser && PermissionSession.CanDeleteOtherSQLQuery)); //PermissionSession.CanDeleteSQLQuery;
            if ((user == currentUser && PermissionSession.UserPermission.CanDeleteSQLQuery) || (user != currentUser && PermissionSession.UserPermission.CanDeleteOtherSQLQuery))
                lbDelete.Enabled = true;
            else
            {
                lbDelete.Enabled = false;
                lbDelete.OnClientClick = null;
            }
        }
    }
}