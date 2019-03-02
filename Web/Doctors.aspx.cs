using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using DAL_AMCPE;
using System.Collections;

public partial class Doctors : System.Web.UI.Page
{
    public int pageSize = 20, PageRange = 5;
    public string sortExpression = "order by Name asc", searchKeyword = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetDoctors();
        }
    }

    private void GetDoctors()
    {
        BAL_AMCPE.Doctors d = new BAL_AMCPE.Doctors();
        var data = d.GetDoctors();
        if (data.Count > 0)
        {
            rptList.DataSource = data;
            message.Visible = false;
            //TotalPages = Convert.ToInt32(data.FirstOrDefault().TotalPages);
        }
        else
        {
            rptList.DataSource = null;
            message.Visible = true;
            lblMessage.Text = "No record found";
            //TotalPages = 0;
        }
        rptList.DataBind();
        //if (TotalPages > 1)
        //    BindRptPagination(TotalPages);
        //else
        //    pnlPagination.Visible = false;
    }

    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.Doctors d = new BAL_AMCPE.Doctors();
            d.obj = d.GetDoctorByID(Convert.ToInt32(e.CommandArgument));
            d.obj.IsDeleted = true;
            d.Save();
            Session["dataction"] = "d";
            GetDoctors();
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
            GetDoctors();
        }
    }
    protected void lnkNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage += 1;
        GetDoctors();
    }
    protected void lnkPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage -= 1;
        GetDoctors();
    }
    protected void lnkFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = 0;
        GetDoctors();
    }
    protected void lnkLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = Convert.ToInt16(Convert.ToInt16(lblTotalPages.Text) - 1);
        GetDoctors();
    }
    protected void lnkEllipses_Click(object sender, System.EventArgs e)
    {
        CurrentPage = (short)((Convert.ToInt32(CurrentPage) / PageRange + 1) * PageRange);
        GetDoctors();
    }
    #endregion
}