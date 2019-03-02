using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using DAL_AMCPE;
using System.Collections;

public partial class TagSQL : System.Web.UI.Page
{
    public int pageSize = 20, PageRange = 5;
    public string sortExpression = "order by Name asc", searchKeyword = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetTagCategories();
            GetTagSQLs();
        }
    }

    private void GetTagCategories()
    {
        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
        ddlTagCategory.DataSource = tc.GetTagCategories();
        ddlTagCategory.DataTextField = "CategoryName";
        ddlTagCategory.DataValueField = "Id";
        ddlTagCategory.DataBind();

        if (Session["CurrentTagCat"] != null)
        {
            ddlTagCategory.SelectedValue = Convert.ToString(Session["CurrentTagCat"]);
        }
    }
    public string procGet(string name)
    {

        return "[field: " + ddlTagCategory.SelectedValue + "." + name + "]";
    }
    private void GetTagSQLs()
    {
        BAL_AMCPE.TagSQL ts = new BAL_AMCPE.TagSQL();
        var data = ts.GetTagSQLByTagCategory(CurrentPage + 1, pageSize, sortExpression, searchKeyword, Convert.ToInt32(ddlTagCategory.SelectedValue));
        if (data.Count > 0)
        {
            rptList.DataSource = data;
            message.Visible = false;
            TotalPages = Convert.ToInt32(data.FirstOrDefault().TotalPages);
        }
        else
        {
            rptList.DataSource = null;
            message.Visible = true;
            lblMessage.Text = "No record found";
            TotalPages = 0;
        }
        rptList.DataBind();
        if (TotalPages > 1)
            BindRptPagination(TotalPages);
        else
            pnlPagination.Visible = false;
    }

    protected void ddlTagCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CurrentTagCat"] = ddlTagCategory.SelectedValue;
        GetTagSQLs();
    }

    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.TagSQL ts = new BAL_AMCPE.TagSQL();
            ts.obj = ts.GetTagSQLByID(Convert.ToInt32(e.CommandArgument));
            ts.obj.IsDeleted = true;
            ts.Save();
            Session["dataction"] = "d";
            GetTagSQLs();
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
            if (currentUser == "amc\\ahmz")
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
            GetTagSQLs();
        }
    }
    protected void lnkNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage += 1;
        GetTagSQLs();
    }
    protected void lnkPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage -= 1;
        GetTagSQLs();
    }
    protected void lnkFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = 0;
        GetTagSQLs();
    }
    protected void lnkLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = Convert.ToInt16(Convert.ToInt16(lblTotalPages.Text) - 1);
        GetTagSQLs();
    }
    protected void lnkEllipses_Click(object sender, System.EventArgs e)
    {
        CurrentPage = (short)((Convert.ToInt32(CurrentPage) / PageRange + 1) * PageRange);
        GetTagSQLs();
    }
    #endregion
}