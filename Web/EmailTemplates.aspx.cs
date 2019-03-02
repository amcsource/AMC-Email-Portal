using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using System.Collections;

public partial class EmailTemplates : System.Web.UI.Page
{
    public int pageSize = 25, PageRange = 5;
    public string sortExpression = "order by Name asc", searchKeyword = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BAL_AMCPE.EmailTemplateCategory etc = new BAL_AMCPE.EmailTemplateCategory();
            ddlEmailTemplateCategories.DataSource = etc.GetEmailTemplateCategoriesNew();
            ddlEmailTemplateCategories.DataTextField = "CategoryName";
            ddlEmailTemplateCategories.DataValueField = "Id";
            ddlEmailTemplateCategories.DataBind();
            ddlEmailTemplateCategories.Items.Insert(0, new ListItem("All Categories", "0"));

            if (Session["CurrentPageET"] != null)
            {
                CurrentPage = Convert.ToInt16(Session["CurrentPageET"]);
                ddlPageSize.SelectedValue = Convert.ToString(Session["PageSizeET"]);
                txtSearch.Text = Convert.ToString(Session["SearchET"]);
                ddlEmailTemplateCategories.SelectedValue = Convert.ToString(Session["EmailTemplateCategoriesET"]);
                //Session["CurrentPageET"] = null;
            }

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
        searchKeyword = txtSearch.Text;
        pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        int categoryId = Convert.ToInt32(ddlEmailTemplateCategories.SelectedValue);

        if (Convert.ToString(Session["dataction"]) == "d")
        {
            lblMessage.Text = "Data deleted successfully!!";
            message.Visible = true;
            Session["dataction"] = null;
        }

        BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
        var data = et.GetEmailTemplatesByCategoryId(CurrentPage + 1, pageSize, sortExpression, searchKeyword, categoryId);
        if (data.Count > 0)
        {
            rptTemplates.DataSource = data;
            norecord.Visible = false;
            TotalPages = Convert.ToInt32(data.FirstOrDefault().TotalPages);
        }
        else
        {
            rptTemplates.DataSource = null;
            norecord.Visible = true;
            TotalPages = 0;
        }
        rptTemplates.DataBind();
        if (TotalPages > 1)
            BindRptPagination(TotalPages);
        else
            pnlPagination.Visible = false;
    }

    protected void rptTemplates_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            Session["CurrentPageET"] = CurrentPage;
            Session["PageSizeET"] = ddlPageSize.SelectedValue;
            Session["SearchET"] = txtSearch.Text;
            Session["EmailTemplateCategoriesET"] = ddlEmailTemplateCategories.SelectedValue;
            Response.Redirect("AddEditEmailTemplate.aspx?Id=" + Convert.ToInt32(e.CommandArgument), false);
        }
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
            et.obj = et.GetTemplateByID(Convert.ToInt32(e.CommandArgument));
            et.obj.DeletedBy = Convert.ToString(Session["UserId"]);
            et.obj.DeletedOn = DateTime.Now;
            et.obj.IsDeleted = true;
            et.UpdateTemplate();
            Session["dataction"] = "d";
            BindData();
        }
    }

    protected void rptTemplates_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string user = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "CreatedBy")).ToLower();
            string currentUser = Convert.ToString(Session["UserId"]).ToLower();
            
            LinkButton lbEdit = (LinkButton)e.Item.FindControl("Edit");
            lbEdit.Enabled = ((user == currentUser && PermissionSession.UserPermission.CanEditTemplate) || (user != currentUser && PermissionSession.UserPermission.CanEditOtherTemplate));

            LinkButton lbDelete = (LinkButton)e.Item.FindControl("Delete");
            //lbDelete.Enabled = ((user == currentUser && PermissionSession.CanDeleteTemplate) || (user != currentUser && PermissionSession.CanDeleteOtherTemplate));
            if ((user == currentUser && PermissionSession.UserPermission.CanDeleteTemplate) || (user != currentUser && PermissionSession.UserPermission.CanDeleteOtherTemplate))
                lbDelete.Enabled = true;
            else
            {
                lbDelete.Enabled = false;
                lbDelete.OnClientClick = null;
            }

            LinkButton lbSendEmail = (LinkButton)e.Item.FindControl("SendEmail");
            if (PermissionSession.UserPermission.CanSendEmail)
                lbSendEmail.Enabled = true;
            else
            {
                lbSendEmail.Enabled = false;
                lbSendEmail.OnClientClick = null;
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CurrentPage = 0;
        //searchKeyword = txtSearch.Text;
        BindData();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        CurrentPage = 0;
        //searchKeyword = txtSearch.Text;
        //pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        BindData();
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