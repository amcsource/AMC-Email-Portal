using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TemplateWordEditor : System.Web.UI.Page
{
    public string user;
    public int Id;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTagCategories();
            Id = 0;
            user = Convert.ToString(Session["UserId"]).Replace("amc\\", "");

            if (Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"])))
            {
                string data = Convert.ToString(Session["TemplateWordEditor"]);
                TextControl1.LoadTextAsync(data, TXTextControl.Web.StringStreamType.RichTextFormat);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["TemplateId"]))
                {
                    Id = Convert.ToInt32(Request.QueryString["TemplateId"]);
                    string url = "~/Attachments/Templates/" + Id + "/Letter.docx";
                    LoadTemplateLetter(url);
                }
                else if (!string.IsNullOrWhiteSpace(Request.QueryString["Url"]))
                {
                    string url = Convert.ToString(Request.QueryString["Url"]);
                    LoadTemplateLetter(url);
                }
            }
        }
    }

    private void LoadTemplateLetter(int id)
    {
        string fileName = Server.MapPath("~/Attachments/Templates/" + id + "/Letter.docx");
        if (System.IO.File.Exists(fileName))
        {
            TextControl1.LoadTextAsync(fileName, TXTextControl.Web.StreamType.WordprocessingML);
        }
    }

    private void LoadTemplateLetter(string url)
    {
        string fileName = Server.MapPath(url);
        if (System.IO.File.Exists(fileName))
        {
            TextControl1.LoadTextAsync(fileName, TXTextControl.Web.StreamType.WordprocessingML);
        }
    }

    private void BindTagCategories()
    {
        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
        var data = tc.GetTagCategories();
        if (data.Count > 0)
            ddlTagSelectorTables.DataSource = data;
        else
        {
            ddlTagSelectorTables.DataSource = null;
        }
        ddlTagSelectorTables.DataTextField = "CategoryName";
        ddlTagSelectorTables.DataValueField = "Id";
        ddlTagSelectorTables.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string data;
        TextControl1.SaveText(out data, TXTextControl.Web.StringStreamType.RichTextFormat);
        Session["TemplateWordEditor"] = data;

        ScriptManager.RegisterStartupScript(this.up1, typeof(string), "alertScript", "window.close();", true);

        //if (!string.IsNullOrEmpty(Request.QueryString["TemplateId"]))
        //{
        //Id = Convert.ToInt32(Request.QueryString["TemplateId"]);
        //string fileName = Server.MapPath("~/Attachments/Templates/" + Id + "/Letter.docx");
        //TextControl1.SaveText(fileName, TXTextControl.Web.StreamType.WordprocessingML);
        //Session["dataction"] = "s";
        //Response.Redirect("EmailTemplates.aspx");
        //}
    }
}