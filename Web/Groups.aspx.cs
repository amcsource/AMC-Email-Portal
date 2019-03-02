using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using DAL_AMCPE;

public partial class Groups : System.Web.UI.Page
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
        if (Session["dataction"] != null && Convert.ToString(Session["dataction"]) == "d")
        {
            lblMessage.Text = "Data deleted successfully!!";
            message.Visible = true;
            Session["dataction"] = null;
        }

        BAL_AMCPE.Groups g = new BAL_AMCPE.Groups();
        var data = g.GetGroups();
        if (data.Count > 0)
        {
            rptGroups.DataSource = g.GetGroups();
            ltlNoRecord.Visible = false;
        }
        else
        {
            rptGroups.DataSource = null;
            ltlNoRecord.Visible = true;
        }
        rptGroups.DataBind();
    }

    protected void rptGroups_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.Groups g = new BAL_AMCPE.Groups();
            g.obj = g.GetGroupsByID(Convert.ToInt32(e.CommandArgument));
            g.Delete();
            
            Session["dataction"] = "d";
            BindData();
        }
    }
    protected void rptGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string user = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "CreatedBy")).ToLower();
            string currentUser = Convert.ToString(Session["UserId"]).ToLower();

            LinkButton lbEdit = (LinkButton)e.Item.FindControl("Edit");
            lbEdit.Enabled = ((user == currentUser && PermissionSession.UserPermission.CanEditGroup) || (user != currentUser && PermissionSession.UserPermission.CanEditOtherGroup));

            LinkButton lbDelete = (LinkButton)e.Item.FindControl("Delete");
            //lbDelete.Enabled = ((user == currentUser && PermissionSession.CanDeleteGroup) || (user != currentUser && PermissionSession.CanDeleteOtherGroup));
            if ((user == currentUser && PermissionSession.UserPermission.CanDeleteGroup) || (user != currentUser && PermissionSession.UserPermission.CanDeleteOtherGroup))
                lbDelete.Enabled = true;
            else
            {
                lbDelete.Enabled = false;
                lbDelete.OnClientClick = null;
            }

            LinkButton lbUser = (LinkButton)e.Item.FindControl("User");
            lbUser.Enabled = PermissionSession.UserPermission.CanCreateGroup;
        }
    }
}