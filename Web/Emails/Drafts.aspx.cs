using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using System.Collections;

public partial class Emails_Drafts : System.Web.UI.Page
{
    public string user;
    public int pageSize = 20, PageRange = 5;
    public string sortExpression = "order by SentOn desc", searchKeyword = "", status = "draft";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //user = Convert.ToString(Session["UserId"]).Replace("amc\\", "");
            // added to give direct access to this page

            if (!string.IsNullOrEmpty(Request.QueryString["PatientNumber"]))
            {
                Session["PatientNumber"] = Convert.ToString(Request.QueryString["PatientNumber"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["RecId"]))
            {
                Session["RecId"] = Convert.ToString(Request.QueryString["RecId"]);
            }


            if (!string.IsNullOrEmpty(Request.QueryString["UserId"]))
            {
                user = Convert.ToString(Request.QueryString["UserId"]).Replace("amc\\", "");
                Session["UserId"] = Convert.ToString(Request.QueryString["UserId"]);
            }
            else
            {
                user = Convert.ToString(Session["UserId"]).Replace("amc\\", "");
            }

            hdnUser.Value = user;
            BindData();
            if (Session["dataction"] != null)
            {
                if (Convert.ToString(Session["dataction"]) == "s")
                    lblMessage.Text = "Email Saved Successfully!!";
                else if (Convert.ToString(Session["dataction"]) == "e")
                    lblMessage.Text = "Some error occurred, email could not be sent. Please try again!!";
                message.Visible = true;
            }
            Session["dataction"] = null;
        }
        else
        {
            user = hdnUser.Value;
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

        BAL_AMCPE.Emails em = new BAL_AMCPE.Emails();
        //var data = em.GetDraftEmailsByUserId("amc\\" + user);
        var data = em.GetEmailsByUser(CurrentPage + 1, pageSize, sortExpression, searchKeyword, status, Convert.ToString(Session["UserId"]), "");
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
}