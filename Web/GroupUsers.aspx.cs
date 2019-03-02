using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class GroupUsers : System.Web.UI.Page
{
    //public int groupId;
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

            //groupId = 0;
            //if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            //{
            //    groupId = Convert.ToInt32(Request.QueryString["Id"]);
            //}

            //hdnGroupId.Value = groupId.ToString();
        }
        //else
        //{
        //    groupId = Convert.ToInt32(hdnGroupId.Value);
        //}
        
    }

    private void BindData()
    {
        if (Convert.ToString(Session["dataction"]) == "d")
        {
            lblMessage.Text = "Data deleted successfully!!";
            message.Visible = true;
            Session["dataction"] = null;
        }

        BAL_AMCPE.UserGroup ug = new BAL_AMCPE.UserGroup();
        var data = ug.GetAllUsersInGroups();
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
            BAL_AMCPE.UserGroup ug = new BAL_AMCPE.UserGroup();
            ug.obj = ug.GetUserByRowId(Convert.ToInt32(e.CommandArgument));
            if (ug.RemoveUserFromGroup())
            {
                Session["dataction"] = "d";
                BindData();
                lblMessage2.Visible = false;
            }
            else
                lblMessage2.Visible = true;
        }
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Here it is assumed that the permissions for group users are same as for the groups

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
        }
    }
}