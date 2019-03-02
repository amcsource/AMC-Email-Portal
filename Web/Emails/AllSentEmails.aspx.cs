using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using System.Collections;

public partial class Emails_AllSentEmails : System.Web.UI.Page
{
    public int pageSize = 20, PageRange = 5;
    public string sortExpression = "order by SentOn desc", status = "sent";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindFilters();
            BindData();
        }
    }

    private void BindFilters()
    {
        BAL_AMCPE.Emails em = new BAL_AMCPE.Emails();
        var data = em.GetEmailFilters();

        List<string> UserName = data.UserName.Split(',').ToList();
        List<string> TemplateName = data.TemplateName.Split(',').ToList();

        UserName.Sort();
        foreach(string name in UserName)
        {
            ddlUserName.Items.Add(new ListItem(name.Trim(), "amc\\" + name.Trim()));
        }
        ddlUserName.Items.Insert(0, new ListItem("All Users", ""));

        TemplateName.Sort();
        foreach (string name in TemplateName)
        {
            ddlEmailTemplate.Items.Add(new ListItem(name.Trim(), name.Trim()));
        }
        ddlEmailTemplate.Items.Insert(0, new ListItem("All Email Templates", ""));

    }

    private void BindData()
    {
        BAL_AMCPE.Emails em = new BAL_AMCPE.Emails();
        var data = em.GetEmailsByUserWithFilter(CurrentPage + 1, pageSize, sortExpression, txtSearch.Text.Trim(), status, ddlUserName.SelectedValue, txtPatientNumber.Text.Trim(), ddlEmailTemplate.SelectedValue);
        if (data.Count > 0)
        {
            rptList.DataSource = data;
            ltlNoRecord.Visible = false;
            TotalPages = Convert.ToInt32(data.FirstOrDefault().TotalPages);
        }
        else
        {
            rptList.DataSource = null;
            ltlNoRecord.Visible = true;
            TotalPages = 0;
        }
        rptList.DataBind();
        if (TotalPages > 1)
            BindRptPagination(TotalPages);
        else
            pnlPagination.Visible = false;
    }

    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.Emails em = new BAL_AMCPE.Emails();
            em.obj = em.GetEmailByEmailId(Convert.ToInt32(e.CommandArgument));
            em.obj.IsDeleted = true;
            em.Save();

            // TODO: Delete email related files as well from server
            Session["dataction"] = "d";
            BindData();
        }
    }

    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string user = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "SentBy")).ToLower();
            string currentUser = Convert.ToString(Session["UserId"]).ToLower();

            LinkButton lbDelete = (LinkButton)e.Item.FindControl("Delete");
            if ((user == currentUser && PermissionSession.UserPermission.CanDeleteEmail) || (user != currentUser && PermissionSession.UserPermission.CanDeleteOtherEmail))
                lbDelete.Enabled = true;
            else
            {
                lbDelete.Enabled = false;
                lbDelete.OnClientClick = null;
            }
        }
    }

    #region Pagination
    public Int16 CurrentPage
    {
        get
        {
            object o = ViewState["_CurrentPage"];
            if (o == null)
            {
                return 0;
            }
            else
            {
                return (Int16)o;
            }
        }
        set { ViewState["_CurrentPage"] = value; }
    }
    public Int32 TotalPages
    {
        get
        {
            object o = ViewState["_TotalPage"];
            if (o == null)
            {
                return 0;
            }
            else
            {
                return (Int32)o;
            }
        }
        set { ViewState["_TotalPage"] = value; }
    }

    protected void BindRptPagination(Int32 intPageCount)
    {
        pnlPagination.Visible = true;
        lblTotalPages.Text = intPageCount.ToString();


        //Show Page range
        //set page start-index and end-index to show numbers
        Int32 StartPageRange = CurrentPage - (CurrentPage % PageRange) + 1;
        Int32 EndPageIndex = default(Int16);
        if (StartPageRange + PageRange - 1 < intPageCount)
        {
            EndPageIndex = StartPageRange + PageRange - 1;
        }
        else
        {
            EndPageIndex = intPageCount;
        }
        ArrayList aryLst = new ArrayList();
        Int32 i = 0;
        for (i = StartPageRange; i <= EndPageIndex; i++)
        {
            aryLst.Add(i);
        }
        rptPagination.DataSource = aryLst;
        rptPagination.DataBind();

        if (i - 1 == (StartPageRange + PageRange - 1) & i - 1 < intPageCount)
        {
            lnkEllipses.Visible = true;
            lnkEllipses.CommandArgument = i.ToString();
        }
        else
        {
            lnkEllipses.Visible = false;
        }

        //set last pageindex to LastPage link
        lnkLast.CommandArgument = (intPageCount - 1).ToString();
        if (CurrentPage == 0)
        {
            lnkFirst.Enabled = false;
            lnkPrev.Enabled = false;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
        }
        else if (CurrentPage == intPageCount - 1)
        {
            lnkNext.Enabled = false;
            lnkLast.Enabled = false;
            lnkFirst.Enabled = true;
            lnkPrev.Enabled = true;
        }
        else
        {
            lnkFirst.Enabled = true;
            lnkPrev.Enabled = true;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;

        }
    }
    protected void rptPagination_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "page")
        {
            CurrentPage = Convert.ToInt16(Convert.ToInt16(e.CommandArgument) - 1);
            BindData();
        }
    }
    protected void lnkNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage += 1;
        BindData();
    }
    protected void lnkPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage -= 1;
        BindData();
    }
    protected void lnkFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = 0;
        BindData();
    }
    protected void lnkLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = Convert.ToInt16(Convert.ToInt16(lblTotalPages.Text) - 1);
        BindData();
    }
    protected void lnkEllipses_Click(object sender, System.EventArgs e)
    {
        CurrentPage = (short)((Convert.ToInt32(CurrentPage) / PageRange + 1) * PageRange);
        BindData();
    }
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CurrentPage = 0;
        BindData();
    }
}