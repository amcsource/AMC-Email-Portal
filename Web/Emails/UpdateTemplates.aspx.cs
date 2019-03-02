using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using System.Collections;
using DAL_AMCPE;
using System.Data;

public partial class UpdateTemplates : System.Web.UI.Page
{
    public int pageSize = 25, PageRange = 5;
    public string sortExpression = "order by TemplateName asc", searchKeyword = "", prescription_parent="";
    public DataSet dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            prescription_parent = ddlxpres.SelectedValue;
            BAL_AMCPE.DocumentTemplates etc3 = new BAL_AMCPE.DocumentTemplates();
            List<xsoPrescription> hhh = etc3.GetTemplateReplaceKw(prescription_parent);

            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataSet ds = new DataSet();
            ds.Tables.Add(converter.ToDataTable(hhh));
            //xsoPrescription etc2 = new xsoPrescription();

            foreach (DataRow d in ds.Tables[0].Rows)
            {
                ListItem li1 = new ListItem(d[0].ToString(), d[0].ToString());
                cblCustomerList.Items.Add(li1);
            }

            //BAL_AMCPE.EmailTemplateCategory etc = new BAL_AMCPE.EmailTemplateCategory();
            //cblCustomerList.DataSource = etc.GetEmailTemplateCategoriesNew();
            //cblCustomerList.DataTextField = "CategoryName";
            //cblCustomerList.DataValueField = "Id";
            //cblCustomerList.DataBind();
            //cblCustomerList.Items.Insert(0, new ListItem("All Categories", "0"));



            //GMEEDevelopmentEntities etc2 = new GMEEDevelopmentEntities();

            BAL_AMCPE.DocumentTemplates etc1 = new BAL_AMCPE.DocumentTemplates();
            ddlDocTemplate.DataSource = etc1.GetAllDocTemplates();
            ddlDocTemplate.DataTextField = "TemplateName";
            ddlDocTemplate.DataValueField = "Id";
            ddlDocTemplate.DataBind();
            ddlDocTemplate.Items.Insert(0, new ListItem("All Templates", "0"));


            if (Session["CurrentPageET"] != null)
            {
                CurrentPage = Convert.ToInt16(Session["CurrentPageET"]);
                ddlPageSize.SelectedValue = Convert.ToString(Session["PageSizeET"]);
                txtSearch.Text = Convert.ToString(Session["SearchET"]);
              
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
       
      
        
       
      
       

       
        //cblCustomerList.DataTextField = "RecId";
        //cblCustomerList.DataTextField = "RecId";
     
        cblCustomerList.DataBind();
        //cblCustomerList.Items.Insert(0, new ListItem("All Template", "0"));
    }
    public  DataTable ConvertListToDataTable(List<xsoPrescription[]> list)
    {
        // New table.
        DataTable table = new DataTable();

        // Get max columns.
        int columns = 0;
        foreach (var array in list)
        {
            if (array.Length > columns)
            {
                columns = array.Length;
            }
        }

        // Add columns.
        for (int i = 0; i < columns; i++)
        {
            table.Columns.Add();
        }

        // Add rows.
        foreach (var array in list)
        {
            table.Rows.Add(array);
        }

        return table;
    }
    public  string GetSelectedValues(CheckBoxList checkBoxList)
    {
        string retval = string.Empty;

        try
        {
            retval = checkBoxList.Items.Cast<ListItem>().Where(item => item.Selected).Aggregate(retval, (current, item) => current + (item.Value + ','));
            retval.TrimEnd(',');
        }
        catch (Exception)
        {
        }

        return retval;
    }
    private void BindData()
    {
        searchKeyword = txtSearch.Text;
        pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        int categoryId = 0;

        if (Convert.ToString(Session["dataction"]) == "d")
        {
            lblMessage.Text = "Data deleted successfully!!";
            message.Visible = true;
            Session["dataction"] = null;
        }

        BAL_AMCPE.DocumentTemplates et1 = new BAL_AMCPE.DocumentTemplates();
       // BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
        var data = et1.GetDocumentTemplatesList(CurrentPage + 1, pageSize, sortExpression, searchKeyword,categoryId);
        if (data.Count > 0)
        {
            rptTemplates.DataSource = data;
            norecord.Visible = false;
          
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
          //  Session["EmailTemplateCategoriesET"] = ddlEmailTemplateCategories.SelectedValue;
            Response.Redirect("AddEditDocumentTemplate.aspx?Id=" + Convert.ToInt32(e.CommandArgument), false);
        }
        if (e.CommandName == "Delete")
        {
            BAL_AMCPE.DocumentTemplates et1 = new BAL_AMCPE.DocumentTemplates();
            DAL_AMCPE.DocumentTemplate et = new DAL_AMCPE.DocumentTemplate();
            et= et1.GetDocTemplateByID(Convert.ToInt32(e.CommandArgument));
           
            et1.UpdateTemplate(et);
            Session["dataction"] = "d";
            BindData();
        }
    }

    protected void rptTemplates_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //{
        //    string user = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "CreatedBy")).ToLower();
        //    string currentUser = Convert.ToString(Session["UserId"]).ToLower();
            
        //    LinkButton lbEdit = (LinkButton)e.Item.FindControl("Edit");
        //    lbEdit.Enabled = ((user == currentUser && PermissionSession.UserPermission.CanEditTemplate) || (user != currentUser && PermissionSession.UserPermission.CanEditOtherTemplate));

        //    LinkButton lbDelete = (LinkButton)e.Item.FindControl("Delete");
        //    //lbDelete.Enabled = ((user == currentUser && PermissionSession.CanDeleteTemplate) || (user != currentUser && PermissionSession.CanDeleteOtherTemplate));
        //    if ((user == currentUser && PermissionSession.UserPermission.CanDeleteTemplate) || (user != currentUser && PermissionSession.UserPermission.CanDeleteOtherTemplate))
        //        lbDelete.Enabled = true;
        //    else
        //    {
        //        lbDelete.Enabled = false;
        //        lbDelete.OnClientClick = null;
        //    }

        //    LinkButton lbSendEmail = (LinkButton)e.Item.FindControl("SendEmail");
        //    if (PermissionSession.UserPermission.CanSendEmail)
        //        lbSendEmail.Enabled = true;
        //    else
        //    {
        //        lbSendEmail.Enabled = false;
        //        lbSendEmail.OnClientClick = null;
        //    }
        //}
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
      string dds=  GetSelectedValues(cblCustomerList);
      prescription_parent = ddlxpres.SelectedValue;
        
       BAL_AMCPE.DocumentTemplates bb=new BAL_AMCPE.DocumentTemplates();
       string cc = bb.GetDocTemplatePathByID(Convert.ToInt32(ddlDocTemplate.SelectedValue));

       Response.Write(dds + "," + prescription_parent + "," + cc);
      int lastSelectedIndex = 0;
      string lastSelectedValue = string.Empty;

      foreach (ListItem listitem in cblCustomerList.Items)
      {
          if (listitem.Selected)
          {
              int thisIndex = cblCustomerList.Items.IndexOf(listitem);

              if (lastSelectedIndex < thisIndex)
              {
                  lastSelectedIndex = thisIndex;
                  lastSelectedValue = listitem.Value;
              }
          }
      }
        CurrentPage = 0;
        searchKeyword = txtSearch.Text;
        BindData();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        cblCustomerList.Items.Clear();
        prescription_parent = ddlxpres.SelectedValue;
        BAL_AMCPE.DocumentTemplates etc3 = new BAL_AMCPE.DocumentTemplates();
        List<xsoPrescription> hhh = etc3.GetTemplateReplaceKw(prescription_parent);

        ListtoDataTableConverter converter = new ListtoDataTableConverter();
        DataSet ds = new DataSet();
        ds.Tables.Add(converter.ToDataTable(hhh));
        //xsoPrescription etc2 = new xsoPrescription();

        foreach (DataRow d in ds.Tables[0].Rows)
        {
            ListItem li1 = new ListItem(d[0].ToString(), d[0].ToString());
            cblCustomerList.Items.Add(li1);
        }
        

        CurrentPage = 0;
        //searchKeyword = txtSearch.Text;
        //pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
       // BindData();
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

    protected void ddlDocTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int prescription_parent = int.Parse(ddlDocTemplate.SelectedValue);
    }
    protected void ddlxpres_SelectedIndexChanged(object sender, EventArgs e)
    {
        string prescription_parent = ddlxpres.SelectedValue;
    }
}